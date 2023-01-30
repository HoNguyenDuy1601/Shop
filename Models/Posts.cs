using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class Posts
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string CreatedBy { get; set; }
        public string PostBodyContent { get; set; }
        public string? Image { get; set; }
        public bool Show { get; set; }
        public int OrderNumber { get; set; }
        public int? idPost { get; set; }

    }
}
