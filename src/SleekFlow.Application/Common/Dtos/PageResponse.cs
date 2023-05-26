namespace SleekFlow.Application.Common.Dtos
{
    public class PageResponse<T>
    {
        public List<T> Data { get; set; } = new List<T>();

        public PageInfo PageInfo { get; set; } = new PageInfo();
    }
}
