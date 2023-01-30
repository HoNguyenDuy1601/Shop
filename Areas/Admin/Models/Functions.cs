using System.ComponentModel.DataAnnotations;

namespace Shop.Areas.Admin.Models
{
    public class Functions
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public ICollection<StaffFunctionAuthority>? Staff { get; set; }
    }
}
