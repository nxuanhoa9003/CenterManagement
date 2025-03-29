using Application.InterfacesServices;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Shared.DTOs.Payment;
using Shared.Helpers;
using Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Application.Services
{
    public class VnPayService : IVnPayService
    {
        private readonly VNPaySettings _vnPaySettings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VnPayService(VNPaySettings vnPaySettings, IHttpContextAccessor httpContextAccessor)
        {
            _vnPaySettings = vnPaySettings;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> CreatePaymentUrl(Order order)
        {
            var context = _httpContextAccessor.HttpContext;
            var timeNow = DateTime.UtcNow;

            var BaseUrl = _vnPaySettings.BaseUrl;
            var HashSecret = _vnPaySettings.HashSecret;



            var returnUrl = _vnPaySettings.ReturnUrl;
            var tmnCode = _vnPaySettings.TmnCode;

            var version = _vnPaySettings.Version;
            var command = _vnPaySettings.Command;
            var locale = _vnPaySettings.Locale;
            var currCode = _vnPaySettings.CurrCode;

            var vpay = new VnPayLibrary();
            vpay.AddRequestData("vnp_Version", version);
            vpay.AddRequestData("vnp_Command", command);
            vpay.AddRequestData("vnp_TmnCode", tmnCode);
            vpay.AddRequestData("vnp_Amount", ((int)order.TotalAmount * 100).ToString());
            vpay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            vpay.AddRequestData("vnp_CurrCode", currCode);
            vpay.AddRequestData("vnp_IpAddr", VnpayUtils.GetIpAddress(context));
            vpay.AddRequestData("vnp_Locale", locale);
            vpay.AddRequestData("vnp_OrderInfo", $"Thanh toán đơn hàng {order.Id}");
            vpay.AddRequestData("vnp_OrderType", "billpayment");
            vpay.AddRequestData("vnp_ReturnUrl", returnUrl);
            vpay.AddRequestData("vnp_TxnRef", order.Id.ToString());

            string paymentUrl = vpay.CreateRequestUrl(BaseUrl, HashSecret);
            return await Task.FromResult(paymentUrl);
        }


        public VnPaymentResponse PaymentExecute(IQueryCollection collections)
        {
            if (collections.Count == 0)
            {
                return new VnPaymentResponse("", "", 0, "", "NO_DATA", "Không có dữ liệu phản hồi từ VNPay");
            }

            VnPayLibrary vnpay = new VnPayLibrary();

            // Lấy toàn bộ dữ liệu từ query string (chỉ lấy các key bắt đầu bằng "vnp_")
            foreach (var key in collections.Keys)
            {
                if (!string.IsNullOrEmpty(collections[key]) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, collections[key]);
                }
            }

            // Lấy thông tin phản hồi từ VNPay
            var transactionId = vnpay.GetResponseData("vnp_TransactionNo");
            var orderId = vnpay.GetResponseData("vnp_TxnRef");
            var amount = decimal.Parse(vnpay.GetResponseData("vnp_Amount")) / 100; // VNPay trả về đơn vị VNĐ x100
            var bankCode = vnpay.GetResponseData("vnp_BankCode");
            var responseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var transactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
            var secureHash = collections["vnp_SecureHash"];
            var hashSecret = _vnPaySettings.HashSecret;

            // Kiểm tra chữ ký bảo mật
            bool isValidSignature = vnpay.ValidateSignature(secureHash, hashSecret);

            // Kiểm tra trạng thái giao dịch
            bool isSuccess = isValidSignature && responseCode == "00" && transactionStatus == "00";

            return new VnPaymentResponse(
                transactionId: transactionId,
                orderId: orderId,
                amount: amount,
                bankCode: bankCode,
                responseCode: responseCode,
                message: isSuccess ? "Giao dịch thành công" : GetResponseMessage(responseCode)
            );
        }

        // Phương thức lấy thông điệp phản hồi từ mã lỗi
        private string GetResponseMessage(string responseCode)
        {
            return responseCode switch
            {
                "00" => "Giao dịch thành công",
                "07" => "Trừ tiền thành công nhưng giao dịch bị nghi ngờ (liên hệ VNPay)",
                "09" => "Giao dịch đang chờ xử lý",
                "10" => "Giao dịch không hợp lệ",
                "24" => "Giao dịch bị hủy",
                _ => "Lỗi không xác định"
            };
        }


    }
}
