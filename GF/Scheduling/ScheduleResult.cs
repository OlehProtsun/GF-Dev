using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GF.Scheduling { 
    public class ScheduleResult
    {
        public DataTable Table { get; set; }    // без  default!
        public HashSet<int> ConflictDays { get; } = new HashSet<int>();

        public ScheduleResult()
        {
            Table = new DataTable();            // ініціалізуємо у конструкторі
        }
    }
}

