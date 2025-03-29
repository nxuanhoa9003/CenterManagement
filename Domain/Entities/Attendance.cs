using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Attendance
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid LessonId { get; set; }
        public Guid StudentId { get; set; }
        public DateTime Date { get; set; }
        public string? Comment { get; set; }
        public int? Evaluation { get; set; } // đánh giá hoặc chấm điểm
        public AttendanceStatus AttendanceStatus { get; set; }// enum // 0: Vắng, 1: Có mặt

        public Student? Student { get; set; }
        public Lesson? Lesson { get; set; }
    }
}
