using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class NotAuthorized : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
