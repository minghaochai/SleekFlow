using SleekFlow.Domain.Entities;

namespace SleekFlow.Application.Features.Base
{
    public abstract class BaseService<T> : IBaseService<T>
       where T : BaseEntity
    {
    }
}
