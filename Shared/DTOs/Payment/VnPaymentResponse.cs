using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Payment
{
    public class VnPaymentResponse
    {
        public string TransactionId { get; set; } // Mã giao dịch từ VNPay
        public string OrderId { get; set; } // Mã đơn hàng của hệ thống
        public decimal Amount { get; set; } // Số tiền thanh toán
        public string BankCode { get; set; } // Mã ngân hàng
        public string ResponseCode { get; set; } // Mã phản hồi từ VNPay
        public string Message { get; set; } // Nội dung phản hồi
        public bool IsSuccess => ResponseCode == "00"; // Kiểm tra giao dịch thành công hay không

        // Constructor khi có đầy đủ dữ liệu
        public VnPaymentResponse(string transactionId, string orderId, decimal amount, string bankCode, string responseCode, string message)
        {
            TransactionId = transactionId;
            OrderId = orderId;
            Amount = amount;
            BankCode = bankCode;
            ResponseCode = responseCode;
            Message = message;
        }
        public VnPaymentResponse(IQueryCollection query)
        {
            TransactionId = query["vnp_TransactionNo"];
            OrderId = query["vnp_TxnRef"];
            Amount = decimal.Parse(query["vnp_Amount"]) / 100; // VNPay trả về đơn vị VNĐ x100
            BankCode = query["vnp_BankCode"];
            ResponseCode = query["vnp_ResponseCode"];
            Message = GetResponseMessage(ResponseCode);
        }

        private string GetResponseMessage(string responseCode)
        {
            return responseCode switch
            {
                "00" => "Giao dịch thành công",
                "07" => "Trừ tiền thành công. Giao dịch bị nghi ngờ (liên hệ VNPay)",
                "09" => "Giao dịch đang chờ xử lý",
                "10" => "Giao dịch không hợp lệ",
                "24" => "Giao dịch bị hủy",
                _ => "Lỗi không xác định"
            };
        }
    }

}
