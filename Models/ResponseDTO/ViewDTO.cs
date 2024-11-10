using System.ComponentModel.DataAnnotations;

namespace TSNO.Models.ResponseDTO
{
    public class GetDTO
    {
        [MaxLength(4)]
        public string? Code {  get; set; }
    }
}
