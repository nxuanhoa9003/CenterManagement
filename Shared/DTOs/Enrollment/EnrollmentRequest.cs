using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Enrollment
{
    public class EnrollmentRequest
    {
        public Guid StudentId { get; set; }
        public Guid ClassId { get; set; }
        public Guid CourseId { get; set; }
        public DateTime EnrolledAt { get; set; }
        public EnrollmentStatusDTO Status { get; set; }

    }

    public enum EnrollmentStatusDTO
    {
        Active,     // hoạt động 
        Completed,  // hoàn thành
        Suspended   // tạm dừng
    }
}
