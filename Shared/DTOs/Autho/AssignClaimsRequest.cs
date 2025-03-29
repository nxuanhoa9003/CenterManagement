using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Autho
{
    public class AssignClaimsRequest
    {
        public string UserId { get; set; }
        public List<ClaimDto> Claims { get; set; } = new();
    }
    public class RemoveClaimsRequest
    {
        public string UserId { get; set; }
        public List<ClaimDto> Claims { get; set; } = new();
    }


    public class ClaimDto
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
