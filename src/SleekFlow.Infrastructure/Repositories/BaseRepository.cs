using Microsoft.EntityFrameworkCore;
using SleekFlow.Domain.Entities;
using SleekFlow.Domain.Filters;
using SleekFlow.Domain.Interfaces;

namespace SleekFlow.Infrastructure.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T>
        where T : BaseEntity
    {
        private readonly DbSet<T> DbSet;

        private readonly SleekFlowDbContext Context;

        public BaseRepository(ISleekFlowDbContext context)
        {
            Context = context.Instance;
            DbSet = Context.Set<T>();
        }

        public virtual async Task<T> GetById(int id)
        {
            return await DbSet.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        public async virtual Task<(IEnumerable<T> Results, long Count)> GetPageList(BasePageFilter filter)
        {
            var filterExp = filter.ToFilterExpression<T>();
            var result = await DbSet
                .Filter(filterExp)
                .HandleSort(filter.SortColumn, filter.SortDirection)
                .Page(filter.PageNumber, filter.ItemsPerPage)
                .AsNoTracking()
                .ToListAsync();
            var count = await DbSet.Filter(filterExp).AsNoTracking().CountAsync();
            return (result, count);
        }

        public async virtual Task<int> Insert(T entity)
        {
            int result = 0;
            try
            {
                var entry = Context.Entry(entity);
                if (entry.State != EntityState.Detached)
                {
                    entry.State = EntityState.Added;
                }
                else
                {
                    await DbSet.AddAsync(entity);
                }

                result = await Context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }

            return result;
        }

        public async virtual Task<int> Update(T entity)
        {
            int result = 0;
            try
            {
                var entry = Context.Entry(entity);
                if (entry.State == EntityState.Detached)
                {
                    var set = Context.Set<T>();
                    T attachedEntity = await set.FindAsync(entity.Id);

                    if (attachedEntity != null)
                    {
                        Context.Entry(attachedEntity).CurrentValues.SetValues(entity);
                    }
                    else
                    {
                        entry.State = EntityState.Modified;
                    }
                }

                result = await Context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }

            return result;
        }

        public async virtual Task<int> Delete(T entity)
        {
            int result = 0;
            try
            {
                var entry = Context.Entry(entity);
                if (entry.State != EntityState.Detached)
                {
                    entry.State = EntityState.Deleted;
                }
                else
                {
                    DbSet.Remove(entity);
                }

                result = await Context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }

            return result;
        }
    }
}
