using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GF.Scheduling
{
    public class Employee
    {
        public string Name { get; set; } = string.Empty;

        /// <summary>Працівник на «повний етап» (full‑time)?</summary>
        public bool IsFullTime { get; set; }

        // Дані, що розраховуються під час генерації
        public int AssignedMinutes { get; set; } = 0;
        internal int ConsecutiveWorkingDays { get; set; } = 0;

        public int AssignedHours => AssignedMinutes / 60;

        public int ConsecutiveFullDays { get; set; }   // NEW  – лічильник «9-21» підряд

        public int FullShiftCount { get; set; }     // скільки «9-21» уже поставили
    }
}
