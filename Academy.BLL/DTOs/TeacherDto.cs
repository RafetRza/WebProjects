using System;
using System.Collections.Generic;
using System.Text;

namespace Academy.BLL.DTOs
{
    public class TeacherDto : Dto
    {
        public string? Name { get; set; }
        public string? AppUserId { get; set; }
        public IEnumerable<GroupDto> Groups { get; set; } = [];
    }

    public class CreateTeacherDto
    {
        public required string Name { get; set; }
        public required string AppUserId { get; set; }
    }

    public class UpdateTeacherDto
    {
        public required string Name { get; set; }
        public required string AppUserId { get; set; }
    }
}
