using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid StudentId { get; set; }
        public Student? Student { get; set; }
        public Guid CourseId { get; set; }
        
        public Course? Course { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending; // Pending, Paid, Canceled
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        // Một Order có một Payment
        public Payment? Payment { get; set; }
    }
}
