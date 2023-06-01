using SleekFlow.Domain.Entities;
using SleekFlow.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace SleekFlow.IntegrationTest.Helpers
{
    public static class Utilities<TEntity>
        where TEntity : BaseEntity
    {
        public static void InitializeDb(SleekFlowDbContext db, List<TEntity> SeedingData, string tableName)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                var dbSet = db.Instance.Set<TEntity>();
                dbSet.AddRange(SeedingData);
                db.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT {tableName} ON;");
                db.SaveChanges();
                db.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT {tableName} OFF");
                transaction.Commit();
            }
        }

        public static void DropAndRecreateDb(SleekFlowDbContext db)
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }

        public static void ReinitializeDb(SleekFlowDbContext db, List<TEntity> SeedingData, string tableName)
        {
            var dbSet = db.Instance.Set<TEntity>();
            db.RemoveRange(dbSet);
            InitializeDb(db, SeedingData, tableName);
        }
    }
}
