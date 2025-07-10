using GF.Domain.Scheduling;
using GF.Domain.Month;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GF.Domain.Month
{
    /// <summary>
    /// Описує «контейнер»-місяць, що його створює користувач.
    /// </summary>
    public class MonthContainer
    {
        public int Month { get; }
        public int Year { get; }
        public string Name { get; }

        /// <summary>Унікальні працівники, прив’язані до контейнера.</summary>
        public List<string> Employees { get; } = new List<string>();

        /// <summary>Усі згенеровані для цього місяця графіки.</summary>
        public List<SavedSchedule> Schedules { get; } = new List<SavedSchedule>();   // NEW

        public MonthContainer(int month, int year, string name)
        {
            Month = month;
            Year = year;
            Name = name;
        }

        public override string ToString() => Name;
    }
}
