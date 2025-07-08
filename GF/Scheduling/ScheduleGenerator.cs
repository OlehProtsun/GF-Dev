using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GF.Scheduling
{
    internal static class ScheduleGenerator
    {
        // ────────────────────────── бізнес-константи ──────────────────────────
        private const int MonthlyHourLimit = 200; // граничний ліміт год/міс
        private const int FullTimeTargetHours = 170; // скільки треба добрати full-time
        private const int MaxConsecutiveDays = 6;   // не допускаємо 7-й день поспіль
        private const int MaxFullShiftsPerMonth = 5;   // 9-21 (double) за місяць

        // ───────────────────────────── API ─────────────────────────────
        public static ScheduleResult Generate(
            IReadOnlyList<Employee> employees,
            DataGridView dispoGrid,
            ScheduleParameters p)
        {
            int days = dispoGrid.Columns.Count - 1;           // перша колонка — ім’я

            // 1. таблиця-результат
            var result = new ScheduleResult
            {
                Table = BuildResultTable(days)
            };

            // 2. словник доступності
            var dispo = BuildDispoDictionary(dispoGrid, days);

            // 3. скидаємо лічильники
            ResetEmployeeCounters(employees);

            // 4. основний цикл по днях
            for (int day = 1; day <= days; day++)
            {
                var morning = new List<Employee>();
                var evening = new List<Employee>();

                AssignShiftForDay(day, dispo, employees, p, morning, evening, isMorning: true);
                AssignShiftForDay(day, dispo, employees, p, morning, evening, isMorning: false);

                // конфлікт?
                if (morning.Count < p.WorkersPerShift || evening.Count < p.WorkersPerShift)
                {
                    ColorConflictColumn(dispoGrid, day);
                    result.ConflictDays.Add(day);
                }

                // запис у DataTable
                WriteDay(result.Table, employees, p, day, morning, evening);

                // оновлюємо лічильники за працівником
                UpdateStreaks(employees, morning, evening);
            }

            return result;
        }

        // ──────────────────────────── ядро ────────────────────────────
        private static void AssignShiftForDay(
            int day,
            IDictionary<string, string[]> dispo,
            IReadOnlyList<Employee> employees,
            ScheduleParameters p,
            List<Employee> morningAssigned,
            List<Employee> eveningAssigned,
            bool isMorning)
        {
            var thisShift = isMorning ? morningAssigned : eveningAssigned;
            var otherShift = isMorning ? eveningAssigned : morningAssigned;

            TimeSpan start = isMorning ? p.MorningStart : p.AfternoonStart;
            TimeSpan end = isMorning ? p.AfternoonStart : p.EveningEnd;
            int shiftMinutes = (int)(end - start).TotalMinutes;

            // відбір та сортування
            var candidates = employees
                .Where(emp => emp.AssignedMinutes < MonthlyHourLimit * 60)
                .Where(emp => emp.ConsecutiveWorkingDays < MaxConsecutiveDays)
                .Where(emp =>
                {
                    return dispo.TryGetValue(emp.Name, out var arr)        // є рядок у словнику?
                           && IsAvailable(arr?[day], start, end);          // і він покриває інтервал
                })
                .Select(emp => new            // додаємо ознаку пріоритету
                {
                    Emp = emp,
                    IsPriority = emp.IsFullTime &&
                                 emp.AssignedMinutes < FullTimeTargetHours * 60
                })
                .OrderBy(x => x.IsPriority ? 0 : 1)          // full-time, що не добрали
                .ThenBy(x => x.Emp.AssignedMinutes)          // далі — найменше годин
                .Select(x => x.Emp)
                .ToList();

            foreach (var emp in candidates)
            {
                if (thisShift.Count >= p.WorkersPerShift) break;

                bool alreadyOnOther = otherShift.Contains(emp);
                bool fitsMonthlyLimit = emp.AssignedMinutes + shiftMinutes
                                           <= MonthlyHourLimit * 60;
                bool nonConsecutiveFull = emp.ConsecutiveFullDays == 0;
                bool belowFullShiftCap = emp.FullShiftCount < MaxFullShiftsPerMonth;

                bool allowDouble = alreadyOnOther &&
                                   emp.IsFullTime &&
                                   nonConsecutiveFull &&
                                   belowFullShiftCap &&
                                   fitsMonthlyLimit;

                bool allowSingle = !alreadyOnOther && fitsMonthlyLimit;

                if (allowSingle || allowDouble)
                {
                    thisShift.Add(emp);
                    emp.AssignedMinutes += shiftMinutes;
                }
            }
        }

        // ──────────────────────── лічильники працівника ────────────────────────
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
            foreach (var emp in emps)
            {
                bool workedMorning = morning.Contains(emp);
                bool workedEvening = evening.Contains(emp);
                bool worked = workedMorning || workedEvening;
                bool fullShift = workedMorning && workedEvening;

                emp.ConsecutiveWorkingDays = worked
                    ? emp.ConsecutiveWorkingDays + 1
                    : 0;

                if (fullShift)
                {
                    emp.ConsecutiveFullDays += 1;
                    emp.FullShiftCount += 1;
                }
                else
                {
                    emp.ConsecutiveFullDays = 0;
                }
            }
        }

        // ────────────────────────── DataTable helpers ──────────────────────────
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
                DataRow row = table.Rows.Cast<DataRow>()
                               .FirstOrDefault(r => (string)r["Employee"] == emp.Name)
                             ?? table.Rows.Add(emp.Name, 0);

                string mark = "";
                if (morning.Contains(emp) && evening.Contains(emp))
                    mark = FormatShift(p.MorningStart, p.EveningEnd);        // 9-21
                else if (morning.Contains(emp))
                    mark = FormatShift(p.MorningStart, p.AfternoonStart);    // 9-15
                else if (evening.Contains(emp))
                    mark = FormatShift(p.AfternoonStart, p.EveningEnd);      // 15-21

                row[day + 1] = mark;                // +1: «Employee» та «Hours»
                row["Hours"] = emp.AssignedMinutes / 60;
            }
        }

        // ─────────────────────────── доступність ───────────────────────────
        private static bool IsAvailable(string dispoText, TimeSpan start, TimeSpan end)
        {
            if (string.IsNullOrWhiteSpace(dispoText)) return false;

            dispoText = dispoText.Trim();
            if (dispoText == "+") return true;
            if (dispoText == "-") return false;

            string[] parts = dispoText.Split('-');
            if (parts.Length != 2) return false;
            if (!TryParseHour(parts[0], out var availStart)) return false;
            if (!TryParseHour(parts[1], out var availEnd)) return false;

            return availStart <= start && availEnd >= end;
        }

        private static bool TryParseHour(string part, out TimeSpan ts)
        {
            part = part.Trim().Replace(":", ".");
            if (double.TryParse(part, out double h))
            {
                ts = TimeSpan.FromHours(h);
                return true;
            }
            ts = default;
            return false;
        }

        // ────────────────────────── побічні методи ──────────────────────────
        private static Dictionary<string, string[]> BuildDispoDictionary(
            DataGridView grid, int days)
        {
            var dict = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

            foreach (DataGridViewRow row in grid.Rows)
            {
                string name = row.Cells[0].Value?.ToString()?.Trim() ?? "";
                if (string.IsNullOrWhiteSpace(name)) continue;

                var arr = new string[days + 1];          // arr[0] не використовується
                for (int c = 1; c <= days; c++)
                    arr[c] = row.Cells[c].Value?.ToString() ?? "";
                dict[name] = arr;
            }
            return dict;
        }

        private static void ColorConflictColumn(DataGridView dgv, int day)
        {
            int col = day;                               // 0 — ім’я
            if (col >= dgv.Columns.Count) return;

            foreach (DataGridViewRow row in dgv.Rows)
                row.Cells[col].Style.BackColor = System.Drawing.Color.Orange;
        }

        private static string FormatShift(TimeSpan s, TimeSpan e) =>
            $"{(int)s.TotalHours}-{(int)e.TotalHours}";
    }
}
