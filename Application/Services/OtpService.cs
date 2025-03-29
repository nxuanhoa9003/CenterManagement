using Domain.Entities;
using Application.InterfacesServices;
using Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class OtpService : IOtpService
    {
        public OtpGenerator CreateOtp(int expiryMinutes = 5)
        {
            var OtpCode = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
            var ExpiryTime = DateTime.Now.AddMinutes(expiryMinutes);
            return new OtpGenerator
            {
                OtpCode = OtpCode,
                ExpiryTime = ExpiryTime
            };
        }

        public OtpGenerator HashOtpModel(OtpGenerator model)
        {
            return new OtpGenerator
            {
                OtpCode = HashOtp(model.OtpCode), // Băm OTP
                ExpiryTime = model.ExpiryTime
            };
        }

        public string HashOtp(string otp)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(otp);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }



        public (bool status, string message) ValidateOtpAsync(ApplicationUser user, string inputOtp)
        {
            if (user == null)
            {
                return (false, "Email không hợp lệ"); // OTP không hợp lệ

            }

            if (user.OtpExpiryDate < DateTime.UtcNow)
            {
                return (false, "Otp hết hạn"); // OTP hết hạn
            }

            // So sánh OTP đã hash
            if (user.Otp != HashOtp(inputOtp))
            {
                return (false, "Otp không hợp lệ"); // OTP không hợp lệ
            }
            return (true, "Otp hợp lệ"); // OTP hợp lệ
        }
    }
}
