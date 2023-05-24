namespace SleekFlow.Domain.Filters
{
    public class BaseFilter
    {
        public string[]? Keyword { get; set; }

        public string? SortColumn { get; set; }

        public string? SortDirection { get; set; }
    }
}
