using SleekFlow.Domain.Attributes;
using SleekFlow.Domain.Enums;

namespace SleekFlow.Domain.Filters
{
    public class ToDoPageFilter : BaseFilter
    {
        public Status? Status { get; set; }

        [Filter(FilterType.RangeStart)]
        public DateTime? DueAtStart { get; set; }

        [Filter(FilterType.RangeEnd)]
        public DateTime? DueAtEnd { get; set; }
    }
}
