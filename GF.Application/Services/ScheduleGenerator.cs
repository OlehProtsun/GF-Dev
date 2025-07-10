// GF.Application/Services/ScheduleGenerator.cs
using GF.Application.Interfaces;
using GF.Domain.Scheduling;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GF.Application.Services;

/// <summary>
///  Алгоритм формування графіка без жодних WinForms-залежностей.
/// </summary>
public sealed class ScheduleGenerator : IScheduleGenerator
{
    // ───────── бізнес-константи ─────────
    private const int MonthlyHourLimit = 200;
    private const int FullTimeTargetHours = 170;
    private const int MaxConsecutiveDays = 6;
    private const int MaxFullShiftsPerMonth = 5;

    // ───────── API ─────────
    public ScheduleResult Generate(
        IReadOnlyList<Employee> employees,
        IReadOnlyList<DayAvailability> dispo,
        ScheduleParameters p)
    {
        int days = dispo.Max(d => d.Day);

        // 1) результат
        var result = new ScheduleResult
        {
            Table = BuildResultTable(days)
        };

        // 2) швидкий індекс доступності:  name -> array[day] = "+|-|9-21"
        var map = BuildDispoDictionary(dispo, days);

        // 3) обнуляємо лічильники
        ResetEmployeeCounters(employees);

        // 4) цикл по днях
        for (int day = 1; day <= days; day++)
        {
            var morning = new List<Employee>();
            var evening = new List<Employee>();

            AssignShiftForDay(day, map, employees, p, morning, evening, isMorning: true);
            AssignShiftForDay(day, map, employees, p, morning, evening, isMorning: false);

            if (morning.Count < p.WorkersPerShift || evening.Count < p.WorkersPerShift)
                result.ConflictDays.Add(day);

            WriteDay(result.Table, employees, p, day, morning, evening);
            UpdateStreaks(employees, morning, evening);
        }

        return result;
    }

    // ───────── ядро ─────────
    private static void AssignShiftForDay(
        int day,
        IReadOnlyDictionary<string, string[]> map,
        IReadOnlyList<Employee> employees,
        ScheduleParameters p,
        List<Employee> morning,
        List<Employee> evening,
        bool isMorning)
    {
        var thisShift = isMorning ? morning : evening;
        var otherShift = isMorning ? evening : morning;

        TimeSpan start = isMorning ? p.MorningStart : p.AfternoonStart;
        TimeSpan end = isMorning ? p.AfternoonStart : p.EveningEnd;
        int shiftMinutes = (int)(end - start).TotalMinutes;

        var candidates =
            employees
            .Where(e => e.AssignedMinutes < MonthlyHourLimit * 60)
            .Where(e => e.ConsecutiveWorkingDays < MaxConsecutiveDays)
            .Where(e =>
                   map.TryGetValue(e.Name, out var arr) &&
                   IsAvailable(arr[day], start, end))
            .Select(e => new
            {
                Emp = e,
                IsPriority = e.IsFullTime && e.AssignedMinutes < FullTimeTargetHours * 60
            })
            .OrderBy(x => x.IsPriority ? 0 : 1)
            .ThenBy(x => x.Emp.AssignedMinutes)
            .Select(x => x.Emp)
            .ToList();

        foreach (var emp in candidates)
        {
            if (thisShift.Count >= p.WorkersPerShift) break;

            bool alreadyOther = otherShift.Contains(emp);
            bool fitsMonthLimit = emp.AssignedMinutes + shiftMinutes <= MonthlyHourLimit * 60;
            bool nonConsecFull = emp.ConsecutiveFullDays == 0;
            bool underFullCap = emp.FullShiftCount < MaxFullShiftsPerMonth;

            bool allowDouble = alreadyOther && emp.IsFullTime && nonConsecFull && underFullCap && fitsMonthLimit;
            bool allowSingle = !alreadyOther && fitsMonthLimit;

            if (allowSingle || allowDouble)
            {
                thisShift.Add(emp);
                emp.AssignedMinutes += shiftMinutes;
            }
        }
    }

    // ───────── лічильники ─────────
    private static void ResetEmployeeCounters(IEnumerable<Employee> emps)
    {
        foreach (var e in emps)
        {
            e.AssignedMinutes = 0;
            e.ConsecutiveWorkingDays = 0;
            e.ConsecutiveFullDays = 0;
            e.FullShiftCount = 0;
        }
    }

    private static void UpdateStreaks(
        IEnumerable<Employee> emps,
        ICollection<Employee> morning,
        ICollection<Employee> evening)
    {
        foreach (var e in emps)
        {
            bool m = morning.Contains(e);
            bool v = evening.Contains(e);
            bool worked = m || v;
            bool full = m && v;

            e.ConsecutiveWorkingDays = worked ? e.ConsecutiveWorkingDays + 1 : 0;

            if (full)
            {
                e.ConsecutiveFullDays += 1;
                e.FullShiftCount += 1;
            }
            else
            {
                e.ConsecutiveFullDays = 0;
            }
        }
    }

    // ───────── helpers ─────────
    private static DataTable BuildResultTable(int days)
    {
        var t = new DataTable();
        t.Columns.Add("Employee", typeof(string));
        t.Columns.Add("Hours", typeof(int));
        for (int d = 1; d <= days; d++)
            t.Columns.Add(d.ToString(), typeof(string));
        return t;
    }

    private static void WriteDay(
        DataTable table,
        IEnumerable<Employee> emps,
        ScheduleParameters p,
        int day,
        ICollection<Employee> morning,
        ICollection<Employee> evening)
    {
        foreach (var emp in emps)
        {
            var row = table.Rows.Cast<DataRow>()
                        .FirstOrDefault(r => (string)r["Employee"] == emp.Name)
                   ?? table.Rows.Add(emp.Name, 0);

            string mark = morning.Contains(emp) && evening.Contains(emp) ? Format(p.MorningStart, p.EveningEnd) :
                          morning.Contains(emp) ? Format(p.MorningStart, p.AfternoonStart) :
                          evening.Contains(emp) ? Format(p.AfternoonStart, p.EveningEnd) :
                                                                             "";

            row[day + 1] = mark;
            row["Hours"] = emp.AssignedMinutes / 60;
        }
    }

    private static string Format(TimeSpan s, TimeSpan e) => $"{(int)s.TotalHours}-{(int)e.TotalHours}";

    private static IReadOnlyDictionary<string, string[]> BuildDispoDictionary(
        IReadOnlyList<DayAvailability> dispo, int days)
    {
        // ініціюємо "+" / "-" рядками
        var dict = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);
        foreach (var d in dispo)
        {
            if (!dict.TryGetValue(d.EmployeeName, out var arr))
            {
                arr = new string[days + 1];
                dict[d.EmployeeName] = arr;
            }
            arr[d.Day] = d.MorningAvailable && d.EveningAvailable ? "+" :
                         d.MorningAvailable || d.EveningAvailable ? (d.MorningAvailable ? "9-15" : "15-21") :
                                                                     "-";
        }
        return dict;
    }

    private static bool IsAvailable(string mark, TimeSpan start, TimeSpan end)
    {
        if (string.IsNullOrWhiteSpace(mark)) return false;
        mark = mark.Trim();
        if (mark == "+") return true;
        if (mark == "-") return false;

        var parts = mark.Split('-');
        if (parts.Length != 2) return false;
        return TryHour(parts[0], out var s) &&
                TryHour(parts[1], out var e) &&
                s <= start && e >= end;

        static bool TryHour(string txt, out TimeSpan t)
        {
            txt = txt.Trim().Replace(":", ".");
            if (double.TryParse(txt, out double h))
            {
                t = TimeSpan.FromHours(h);
                return true;
            }
            t = default;
            return false;
        }
    }
}
