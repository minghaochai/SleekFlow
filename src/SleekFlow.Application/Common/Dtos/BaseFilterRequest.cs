using SleekFlow.Application.Mappings;

namespace SleekFlow.Application.Common.Dtos
{
    public abstract class BaseFilterRequest<T> : IMapFrom<T>
    {
        public string? SortColumn { get; set; }

        public string? SortDirection { get; set; }
    }
}
