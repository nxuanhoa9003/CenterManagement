using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Autho
{
    public class ClaimToRoleRequest
    {
        public string RoleName { set; get; }
        public List<ClaimDto> Claims { get; set; } = new();
    }
}
