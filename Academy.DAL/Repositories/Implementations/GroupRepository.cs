using Academy.DAL.DataContext;
using Academy.DAL.DataContext.Entities;
using Academy.DAL.Repositories.Interfaces;
using Core.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Academy.DAL.Repositories.Implementations
{
    public class GroupRepository : EfCoreRepositoryAsync<Group, AcademyDbContext>, IGroupRepository
    {
        private readonly AcademyDbContext _context;

        public GroupRepository(AcademyDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Group>> GetAllAsync(params Expression<Func<Group, object>>[] includes)
        {
            return await base.GetAllAsync(g => g.Students);
        }

        public async Task<IEnumerable<Group>> GetByTeacherUserIdAsync(string userId)
        {
            return await _context.Groups
                .AsNoTracking()
                .Include(g => g.Students)
                .Include(g => g.Teacher)
                .Where(g => g.Teacher != null && g.Teacher.AppUserId == userId)
                .ToListAsync();
        }
    }
}
