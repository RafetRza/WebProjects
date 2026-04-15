using Academy.BLL.DTOs;
using Academy.BLL.Services.Interfaces;
using Academy.DAL.DataContext;
using Academy.DAL.DataContext.Entities;
using AutoMapper;
using Core.Persistence.Repositories;

namespace Academy.BLL.Services.Implementations
{
    public class AttendanceManager : CrudManager<Attendance, AttendanceDto, CreateAttendanceDto, UpdateAttendanceDto>, IAttendanceService
    {
        public AttendanceManager(IRepositoryAsync<Attendance, AcademyDbContext> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
