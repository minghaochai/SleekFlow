using Microsoft.EntityFrameworkCore;
using SleekFlow.Domain.Entities;

namespace SleekFlow.Infrastructure
{
    public class SleekFlowDbContext : DbContext, ISleekFlowDbContext
    {
        public SleekFlowDbContext()
        {
        }

        public SleekFlowDbContext(DbContextOptions<SleekFlowDbContext> options)
            : base(options)
        {
        }

        public DbSet<ToDo> ToDos { get; set; }

        public SleekFlowDbContext Instance => this;

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
