using DaycareSolutionSystem.Database.DataContext;
using Microsoft.EntityFrameworkCore;

namespace DaycareSolutionSystem.Api.Host.Test
{
    public class ApiTestBase
    {
        protected DssDataContext DataContext { get; private set; }


        protected ApiTestBase()
        {
            DataContext = GetInMemoryDataContext();
        }

        private DssDataContext GetInMemoryDataContext()
        {
            var options = GetInMemoryDataContextOptions();

            var dataContext = new DssDataContext(options);

            return dataContext;
        }

        private DbContextOptions<DssDataContext> GetInMemoryDataContextOptions()
        {
            var options = new DbContextOptionsBuilder<DssDataContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabse")
                    .Options;

            return options;
        }
    }
}
