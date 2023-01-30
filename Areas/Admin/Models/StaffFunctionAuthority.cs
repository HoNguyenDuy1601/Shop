namespace Shop.Areas.Admin.Models
{
    public class StaffFunctionAuthority
    {
        public int IdStaff { get; set; }
        public Staffs Staff { get; set; }
        public int IdFunction { get; set; }
        public Functions Function { get; set; }
    }
}
