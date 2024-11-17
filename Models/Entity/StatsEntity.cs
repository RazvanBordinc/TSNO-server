using System.ComponentModel.DataAnnotations;

namespace TSNO.Models
{
    public class StatsEntity
    {
        [Key]
        public int Id { get; set; }
        public int TotalNotes { get; set; }
        public int ActiveNotes { get; set; }
    }
}
