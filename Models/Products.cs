using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class Products
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        [DisplayName("Url Image")]
        public string? UrlHinhAnh { get; set; }
        public int? idProduct { get; set; }
    }
}
