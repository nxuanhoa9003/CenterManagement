using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Autho
{
    public class RoleRequest
    {
        public string rolename { get; set; }
    }

    public class RoleUpdateRequest
    {
        public string oldRolename { get; set; }
        public string newRolename { get; set; }
    }

    public class RoleToUserRequest
    {
        public string UserId { get; set; }
        public List<string> RoleNames { get; set; } = new();
    }

    public class RemoveRolesRequest
    {
        public string UserId { get; set; }
        public List<string> RoleNames { get; set; } = new();
    }

}
