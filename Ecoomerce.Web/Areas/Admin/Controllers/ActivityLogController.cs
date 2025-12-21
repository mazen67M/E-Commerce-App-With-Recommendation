using Ecommerce.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ecoomerce.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ActivityLogController : Controller
    {
        private readonly IActivityLogService _activityLogService;

        public ActivityLogController(IActivityLogService activityLogService)
        {
            _activityLogService = activityLogService;
        }

        public async Task<IActionResult> Index(int count = 100)
        {
            var activities = await _activityLogService.GetRecentActivitiesAsync(count);
            return View(activities);
        }

        public async Task<IActionResult> UserActivities(string userId, int count = 50)
        {
            var activities = await _activityLogService.GetUserActivitiesAsync(userId, count);
            return View("Index", activities);
        }

        public async Task<IActionResult> ByAction(string action, int count = 50)
        {
            var activities = await _activityLogService.GetActivitiesByActionAsync(action, count);
            return View("Index", activities);
        }
    }
}
