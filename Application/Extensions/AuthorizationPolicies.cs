using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public static class AuthorizationPolicies
    {
        public static void AddCustomPolicies(AuthorizationOptions options)
        {
            foreach (Permission permission in Enum.GetValues(typeof(Permission)))
            {
                options.AddPolicy(permission.ToString(), policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.Requirements.Add(new CustomRequirement("Permission", permission.ToString()));

                });
            }
        }
    }
}
