using Academy.BLL.DTOs;
using Academy.MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Academy.MVC.Areas.TeacherPanel.Controllers
{
    [Area("TeacherPanel")]
    [Authorize(Roles ="Teacher")]
    public class GroupController : Controller
    {
        private readonly ApiClient _apiClient;

        public GroupController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    ViewBag.Error = "Could not identify the current teacher.";
                    return View(Enumerable.Empty<GroupDto>());
                }

                var groups = await _apiClient.GetAsync<List<GroupDto>>($"api/groups/byteacher/{userId}");

                if (groups is null)
                {
                    ViewBag.Error = "Could not load groups from API.";
                    return View(Enumerable.Empty<GroupDto>());
                }

                return View(groups);
            }
            catch
            {
                ViewBag.Error = "Academy API is not reachable.";
                return View(Enumerable.Empty<GroupDto>());
            }
        }
    }
}
