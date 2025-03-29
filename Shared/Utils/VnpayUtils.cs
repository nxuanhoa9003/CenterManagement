using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Utils
{
    public static class VnpayUtils
    {
        public static string GetIpAddress(HttpContext httpContext)
        {
            if (httpContext == null) return "Unknown";

            string ip = httpContext.Connection.RemoteIpAddress?.ToString();

            if (httpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                ip = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            }

            if (string.IsNullOrEmpty(ip) || ip == "::1") // Localhost IPv6
            {
                ip = "127.0.0.1"; // IPv4 cho localhost
            }

            return ip;
        }


        public static string HmacSHA512(string key, string inputData)
        {
            var hash = new StringBuilder();
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                var hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }

            return hash.ToString();
        }

    }

}
