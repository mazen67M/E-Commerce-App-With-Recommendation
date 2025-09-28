using Microsoft.AspNetCore.Mvc;

namespace Ecoomerce.Web.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
