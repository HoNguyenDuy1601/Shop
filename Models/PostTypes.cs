using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class PostTypes
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
    }
}

