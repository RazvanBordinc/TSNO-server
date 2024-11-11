using System.ComponentModel.DataAnnotations;

namespace TSNO.Models.ResponseDTO
{
    public class AddDTO
    {
        [MaxLength(10000)]
        public string Notes { get; set; } = string.Empty;

    }
}
