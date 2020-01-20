using System;
using System.IO;
using DaycareSolutionSystem.Database.DataContext;
using DaycareSolutionSystem.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DaycareSolutionSystem.Database.Migrator
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = SetupConfiguration();

            var serviceProvider = new ServiceCollection()
                .AddDbContext<DssDataContext>(options => options.UseNpgsql(configuration.GetConnectionString("DssConnectionString")))
                .AddSingleton<IDemoDataInitializer, DemoDataInitializer>()
                .AddSingleton<IImageFetcherService, ImageFetcherService>()
                .BuildServiceProvider();

            var initializer = serviceProvider.GetService<IDemoDataInitializer>();
            initializer.DatabaseInit();
            initializer.CreateDemoData();
        }

        private static IConfigurationRoot SetupConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }
    }
}
