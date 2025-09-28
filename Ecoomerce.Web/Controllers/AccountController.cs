using AutoMapper;
using Ecommerce.Application.DTOs.Auth;
using Ecommerce.Application.Services.Interfaces;
using Ecommerce.Application.ViewModels.Forms___Input_Models;
using Microsoft.AspNetCore.Mvc;

namespace Ecoomerce.Web.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
    }
}
