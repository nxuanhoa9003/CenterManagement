using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Order
{
    public class OrderRequest
    {
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
    }
}
