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
        /* синхронний API (старий) */
        void Save(SavedSchedule dto);
        IReadOnlyList<SavedSchedule> LoadAll();

        /* асинхронний API (новий) */
        Task SaveAsync(SavedSchedule dto, CancellationToken ct = default);
        Task<IReadOnlyList<SavedSchedule>> LoadAllAsync(CancellationToken ct = default);

        /* опціонально */
        Task<SavedSchedule?> LoadAsync(string name, CancellationToken ct = default);
        Task DeleteAsync(string name, CancellationToken ct = default);
    }
}
