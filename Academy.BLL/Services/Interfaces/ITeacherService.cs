using Academy.BLL.DTOs;
using Academy.DAL.DataContext.Entities;

namespace Academy.BLL.Services.Interfaces
{
    public interface ITeacherService : ICrudServiceAsync<Teacher, TeacherDto, CreateTeacherDto, UpdateTeacherDto>
    {
    }
}
