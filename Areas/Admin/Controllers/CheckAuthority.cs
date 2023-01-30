using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using System.Security.Claims;

namespace Shop.Areas.Admin.Controllers
{
/*    public class CheckAuthority : Controller
    {
    
    }*/
    public class ControllerBase : Controller
    {
        public ControllerBase()
        {
        }
        public bool CheckFunctionAuthority(int idFunction, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            var user = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            var id = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            int count = context.StaffFunctionAuthority.Count(m => m.IdStaff.ToString() == id && m.IdFunction == idFunction);
            if (count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}
