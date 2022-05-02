
using System.ComponentModel.DataAnnotations.Schema;

namespace Flexify.Models
{
    [Table("film")]
    public class Film
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Cast { get; set; }
        public string Country { get; set; }
        public int ReleaseYear { get; set; }
        public string Rating { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
    }
}