using System.Linq;
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
            var dbContextOptions = new DbContextOptionsBuilder<DssDataContext>()
                .UseNpgsql(_configuration.GetConnectionString("DssConnectionString"))
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
