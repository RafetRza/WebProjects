using Academy.BLL.DTOs;
using Academy.MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Academy.MVC.Areas.TeacherPanel.Controllers
{
    [Area("TeacherPanel")]
    [Authorize(Roles = "Teacher")]
    public class AttendanceController : Controller
    {
        private readonly ApiClient _apiClient;

        public AttendanceController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? groupId, string? date)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    ViewBag.Error = "Could not identify the current teacher.";
                    return View();
                }

                // Load teacher's groups for the dropdown
                var groups = await _apiClient.GetAsync<List<GroupDto>>($"api/groups/byteacher/{userId}");
                ViewBag.Groups = groups ?? new List<GroupDto>();

                var selectedDate = string.IsNullOrEmpty(date) ? DateTime.Today : DateTime.Parse(date);
                ViewBag.SelectedDate = selectedDate.ToString("yyyy-MM-dd");
                ViewBag.SelectedGroupId = groupId;

                if (groupId.HasValue)
                {
                    // Load students of the selected group
                    var selectedGroup = groups?.FirstOrDefault(g => g.Id == groupId.Value);
                    ViewBag.Students = selectedGroup?.Students?.ToList() ?? new List<GroupStudentDto>();

                    // Load existing attendance for this group and date
                    var attendances = await _apiClient.GetAsync<List<AttendanceDto>>(
                        $"api/attendances/bygroup/{groupId.Value}?date={selectedDate:yyyy-MM-dd}");
                    ViewBag.Attendances = attendances ?? new List<AttendanceDto>();
                }

                return View();
            }
            catch
            {
                ViewBag.Error = "Academy API is not reachable.";
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAttendance(int groupId, string date, int[] studentIds, int[] statuses)
        {
            try
            {
                var selectedDate = DateTime.Parse(date);

                var attendances = new List<CreateAttendanceDto>();

                for (int i = 0; i < studentIds.Length; i++)
                {
                    attendances.Add(new CreateAttendanceDto
                    {
                        StudentId = studentIds[i],
                        Date = selectedDate,
                        AttendanceStatus = (AttendanceStatusDto)statuses[i]
                    });
                }

                var success = await _apiClient.PostAsync("api/attendances/bulk", attendances);

                if (!success)
                {
                    TempData["Error"] = "Failed to save attendance. API returned an error.";
                    return RedirectToAction("Index", new { groupId, date = selectedDate.ToString("yyyy-MM-dd") });
                }

                TempData["Success"] = "Attendance saved successfully!";
                return RedirectToAction("Index", new { groupId, date = selectedDate.ToString("yyyy-MM-dd") });
            }
            catch
            {
                TempData["Error"] = "Failed to save attendance. Please try again.";
                return RedirectToAction("Index", new { groupId, date });
            }
        }
    }
}
