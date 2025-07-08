using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GF.Scheduling
{
    /// <summary>DTO-модель для серіалізації розкладу.</summary>
    public sealed class SavedSchedule
    {
        /* ───────────── метадані ───────────── */
        public string Name { get; set; }        // Назва, яку ввів користувач
        public DateTime SavedAt { get; set; }        // Дата-час збереження
        public int Year { get; set; }
        public int Month { get; set; }

        /* ───────────── сам розклад ─────────── */
        public string[] ColumnHeaders { get; set; }          // назви стовпців
        public string[,] Values { get; set; }          // [row,col] вміст
        public int[] ConflictDays { get; set; }          // номери днів з конфліктами

        public Dictionary<string, string> BackColors { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> ForeColors { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> Borders { get; set; } = new Dictionary<string, string>();

        public Dictionary<string, string> Comments { get; set; } = new Dictionary<string, string>();    // "col_row" -> comment

        /* ───── фабрика → DTO ───── */
        public static SavedSchedule From(ScheduleResult src, string displayName)
        {
            if (src == null) throw new ArgumentNullException(nameof(src));

            /* 1. Читаємо таблицю */
            DataTable tbl = src.Table;
            int rows = tbl.Rows.Count;
            int cols = tbl.Columns.Count;

            var headers = new string[cols];
            for (int c = 0; c < cols; c++)
                headers[c] = tbl.Columns[c].ColumnName;

            var vals = new string[rows, cols];
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                    vals[r, c] = tbl.Rows[r][c]?.ToString() ?? string.Empty;

            /* 2. Повертаємо DTO */
            DateTime now = DateTime.Now;
            return new SavedSchedule
            {
                Name = displayName,
                SavedAt = now,
                Year = now.Year,
                Month = now.Month,
                ColumnHeaders = headers,
                Values = vals,
                ConflictDays = src.ConflictDays.ToArray()
            };
        }

        /* ───── відновлення ← DTO (за потреби) ───── */
        public ScheduleResult ToScheduleResult()
        {
            var result = new ScheduleResult();

            /* колонки */
            foreach (string h in ColumnHeaders)
                result.Table.Columns.Add(h);

            /* рядки */
            int rowCnt = Values.GetLength(0);
            int colCnt = Values.GetLength(1);

            for (int r = 0; r < rowCnt; r++)
            {
                var row = result.Table.NewRow();
                for (int c = 0; c < colCnt; c++)
                    row[c] = Values[r, c];
                result.Table.Rows.Add(row);
            }

            /* конфліктні дні */
            foreach (int d in ConflictDays)
                result.ConflictDays.Add(d);

            return result;
        }
    }
}
