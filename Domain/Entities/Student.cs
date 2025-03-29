using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Student
    {
        public Guid? StudentId { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Attendance>? Attendances { get; set; }
        public ICollection<Enrollment>? Enrollments { get; set; }
        public ICollection<Payment>? Payments { get; set; } = new List<Payment>();
        // Một Student có nhiều Orders
        [JsonIgnore]
        public ICollection<Order> Orders { get; set; } = new List<Order>();

    }
}
