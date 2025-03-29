using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IAttendanceRepository : IGenericRepository<Attendance>
    {
        Task<Attendance> GetAttendancesById(Guid Id);
        Task<IEnumerable<Attendance>> GetAttendancesByLessonId(Guid lessonId);
        Task<IEnumerable<Attendance>> GetAttendancesByStudentId(Guid studentId);
        Task<IEnumerable<Attendance>> GetAttendancesByClassId(Guid classId);

        Task<IEnumerable<Attendance>> GetAttendancesByClassLessonId(Guid classId, Guid lessonId);

        Task AddAttendanceAsync(Attendance attendance);
        Task UpdateAttendanceAsync(Attendance attendance);
        Task DeleteAttendanceAsync(Guid attendanceId);
    }
}
