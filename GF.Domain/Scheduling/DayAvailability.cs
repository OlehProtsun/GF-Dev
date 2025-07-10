using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GF.Domain.Scheduling
{
    public record DayAvailability(
        string EmployeeName,
        int Day,                 // 1-based
        bool MorningAvailable,
        bool EveningAvailable);
}
