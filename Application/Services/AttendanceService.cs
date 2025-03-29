using Application.InterfacesServices;
using Domain.Entities;
using Domain.Interface;
using Shared.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;

        public AttendanceService(IAttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        public async Task AddAttendanceAsync(Attendance attendance)
        {
            await _attendanceRepository.AddAttendanceAsync(attendance);
        }

        public async Task DeleteAttendanceAsync(Guid attendanceId)
        {
            await _attendanceRepository.DeleteAttendanceAsync(attendanceId);
        }

        public async Task UpdateAttendanceAsync(Attendance attendance)
        {
            await _attendanceRepository.UpdateAttendanceAsync(attendance);
        }

        public async Task<IEnumerable<Attendance>> GetAttendancesByClassId(Guid classId)
        {
            return await _attendanceRepository.GetAttendancesByClassId(classId);
        }

        public async Task<IEnumerable<Attendance>> GetAttendancesByLessonId(Guid lessonId)
        {
            return await _attendanceRepository.GetAttendancesByLessonId(lessonId);
        }

        public async Task<IEnumerable<Attendance>> GetAttendancesByStudentId(Guid studentId)
        {
            return await _attendanceRepository.GetAttendancesByStudentId(studentId);
        }

        public async Task<IEnumerable<AttendanceDto>> GetAttendancesByClassLessonId(Guid classId, Guid lessonId)
        {
            var attendances = await _attendanceRepository.GetAttendancesByClassLessonId(classId, lessonId);
            var attendanceDtos = attendances.Select(a => new AttendanceDto
            {
                AttendanceId = a.Id,
                StudentId = a.StudentId,
                StudentName = a.Student.User.FullName,
                LessonId = a.LessonId,
                LessonDate = a.Lesson.Date,
                AttendanceStatus = (AttendanceStatusDTO)a.AttendanceStatus, // Enum (0: Vắng, 1: Có mặt)
                Comment = a.Comment,
                Evaluation = a.Evaluation
            }).ToList();
            return attendanceDtos;
        }

        public async Task<Attendance?> GetAttendancesById(Guid Id)
        {
            return await _attendanceRepository.GetAttendancesById(Id);
        }
    }
}
