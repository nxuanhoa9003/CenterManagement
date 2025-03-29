using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string? Address { get; set; }
        public string? AvatarUrl { get; set; }

        public string ? Otp { get; set; }
        public DateTime? OtpExpiryDate { get; set; }
        public DateTime? LastOtpSentTime { get; set; } // lần gửi otp cuối
        public Student? Student { get; set; }
        public Teacher? Teacher { get; set; }
    }
}
