using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class ProductTypes
    {
        [Key]
        public int Id { get; set; }
        public string Type   { get; set; }

    }
}

