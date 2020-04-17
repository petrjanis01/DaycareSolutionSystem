using System.Linq;
using System.Threading;
using DaycareSolutionSystem.Database.DataContext;
using DaycareSolutionSystem.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DaycareSolutionSystem.Api.Host.DatabaseValidation
{
    // This class is used in startup process and has to be singleton. To avoid standard approach it's passed via DI container.
    // Because of this interface has to be used and db context cannot be passed since is registered as scoped service.
    public class RuntimeDatabaseValidator : IRuntimeDatabaseValidator
    {
        private IConfiguration _configuration;

        public RuntimeDatabaseValidator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void EnsureRuntimeDatabaseValidity()
        {
            // This workaround allows service to start inside container without crashing on db connection for the first time.
            // Problem (docker) - Even that api service is depends on db service postgres default behaviour is that it reset itself after first start.
            // This results in small time period when db service is unavailable and api startup process crashes.
            Thread.Sleep(5 * 1000);

            var dbContextOptions = new DbContextOptionsBuilder<DssDataContext>()
                .UseNpgsql(_configuration.GetValue<string>("DssConnectionString"))
                .Options;

            var dbContext = new DssDataContext(dbContextOptions);

            // checks if db is created and does migration if needed
            dbContext.Database.EnsureCreated();

            // just a simple check => not reliable
            var demoDataValid = dbContext.RegisteredClientActions.Any();

            if (demoDataValid == false)
            {
                var initializer = new DemoDataInitializer(dbContext);
                initializer.CreateDemoData();
            }
        }
    }
}
