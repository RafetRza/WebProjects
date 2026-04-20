using Academy.BLL.DTOs;
using Academy.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Academy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendancesController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendancesController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var attendances = await _attendanceService.GetAllAsync();
            return Ok(attendances);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var attendance = await _attendanceService.GetByIdAsync(id);
            if (attendance == null)
                return NotFound();
            return Ok(attendance);
        }

        [HttpGet("bygroup/{groupId}")]
        public async Task<IActionResult> GetByGroupAndDate(int groupId, [FromQuery] DateTime date)
        {
            var attendances = await _attendanceService.GetByGroupAndDateAsync(groupId, date);
            return Ok(attendances);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAttendanceDto dto)
        {
            await _attendanceService.AddAsync(dto);
            return Ok();
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> SaveBulk([FromBody] List<CreateAttendanceDto> attendances)
        {
            await _attendanceService.SaveBulkAttendanceAsync(attendances);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAttendanceDto dto)
        {
            await _attendanceService.UpdateAsync(dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _attendanceService.DeleteAsync(id);
            return Ok();
        }
    }
}
