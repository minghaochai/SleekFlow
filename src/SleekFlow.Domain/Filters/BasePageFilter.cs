namespace SleekFlow.Domain.Filters
{
    public class BasePageFilter : BaseFilter
    {
        public int PageNumber { get; set; }

        public int ItemsPerPage { get; set; }
    }
}
