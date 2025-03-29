using Application.InterfacesServices;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using Shared.DTOs.Enrollment;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EnrollmentController : ControllerBase
    {
        private readonly ILogger<ClassController> _logger;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IEnrollmentService _enrollmentService;
        private readonly IOrderService _orderService;

        public EnrollmentController(ILogger<ClassController> logger, IMapper mapper, ITransactionService transactionService, IEnrollmentService enrollmentService, IOrderService orderService)
        {
            _logger = logger;
            _mapper = mapper;
            _transactionService = transactionService;
            _enrollmentService = enrollmentService;
            _orderService = orderService;
        }

        [HttpPost("enroll")]
        [Authorize(Policy = "CanEnrollStudent")]
        public async Task<IActionResult> EnrollStudent([FromBody] EnrollmentRequest request)
        {
            if (request.StudentId == Guid.Empty || request.ClassId == Guid.Empty)
            {
                return BadRequest(new ApiResponse
                {
                    Message = "Student ID and Class ID are required."
                });
            }

            try
            {
                var existingEnrollment = await _enrollmentService.IsStudentEnrolled(request.StudentId, request.ClassId);
                if (existingEnrollment)
                {
                    return BadRequest(new ApiResponse
                    {
                        
                        Message = "Student is already enrolled in this class."
                    });
                }

                var IsOrderPaid = await _orderService.IsStudentPaid(request.StudentId, request.CourseId);
                if (!IsOrderPaid)
                {
                    return BadRequest(new ApiResponse
                    {
                        Message = "The student has not paid for the course."
                    });
                }

               
                var enrollment = _mapper.Map<Domain.Entities.Enrollment>(request);

               
                enrollment.EnrolledAt = DateTime.UtcNow;
                enrollment.Status = EnrollmentStatus.Active;
                await _transactionService.ExecuteTransactionAsync(async () =>
                {
                    await _enrollmentService.AddEnrollmentAsync(enrollment);
                });

                return Ok(new ApiResponse
                {

                    Message = "Enrollment successful."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm sinh viên");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));
            }
        }


        [HttpDelete("unenroll/{studentId}/{classId}")]
        [Authorize(Policy = "CanUnenrollStudent")]
        public async Task<IActionResult> UnenrollStudent([FromRoute] Guid studentId, [FromRoute] Guid classId)
        {

            if (studentId == Guid.Empty)
            {
                return NotFound(new ApiResponse
                {
                    
                    Message = "Student Id not found."
                });
            }

            if (classId == Guid.Empty)
            {
                return NotFound(new ApiResponse
                {
                    
                    Message = "Class Id not found."
                });
            }
            try
            {
                var enrollment = await _enrollmentService.GetEnrollmentAsync(studentId, classId);
                if (enrollment == null)
                {
                    return NotFound(new ApiResponse
                    {
                        
                        Message = "Enrollment record not found."
                    });
                }

                await _transactionService.ExecuteTransactionAsync(async () =>
                {
                    await _enrollmentService.RemoveEnrollmentAsync(enrollment);
                });

                return Ok(new ApiResponse
                {

                    Message = "Student has been unenrolled successfully."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xoá sinh viên khỏi khoá học");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));
            }

        }


        [HttpGet("students/{classId}")]
        public async Task<IActionResult> GetStudentsByClass([FromRoute] Guid classId)
        {
            try
            {
                var students = await _enrollmentService.GetStudentsByClassIdAsync(classId);
                if (!students.Any())
                {
                    return NotFound(new ApiResponse
                    {
                        
                        Message = "No students found for this class."
                    });
                }

                return Ok(new ApiResponse
                {

                    Message = "Students retrieved successfully.",
                    Data = students
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách sinh viên của lớp");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));
            }

        }


        [HttpGet("classes/{studentId}")]
        public async Task<IActionResult> GetClassesByStudent([FromRoute] Guid studentId)
        {
            try
            {
                var classes = await _enrollmentService.GetClassesByStudentIdAsync(studentId);
                if (!classes.Any())
                {
                    return NotFound(new ApiResponse
                    {
                        
                        Message = "No classes found for this student."
                    });
                }

                return Ok(new ApiResponse
                {

                    Message = "Classes retrieved successfully.",
                    Data = classes
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy dữ liệu");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));
            }

        }

    }
}
