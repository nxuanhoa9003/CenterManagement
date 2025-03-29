using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Teacher
    {
        public Guid? TeacherId { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public string? Specialty { get; set; } // Chuyên môn
        public DateTime CreatedAt { get; set; }
        public ICollection<Class>? Classes { get; set; }

    }
}
