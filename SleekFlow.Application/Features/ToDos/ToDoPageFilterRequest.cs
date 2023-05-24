using SleekFlow.Application.Common.Dtos;
using SleekFlow.Domain.Enums;
using SleekFlow.Domain.Filters;

namespace SleekFlow.Application.Features.ToDos
{
    public class ToDoPageFilterRequest : BasePageFilterRequest<ToDoPageFilter>
    {
        public Status? Status { get; set; }

        public DateTime? DueAtStart { get; set; }

        public DateTime? DueAtEnd { get; set; }
    }
}
