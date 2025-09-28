using Microsoft.AspNetCore.Mvc;

namespace Ecoomerce.Web.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
