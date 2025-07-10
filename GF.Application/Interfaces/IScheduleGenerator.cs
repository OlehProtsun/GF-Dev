using GF.Domain.Scheduling;

namespace GF.Application.Interfaces
{
    public interface IScheduleGenerator
    {
        ScheduleResult Generate(
            IReadOnlyList<Employee> employees,
            IReadOnlyList<DayAvailability> dispo,
            ScheduleParameters p);
    }
}