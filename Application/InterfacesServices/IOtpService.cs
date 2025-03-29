using Domain.Entities;
using Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InterfacesServices
{
    public interface IOtpService
    {
        OtpGenerator CreateOtp(int expiryMinutes = 5);
        OtpGenerator HashOtpModel(OtpGenerator model);
        string HashOtp(string otp);
        (bool status, string message) ValidateOtpAsync(ApplicationUser user, string inputOtp);
    }
}
