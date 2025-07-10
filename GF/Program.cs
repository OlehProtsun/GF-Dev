global using GF.Domain.Scheduling;
global using GF.Domain.Month;
global using GF.Domain.Utils;

global using GF.Application.Interfaces;
global using GF.Application.Services;

using System;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using GF.Infrastructure.Interfaces;
using GF.Infrastructure;              // де лежить JsonScheduleRepository
using GF.UI;                          // форми

namespace GF
{
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            // 1) Generic Host + Serilog
            var hostBuilder = Host.CreateDefaultBuilder(args)
                .UseSerilog((ctx, lc) => lc
                   .MinimumLevel.Debug()
                   .WriteTo.Debug()
                   .WriteTo.File("Logs/log-.txt",
                                 rollingInterval: RollingInterval.Day,
                                 retainedFileCountLimit: 7))
                .ConfigureServices((ctx, services) =>
                {
                    // тимчасово використовуємо існуючий JsonScheduleRepository
                    services.AddSingleton<IScheduleRepository, FileScheduleRepository>();

                    // форми
                    services.AddSingleton<FormMain>();
                    services.AddTransient<AddEmployeeDialog>();
                    services.AddTransient<MonthCreationDialog>();
                    services.AddSingleton<IScheduleGenerator, ScheduleGenerator>();
                    services.AddTransient<SchedulePresenter>();
                });

            using IHost host = hostBuilder.Build();


            // 2) Стандартна WinForms 4.x ініціалізація
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            // 3) Запускаємо головну форму через DI
            var mainForm = host.Services.GetRequiredService<FormMain>();
            System.Windows.Forms.Application.Run(mainForm);
        }
    }
}
