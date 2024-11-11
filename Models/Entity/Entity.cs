using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSNO.Models
{
    public class Entity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [MaxLength(10000)]
        public string Notes { get; set; } = string.Empty;

        [Range(0, 9999)]
        public int Code { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public static int TotalNotes { get; set; }

        [NotMapped]
        public static int ActiveNotes { get; set; }
    }
}
