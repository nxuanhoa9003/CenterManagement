using Application.InterfacesServices;
using Application.Services;
using AutoMapper;
using CloudinaryDotNet.Actions;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using Shared.DTOs.Order;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IOrderService _orderService;
        private readonly IStudentService _studentService;
        private readonly ICourseService _courseService;

        public OrderController(ILogger<OrderController> logger, IMapper mapper, ITransactionService transactionService, IOrderService orderService, IStudentService studentService, ICourseService courseService)
        {
            _logger = logger;
            _mapper = mapper;
            _transactionService = transactionService;
            _orderService = orderService;
            _studentService = studentService;
            _courseService = courseService;
        }

        [HttpPost("create-order")]
        [Authorize]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequest request)
        {
            try
            {
                var student = await _studentService.GetStudentById(request.StudentId);
                var course = await _courseService.GetCourseById(request.CourseId);

                if (student == null || course == null)
                {
                    return BadRequest(new ApiResponse
                    {
                       
                        Message = "Student or Course not found."
                    });
                }

                var order = new Order
                {
                    StudentId = request.StudentId,
                    CourseId = request.CourseId,
                    TotalAmount = course.Price,
                    Status = (Domain.Enums.OrderStatus)1
                };

                await _transactionService.ExecuteTransactionAsync(async () =>
                {
                    await _orderService.AddOrder(order);
                });

                return Ok(order);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while finding class by teacherId");
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = "An error occurred while processing your request" });
            }

        }


        [HttpGet("get-orders")]
        [Authorize(Policy = "CanGetAllOrders")]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var listorders = await _orderService.GetOrdersAsync();
                return Ok(new ApiResponse
                {
                    
                    Message = "Get data successful",
                    Data = listorders
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while finding class by teacherId");
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = "An error occurred while processing your request" });
            }
        }

        [HttpGet("get-order-student/{id:guid}")]
        [Authorize(Policy = "CanGetOrderStudent")]
        public async Task<IActionResult> GetOrderStudent([FromRoute] Guid? id)
        {
            try
            {
                if (id == null || id == Guid.Empty)
                {
                    return BadRequest(new ApiResponse
                    {
                       
                        Message = "Invalid Id"
                    });
                }

                var order = await _orderService.GetOrdersByStudentAsync(id.Value);
                return Ok(new ApiResponse
                {
                    
                    Message = "Get data successful",
                    Data = order
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while finding class by teacherId");
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = "An error occurred while processing your request" });
            }
        }


        [HttpGet("get-my-order-student/{id:guid}")]
        public async Task<IActionResult> GetMyOrder()
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

                var order = await _orderService.GetOrdersByStudentAsync(student.StudentId.Value);

                if (order == null)
                {
                    return BadRequest(new ApiResponse
                    {
                       
                        Message = "Not found order",

                    });
                }

                return Ok(new ApiResponse
                {
                    
                    Message = "Get data successful",
                    Data = order
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing your request");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Message = "An error occurred while processing your request" });
            }
        }

    }
}
