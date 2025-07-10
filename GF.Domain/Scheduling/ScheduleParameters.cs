using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GF.Domain.Scheduling
{
    public class ScheduleParameters
    {
        public int WorkersPerShift { get; set; }

        public TimeSpan MorningStart { get; set; }
        public TimeSpan AfternoonStart { get; set; }
        public TimeSpan EveningEnd { get; set; }

        public int MorningLengthHours => (int)(AfternoonStart - MorningStart).TotalHours;
        public int EveningLengthHours => (int)(EveningEnd - AfternoonStart).TotalHours;
        public int DoubleLengthHours => MorningLengthHours + EveningLengthHours;
    }
}
