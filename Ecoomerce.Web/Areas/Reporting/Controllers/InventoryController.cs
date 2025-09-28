using Microsoft.AspNetCore.Mvc;

namespace Ecoomerce.Web.Areas.Reporting.Controllers
{
    public class InventoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
