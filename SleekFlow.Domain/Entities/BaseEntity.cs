﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SleekFlow.Domain.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "DATETIME")]
        public DateTime AddAt { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "NVARCHAR(100)")]
        public string AddBy { get; set; } = string.Empty;

        [Column(TypeName = "DATETIME")]
        public DateTime? EditAt { get; set; }

        [Column(TypeName = "NVARCHAR(100)")]
        public string? EditBy { get; set; }
    }
}
