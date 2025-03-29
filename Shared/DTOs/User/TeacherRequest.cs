using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.User
{
    public class TeacherRequest
    {
        public Guid? TeacherId { get; set; }
        public string? UserId { get; set; }
        public string? Specialty { get; set; } // Chuyên môn
        //public DateTime CreatedAt { get; set; }
    }
}
