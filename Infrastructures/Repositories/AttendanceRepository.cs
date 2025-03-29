using Domain.Entities;
using Domain.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
    public class AttendanceRepository : GenericRepository<Attendance>, IAttendanceRepository
    {
        public AttendanceRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task AddAttendanceAsync(Attendance attendance) => await InsertAsync(attendance);

        public async Task UpdateAttendanceAsync(Attendance attendance) => await UpdateAsync(attendance);

        public async Task DeleteAttendanceAsync(Guid attendanceId) => await DeleteAsync(x => x.Id == attendanceId);

        // Lấy danh sách điểm danh theo lớp học
        public async Task<IEnumerable<Attendance>> GetAttendancesByClassId(Guid classId)
        {
            return await _dbSet
            .Where(a => a.Lesson != null && a.Lesson.ClassId == classId)
            .Include(a => a.Student)
            .Include(a => a.Lesson)
            .ThenInclude(l => l.Class)
            .ToListAsync();
        }

        // Lấy danh sách điểm danh theo buổi học
        public async Task<IEnumerable<Attendance>> GetAttendancesByLessonId(Guid lessonId)
        {
            return await _dbSet
            .Where(a => a.LessonId == lessonId)
            .Include(a => a.Student) // Nếu cần thông tin học viên
            .ToListAsync();
        }

        // Lấy danh sách điểm danh theo học viên
        public async Task<IEnumerable<Attendance>> GetAttendancesByStudentId(Guid studentId)
        {
            return await _dbSet
             .Where(a => a.StudentId == studentId)
             .Include(a => a.Lesson) // Nếu cần thông tin buổi học
             .ToListAsync();
        }

        public async Task<IEnumerable<Attendance>> GetAttendancesByClassLessonId(Guid classId, Guid lessonId)
        {
            var attendances = await _context.Attendances
                 .Where(a => a.Lesson.ClassId == classId)
                 .Include(a => a.Lesson)
                 .Include(a => a.Student)
                 .ToListAsync();
            return attendances;
        }

        public async Task<Attendance?> GetAttendancesById(Guid Id)
        {
            return await GetByIdAsync(Id);
        }
    }
}
