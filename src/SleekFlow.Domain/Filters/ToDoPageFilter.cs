using SleekFlow.Domain.Attributes;
using SleekFlow.Domain.Enums;

namespace SleekFlow.Domain.Filters
{
    public class ToDoPageFilter : BasePageFilter
    {
        public Status? Status { get; set; }

        [FieldAttribute(exactName: "DueAt")]
        [Filter(FilterType.RangeStart)]
        public DateTime? DueAtStart { get; set; }

        [FieldAttribute(exactName: "DueAt")]
        [Filter(FilterType.RangeEnd)]
        public DateTime? DueAtEnd { get; set; }
    }
}
