using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; } // Khóa chính

        public string Token { get; set; } = string.Empty; // Refresh Token lưu dưới dạng chuỗi

        public string UserId { get; set; } = string.Empty; // Liên kết với bảng User (AspNetUsers)

        public string AccessTokenId { get; set; } = string.Empty; // Liên kết với accesstoken
        public DateTime ExpiryDate { get; set; } // Ngày hết hạn của token

        public bool IsRevoked { get; set; } = false; // Nếu true, token này không thể sử dụng

        public bool IsUsed { get; set; } = false; // Đánh dấu nếu token đã được sử dụng

        public DateTime CreatedAt { get; set; } // Ngày tạo token
        public ApplicationUser User { get; set; }
    }
}
