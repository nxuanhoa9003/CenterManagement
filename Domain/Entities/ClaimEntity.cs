using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ClaimEntity
    {
        //public Guid Id { get; set; } = Guid.NewGuid();
        public int Id { get; set; } // ID tự động tăng
        public string Type { get; set; }  // Loại claim, ví dụ: "Permission"
        public string Value { get; set; } // Giá trị claim, ví dụ: "CanEditUsers"
        public string Name { get; set; } // Giá trị claim, ví dụ: "CanEditUsers"
        public string Category { get; set; } 
    }
}
