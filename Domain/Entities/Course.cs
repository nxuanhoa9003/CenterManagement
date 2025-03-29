using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Course
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? CourseName { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? CategoryId { get; set; }
        public Category? Category { get; set; } = new();
       
        public ICollection<Class>? Classes { get; set; } = new List<Class>();
        // Một Course có nhiều Orders
        [JsonIgnore]
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
