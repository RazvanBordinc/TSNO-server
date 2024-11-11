using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TSNO.Models.ResponseDTO
{
    public class ViewDTO
    {
        [MaxLength(4)]
        public string? Code { get; set; }
 
    }
 
}
 