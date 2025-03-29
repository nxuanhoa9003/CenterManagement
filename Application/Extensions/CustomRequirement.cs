using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public class CustomRequirement : IAuthorizationRequirement
    {
        public string RequiredClaimType { get; }
        public string RequiredClaimValue { get; }

        public CustomRequirement(string requiredClaimType, string requiredClaimValue)
        {
            RequiredClaimType = requiredClaimType;
            RequiredClaimValue = requiredClaimValue;
        }
    }
}
