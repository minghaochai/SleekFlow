using SleekFlow.Application.Common.Dtos;
using SleekFlow.Domain.Entities;
using SleekFlow.Domain.Enums;

namespace SleekFlow.Application.Features.ToDos
{
    public class ToDoResponse : BaseResponse<ToDo>
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public DateTime? DueAt { get; set; }

        public Status? Status { get; set; }
    }
}
