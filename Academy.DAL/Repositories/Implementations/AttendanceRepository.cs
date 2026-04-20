using Academy.DAL.DataContext;
using Academy.DAL.DataContext.Entities;
using Academy.DAL.Repositories.Interfaces;
using Core.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Academy.DAL.Repositories.Implementations
{
    public class AttendanceRepository : EfCoreRepositoryAsync<Attendance, AcademyDbContext>, IAttendanceRepository
    {
        private readonly AcademyDbContext _context;

        public AttendanceRepository(AcademyDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Attendance>> GetByGroupAndDateAsync(int groupId, DateTime date)
        {
            return await _context.Attendances
                .AsNoTracking()
                .Include(a => a.Student)
                .Where(a => a.Student != null && a.Student.GroupId == groupId && a.Date.Date == date.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Attendance>> GetByStudentIdsAndDateAsync(IEnumerable<int> studentIds, DateTime date)
        {
            var ids = studentIds.ToList();
            return await _context.Attendances
                .Where(a => ids.Contains(a.StudentId) && a.Date.Date == date.Date)
                .ToListAsync();
        }
    }
}
