using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.User
{
    public class StudentRequest
    {
        public Guid? StudentId { get; set; }
        public string? UserId { get; set; }
    }
}
