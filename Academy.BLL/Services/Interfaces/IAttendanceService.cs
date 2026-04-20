using Academy.BLL.DTOs;
using Academy.DAL.DataContext.Entities;

namespace Academy.BLL.Services.Interfaces
{
    public interface IAttendanceService : ICrudServiceAsync<Attendance, AttendanceDto, CreateAttendanceDto, UpdateAttendanceDto>
    {
        Task<IEnumerable<AttendanceDto>> GetByGroupAndDateAsync(int groupId, DateTime date);
        Task SaveBulkAttendanceAsync(List<CreateAttendanceDto> attendances);
    }
}
