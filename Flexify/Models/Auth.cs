using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flexify.Models
{
    [Table("auth")]
    public class Auth
    {
        [Key]
        public string ApiKey { get; set; }
        public string UserName { get; set; }
    }
}