using System.ComponentModel.DataAnnotations;

namespace Flexify.Models
{
    public class Image
    {
        [Required(AllowEmptyStrings = false)]
        public string FileName { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string File { get; set; }
    }
}