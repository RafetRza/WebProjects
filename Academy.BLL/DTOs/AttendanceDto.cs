using System;
using System.Collections.Generic;
using System.Text;

namespace Academy.BLL.DTOs
{
    public class AttendanceDto : Dto
    {
        public int Id { get; set; }
        public string? StudentName { get; set; }
        public string? GroupName { get; set; }
        public DateTime Date { get; set; }
        public string? AttendanceStatus { get; set; }
    }

    public class CreateAttendanceDto
    {
        public int StudentId { get; set; }
        public DateTime Date { get; set; }
        public AttendanceStatusDto AttendanceStatus { get; set; }
    }

    public class UpdateAttendanceDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public DateTime Date { get; set; }
        public AttendanceStatusDto AttendanceStatus { get; set; }
    }

    public enum AttendanceStatusDto
    {
        Present,
        Absent,
        Late,
        Excused
    }
}
