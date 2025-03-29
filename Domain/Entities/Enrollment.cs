using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Enrollment
    {
        public Guid StudentId { get; set; }
        public Guid ClassId { get; set; }
        public DateTime EnrolledAt { get; set; }
        public EnrollmentStatus Status { get; set; }
        public Student? Student { get; set; }
        public Class? Class { get; set; } 
    }

    public enum EnrollmentStatus { 
        Active,     // hoạt động 0
        Completed,  // hoàn thành 1
        Suspended   // tạm dừng 2
    } 

}
