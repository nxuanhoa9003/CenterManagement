using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Class
{
    public class ClassRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CourseId { get; set; }
        public Guid? TeacherId { get; set; } // Giáo viên phụ trách
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class DeleteStudentClassRequest
    {
        public Guid classId { get; set; }
        public Guid studentId { get; set; }
    }

}
