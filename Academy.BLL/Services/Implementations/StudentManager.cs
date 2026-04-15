using Academy.BLL.DTOs;
using Academy.BLL.Services.Interfaces;
using Academy.DAL.DataContext;
using Academy.DAL.DataContext.Entities;
using AutoMapper;
using Core.Persistence.Repositories;

namespace Academy.BLL.Services.Implementations
{
    public class StudentManager : CrudManager<Student, StudentDto, CreateStudentDto, UpdateStudentDto>, IStudentService
    {
        public StudentManager(IRepositoryAsync<Student, AcademyDbContext> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
