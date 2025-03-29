using Application.InterfacesServices;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.DTOs.Payment;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly IVnPayService _vpnPayService;
        private readonly IStudentService _studentService;
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;
        private readonly ITransactionService _transactionService;

        public PaymentController(ILogger<PaymentController> logger, IVnPayService vpnPayService, IStudentService studentService, IOrderService orderService, IPaymentService paymentService, ITransactionService transactionService)
        {
            _logger = logger;
            _vpnPayService = vpnPayService;
            _studentService = studentService;
            _orderService = orderService;
            _paymentService = paymentService;
            _transactionService = transactionService;
        }

        [HttpPost("handel-payment")]
        [Authorize]
        public async Task<IActionResult> HandelPayment([FromBody] PaymentRequest request)
        {
            try
            {

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var student = await _studentService.GetStudentsByUserId(userId);

                if (student == null)
                {
                    return BadRequest(new ApiResponse
                    {

                        Message = "Not found student",

                    });
                }

                var order = await _orderService.GetOrderByIdAsync(request.OrderId);

                if (order == null || order.StudentId != student.StudentId)
                {
                    return BadRequest(new ApiResponse
                    {

                        Message = "Not found order",

                    });
                }


                // Gọi hàm tạo URL thanh toán
                string paymentUrl = await _vpnPayService.CreatePaymentUrl(order);

                // Trả về URL để kiểm tra
                return Ok(new { paymentUrl });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing your request");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Message = "An error occurred while processing your request" });
            }

        }

        [HttpGet("vnpay-return")]
        [Authorize]
        public async Task<IActionResult> VNPayReturn()
        {
            try
            {
                var response = _vpnPayService.PaymentExecute(Request.Query);

                if (response.IsSuccess)
                {
                    // Cập nhật trạng thái đơn hàng

                    var order = await _orderService.GetOrderByIdAsync(Guid.Parse(response.OrderId));
                    if (order == null)
                    {
                        return BadRequest(new ApiResponse { Message = "Không tìm thấy đơn hàng" });
                    }

                    await _transactionService.ExecuteTransactionAsync(async () =>
                    {
                        order.Status = OrderStatus.Paid;
                        await _orderService.UpdateOrder(order);

                        // Tạo bản ghi thanh toán
                        var payment = new Payment
                        {
                            OrderId = order.Id,
                            StudentId = order.StudentId,
                            Amount = response.Amount,
                            PaymentMethod = response.BankCode,
                            TransactionId = response.TransactionId,
                            PaymentDate = DateTime.UtcNow
                        };

                        await _paymentService.AddPaymentAsync(payment);
                    });
                    return Ok(new ApiResponse { Message = "Thanh toán thành công", Data = response });
                }
                else
                {
                    return BadRequest(new { message = "Thanh toán thất bại", response });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing your request");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Message = "An error occurred while processing your request" });
            }

        }

    }
}
