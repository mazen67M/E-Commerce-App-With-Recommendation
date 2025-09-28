using Microsoft.AspNetCore.Mvc;

namespace Ecoomerce.Web.Areas.Reporting.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
