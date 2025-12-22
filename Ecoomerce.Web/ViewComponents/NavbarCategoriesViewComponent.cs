using Ecommerce.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecoomerce.Web.ViewComponents
{
    public class NavbarCategoriesViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public NavbarCategoriesViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _unitOfWork.Categories.GetCategoriesWithProductsAsync();
            return View(categories);
        }
    }
}
