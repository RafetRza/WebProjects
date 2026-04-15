using Academy.BLL.DTOs;
using Academy.DAL.DataContext.Entities;

namespace Academy.BLL.Services.Interfaces
{
    public interface IStudentService : ICrudServiceAsync<Student, StudentDto, CreateStudentDto, UpdateStudentDto>
    {
    }
}
