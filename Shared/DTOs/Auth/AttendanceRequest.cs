using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Auth
{
    public class AttendanceRequest
    {
        public Guid Id { get; set; }
        public Guid LessonId { get; set; }
        public Guid StudentId { get; set; }
        public DateTime Date { get; set; }
        public string? Comment { get; set; }
        public int? Evaluation { get; set; }
        public AttendanceStatusDTO AttendanceStatus { get; set; }
    }

    public enum AttendanceStatusDTO
    {
        Absent = 0,  // Vắng mặt
        Present = 1  // Có mặt
    }


    public class AttendaceInfoRequet
    {
        public Guid classId { get; set; }
        public Guid lessonId { get; set; }
    }

    public class AttendanceDto
    {
        public Guid AttendanceId { get; set; }
        public Guid StudentId { get; set; }
        public string? StudentName { get; set; }
        public Guid LessonId { get; set; }
        public DateTime LessonDate { get; set; }
        public string? Comment { get; set; }
        public int? Evaluation { get; set; }
        public AttendanceStatusDTO AttendanceStatus { get; set; }// enum // 0: Vắng, 1: Có mặt

    }


}
