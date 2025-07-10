using GF.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GF
{
    // SchedulePresenter.cs  (у GF)
    public class SchedulePresenter
    {
        private readonly IScheduleGenerator _gen;

        public SchedulePresenter(IScheduleGenerator gen) => _gen = gen;

        public ScheduleResult Build(DataGridView dispoGrid,
                                    List<Employee> employees,
                                    ScheduleParameters p)
        {
            var dispo = ToDto(dispoGrid);
            return _gen.Generate(employees, dispo, p);
        }

        private static List<DayAvailability> ToDto(DataGridView g)
        {
            int days = g.Columns.Count - 1;
            var list = new List<DayAvailability>();

            foreach (DataGridViewRow r in g.Rows)
            {
                string name = r.Cells[0].Value?.ToString()?.Trim();
                if (string.IsNullOrWhiteSpace(name)) continue;

                for (int d = 1; d <= days; d++)
                {
                    string mark = r.Cells[d].Value?.ToString()?.Trim() ?? "";
                    list.Add(new DayAvailability(
                        name, d,
                        mark == "+" || mark == "9-15" || mark == "9-21",
                        mark == "+" || mark == "15-21" || mark == "9-21"));
                }
            }
            return list;
        }
    }

}
