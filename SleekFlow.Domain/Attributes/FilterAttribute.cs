using SleekFlow.Domain.Enums;

namespace SleekFlow.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FilterAttribute : Attribute
    {
        public FilterAttribute(FilterType filterType)
        {
            FilterType = filterType;
        }

        public FilterType FilterType { get; set; }
    }
}
