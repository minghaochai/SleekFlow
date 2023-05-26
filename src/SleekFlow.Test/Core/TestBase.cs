using Microsoft.EntityFrameworkCore;
using SleekFlow.Infrastructure;

namespace SleekFlow.Test.Core
{
    public class TestBase
    {
        protected readonly DbContextOptions<SleekFlowDbContext> DbContextOptions;

        public TestBase()
        {
            DbContextOptions = new DbContextOptionsBuilder<SleekFlowDbContext>()
                .UseInMemoryDatabase("SleekFlow")
                .Options;
        }

        protected void ResetDatabase(SleekFlowDbContext sleekFlowDbContext)
        {
            sleekFlowDbContext.Database.EnsureDeleted();
            sleekFlowDbContext.Database.EnsureCreated();
        }
    }
}
