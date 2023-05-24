using SleekFlow.Domain.Filters;

namespace SleekFlow.Domain.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task<T> GetById(string id);

        Task<(IEnumerable<T> Results, long Count)> GetPageList(BasePageFilter filter);

        Task<int> Insert(T entity);

        Task<bool> UpdateById(string id, T entity);

        Task<int> Delete(string id);
    }
}
