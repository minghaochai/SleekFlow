using SleekFlow.Domain.Filters;

namespace SleekFlow.Domain.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task<T> GetById(int id);

        Task<(IEnumerable<T> Results, long Count)> GetPageList(BasePageFilter filter);

        Task<int> Insert(T entity);

        Task<int> Update(T entity);

        Task<int> Delete(T entity);
    }
}
