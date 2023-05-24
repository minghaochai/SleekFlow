using SleekFlow.Domain.Entities;
using SleekFlow.Domain.Filters;
using SleekFlow.Domain.Interfaces;

namespace SleekFlow.Infrastructure.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T>
        where T : BaseEntity
    {
        private readonly ISleekFlowDbContext _context;
        public BaseRepository(ISleekFlowDbContext context)
        {
            _context = context;
        }

        public async virtual Task<T> GetById(string id)
        {
            return null;
        }

        public async virtual Task<(IEnumerable<T> Results, long Count)> GetPageList(BasePageFilter filter)
        {
            var x = new List<T>();
            return (x, 1000);
        }

        public async virtual Task<int> Insert(T entity)
        {
            return 0;
        }

        public async virtual Task<bool> UpdateById(string id, T entity)
        {
            return false;
        }

        public async virtual Task<int> Delete(string id)
        {
            return 0;
        }
    }
}
