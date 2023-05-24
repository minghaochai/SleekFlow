using SleekFlow.Application.Common.Dtos;
using SleekFlow.Domain.Entities;
using SleekFlow.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SleekFlow.Application.Features.ToDos
{
    public class ToDoRequest : BaseRequest<ToDo>
    {
        [Required(ErrorMessage = nameof(Name) + " is a required field.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = nameof(Description) + " is a required field.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = nameof(DueAt) + " is a required field.")]
        public DateTime? DueAt { get; set; }

        [Required(ErrorMessage = nameof(Status) + " is a required field.")]
        public Status? Status { get; set; }
    }
}
