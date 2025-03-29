using Application.InterfacesServices;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using Shared.DTOs.Auth;

namespace API.Controllers
{
    [Route("api/attendance")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly IMapper _mapper;
        private readonly IAttendanceService _attendanceService;
        private readonly ITransactionService _transactionService;

        public AttendanceController(ILogger<CategoryController> logger, IMapper mapper, IAttendanceService attendanceService, ITransactionService transactionService)
        {
            _logger = logger;
            _mapper = mapper;
            _attendanceService = attendanceService;
            _transactionService = transactionService;
        }

        private (bool IsValid, string Message) ValidateAttendanceRequest(AttendanceRequest request)
        {
            if (request.LessonId == Guid.Empty)
                return (false, "LessonId is required.");

            if (request.StudentId == Guid.Empty)
                return (false, "StudentId is required.");

            if (request.Date == DateTime.MinValue)
                return (false, "Invalid attendance date.");

            if (!Enum.IsDefined(typeof(AttendanceStatusDTO), request.AttendanceStatus))
                return (false, "Invalid attendance status.");

            if (request.Evaluation.HasValue && (request.Evaluation < 0 || request.Evaluation > 10))
                return (false, "Evaluation must be between 0 and 10.");

            return (true, "Valid request.");
        }



        [HttpPost("get-attendance-by-id")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAttendanceById([FromBody] AttendaceInfoRequet request)
        {
            try
            {
                var attendance = await _attendanceService.GetAttendancesByClassLessonId(request.classId, request.lessonId);
                return attendance.Any()
                    ? Ok(new ApiResponse
                    {
                        Message = "Get data successfully",
                        Data = attendance
                    })
                    : NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy bản ghi điểm danh");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));

            }
        }


        [HttpGet("class/{classId}")]
        public async Task<IActionResult> GetAttendanceByClass([FromRoute] Guid classId)
        {
            if (classId == Guid.Empty)
            {
                return BadRequest(new ApiResponse("classId không hợp lệ"));
            }
            var attendances = await _attendanceService.GetAttendancesByClassId(classId);
            return Ok(new ApiResponse { Message = "Get data successfully", Data = attendances });
        }


        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetAttendancesByStudent([FromRoute] Guid studentId)
        {
            var attendances = await _attendanceService.GetAttendancesByStudentId(studentId);

            if (!attendances.Any())
            {
                return NotFound(new ApiResponse("Không tìm thấy bản điểm danh nào"));
            }

            return Ok(new ApiResponse { Message = "Get data successfully", Data = attendances });
        }



        [HttpPost("record")]
        [Authorize(Policy = "CanRecordAttendance")]
        public async Task<IActionResult> RecordAttendance([FromBody] AttendanceRequest request)
        {
            var (isValid, message) = ValidateAttendanceRequest(request);
            if (!isValid)
            {
                return BadRequest(new ApiResponse(message));
            }

            try
            {
                // Gọi service để thêm bản ghi điểm danh
                var attendance = _mapper.Map<Attendance>(request);
                attendance.Id = Guid.NewGuid();
                await _transactionService.ExecuteTransactionAsync(async () =>
                {
                    await _attendanceService.AddAttendanceAsync(attendance);
                });
                return Ok(new ApiResponse("Đã ghi nhận điểm danh thành công."));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm bản ghi điểm danh");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));
            }
        }


        [HttpPut("{attendanceId}")]
        [Authorize(Policy = "CanUpdateAttendance")]
        public async Task<IActionResult> UpdateAttendance([FromRoute] Guid attendanceId, [FromBody] AttendanceRequest request)
        {
            try
            {
                var existingAttendance = await _attendanceService.GetAttendancesById(attendanceId);

                if (existingAttendance == null)
                {
                    return NotFound(new ApiResponse("Không tìm thấy bản điểm danh nào"));
                }


                await _transactionService.ExecuteTransactionAsync(async () =>
                {
                    // Ánh xạ từ request vào đối tượng hiện có
                    _mapper.Map(request, existingAttendance);
                    await _attendanceService.UpdateAttendanceAsync(existingAttendance);
                });

                return Ok(new ApiResponse { Message = "Attendance updated successfully", Data = existingAttendance });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật bản ghi điểm danh");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));
            }
        }

        [HttpDelete("{attendanceId}")]
        [Authorize(Policy = "CanDeleteAttendance")]
        public async Task<IActionResult> DeleteAttendance([FromRoute] Guid attendanceId)
        {
            try
            {
                await _attendanceService.DeleteAttendanceAsync(attendanceId);
                return Ok(new ApiResponse { Message = "Xoá bản điểm danh thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xoá bản ghi điểm danh");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));

            }
        }


    }
}
