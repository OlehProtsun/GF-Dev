global using GF.Domain.Scheduling;
global using GF.Domain.Month;
global using GF.Domain.Utils;
global using GF.Application.Interfaces;
global using GF.Application.Services;   
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GF.UI;
using Microsoft.Extensions.DependencyInjection;


namespace GF
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var services = new ServiceCollection()
                .AddSingleton<IScheduleGenerator, ScheduleGenerator>()
                .AddTransient<SchedulePresenter>()
                .AddTransient<FormMain>()
                .AddTransient<FormPageSecond>();

            using var provider = services.BuildServiceProvider();

            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            System.Windows.Forms.Application.Run(provider.GetRequiredService<FormMain>());

        }
    }
}
