using GF.Domain.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GF.Infrastructure.Interfaces
{
    public interface IScheduleRepository
    {
        void Save(SavedSchedule schedule);
        IReadOnlyList<SavedSchedule> LoadAll();
    }
}
