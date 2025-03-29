using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Auth
{
    public class ConfirmEmailRequest
    {
        public string Email {  get; set; }   
        public string Otp { get; set; }
    }

    public class VerifyOtpDto
    {
        public string Email { get; set; }
        public string Otp { get; set; }
    }

    public class SendOtpRequest
    {
        public string Email { get; set; }
    }


}
