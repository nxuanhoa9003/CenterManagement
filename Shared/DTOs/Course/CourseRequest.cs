using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Course
{
    public class CourseRequest
    {
        public Guid Id { get; set; }
        public string? CourseName { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CategoryId { get; set; }
    }
}
