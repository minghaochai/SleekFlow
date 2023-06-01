using System.ComponentModel.DataAnnotations;

namespace SleekFlow.Application.Common.Dtos
{
    public abstract class BasePageFilterRequest<T> : BaseFilterRequest<T>
    {
        public int PageNumber { get; set; } = 1;

        [Range(1, 1000, ErrorMessage = "Valid ItemsPerPage is only 1 - 1000")]
        public int ItemsPerPage { get; set; } = 100;
    }
}
