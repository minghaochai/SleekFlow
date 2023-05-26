using SleekFlow.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace SleekFlow.Domain.Entities
{
    public class ToDo : BaseEntity
    {
        [Column(TypeName = "NVARCHAR(100)")]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "NVARCHAR(100)")]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "DATETIME")]
        public DateTime DueAt { get; set; }

        public Status Status { get; set; }
    }
}
