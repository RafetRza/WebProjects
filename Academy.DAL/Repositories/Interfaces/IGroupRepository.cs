using Academy.DAL.DataContext;
using Academy.DAL.DataContext.Entities;
using Core.Persistence.Repositories;

namespace Academy.DAL.Repositories.Interfaces
{
    public interface IGroupRepository : IRepositoryAsync<Group, AcademyDbContext>
    {
        Task<IEnumerable<Group>> GetByTeacherUserIdAsync(string userId);
    }
}
