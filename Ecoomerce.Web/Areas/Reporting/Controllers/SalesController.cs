using Microsoft.AspNetCore.Mvc;

namespace Ecoomerce.Web.Areas.Reporting.Controllers
{
    public class SalesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
