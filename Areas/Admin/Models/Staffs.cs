using System.ComponentModel.DataAnnotations;

namespace Shop.Areas.Admin.Models
{
    public class Staffs
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public int idType { get; set; }
        public string Position { get; set; }
        public ICollection<StaffFunctionAuthority>? Fuction { get; set; }
    }
}
