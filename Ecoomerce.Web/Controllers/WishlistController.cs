using Microsoft.AspNetCore.Mvc;

namespace Ecoomerce.Web.Controllers
{
    public class WishlistController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
