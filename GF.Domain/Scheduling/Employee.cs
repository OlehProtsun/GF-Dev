using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GF.Domain.Scheduling
{
    public class Employee
    {
        public string Name { get; init; } = string.Empty;

        public bool IsFullTime { get; set; }

        public int AssignedMinutes { get; set; }
        public int ConsecutiveWorkingDays { get; set; }
        public int ConsecutiveFullDays { get; set; }
        public int FullShiftCount { get; set; }
    }
}
