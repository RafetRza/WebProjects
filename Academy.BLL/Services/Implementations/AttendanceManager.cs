using Academy.BLL.DTOs;
using Academy.BLL.Services.Interfaces;
using Academy.DAL.DataContext;
using Academy.DAL.DataContext.Entities;
using Academy.DAL.Repositories.Interfaces;
using AutoMapper;
using Core.Persistence.Repositories;

namespace Academy.BLL.Services.Implementations
{
    public class AttendanceManager : CrudManager<Attendance, AttendanceDto, CreateAttendanceDto, UpdateAttendanceDto>, IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IMapper _mapper;

        public AttendanceManager(IAttendanceRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _attendanceRepository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AttendanceDto>> GetByGroupAndDateAsync(int groupId, DateTime date)
        {
            var attendances = await _attendanceRepository.GetByGroupAndDateAsync(groupId, date);
            return _mapper.Map<IEnumerable<AttendanceDto>>(attendances);
        }

        public async Task SaveBulkAttendanceAsync(List<CreateAttendanceDto> attendances)
        {
            if (attendances == null || attendances.Count == 0) return;

            var studentIds = attendances.Select(a => a.StudentId).ToList();
            var date = attendances.First().Date;

            // Get existing attendance records for these students on this date
            var existing = (await _attendanceRepository.GetByStudentIdsAndDateAsync(studentIds, date)).ToList();

            foreach (var dto in attendances)
            {
                var existingRecord = existing.FirstOrDefault(e => e.StudentId == dto.StudentId);

                if (existingRecord != null)
                {
                    // Update existing record
                    existingRecord.AttendanceStatus = (AttendanceStatus)(int)dto.AttendanceStatus;
                    await _attendanceRepository.UpdateAsync(existingRecord);
                }
                else
                {
                    // Create new record
                    var entity = _mapper.Map<Attendance>(dto);
                    await _attendanceRepository.AddAsync(entity);
                }
            }
        }
    }
}
