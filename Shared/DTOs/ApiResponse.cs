using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class ApiResponse
    {
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }
        public ApiResponse()
        {
            
        }
        public ApiResponse(string message, object? data = null)
        {
            Message = message;
            Data = data;
        }
    }
}
