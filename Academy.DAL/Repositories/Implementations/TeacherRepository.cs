using Academy.DAL.DataContext;
using Academy.DAL.DataContext.Entities;
using Academy.DAL.Repositories.Interfaces;
using Core.Persistence.Repositories;

namespace Academy.DAL.Repositories.Implementations
{
    public class TeacherRepository : EfCoreRepositoryAsync<Teacher, AcademyDbContext>, ITeacherRepository
    {
        public TeacherRepository(AcademyDbContext context) : base(context)
        {
        }
    }
}
