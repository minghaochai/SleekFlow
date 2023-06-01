using SleekFlow.Application.Mappings;

namespace SleekFlow.Application.Common.Dtos
{
    public abstract class BaseRequest<T> : IMapFrom<T>
    {
    }
}
