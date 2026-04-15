using Academy.DAL.DataContext;
using Academy.DAL.DataContext.Entities;
using Academy.DAL.Repositories.Interfaces;
using Core.Persistence.Repositories;

namespace Academy.DAL.Repositories.Implementations
{
    public class AttendanceRepository : EfCoreRepositoryAsync<Attendance, AcademyDbContext>, IAttendanceRepository
    {
        public AttendanceRepository(AcademyDbContext context) : base(context)
        {
        }
    }
}
