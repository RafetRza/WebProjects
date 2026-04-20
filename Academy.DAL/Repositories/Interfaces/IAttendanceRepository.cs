using Academy.DAL.DataContext;
using Academy.DAL.DataContext.Entities;
using Core.Persistence.Repositories;

namespace Academy.DAL.Repositories.Interfaces
{
    public interface IAttendanceRepository : IRepositoryAsync<Attendance, AcademyDbContext>
    {
        Task<IEnumerable<Attendance>> GetByGroupAndDateAsync(int groupId, DateTime date);
        Task<IEnumerable<Attendance>> GetByStudentIdsAndDateAsync(IEnumerable<int> studentIds, DateTime date);
    }
}
