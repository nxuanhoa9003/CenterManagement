using Application.InterfacesServices;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.DTOs.Lesson;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/lesson")]
    [ApiController]
    [Authorize]
    public class LessonController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly IMapper _mapper;
        private readonly ILessonService _lessonService;
        private readonly ITransactionService _transactionService;

        public LessonController(ILogger<CategoryController> logger, IMapper mapper, ILessonService lessonService, ITransactionService transactionService)
        {
            _logger = logger;
            _mapper = mapper;
            _lessonService = lessonService;
            _transactionService = transactionService;
        }

        private (bool IsValid, string Message) ValidateLessonRequest(LessonRequest request)
        {
            if (request == null)
                return (false, "Request cannot be null.");

            if (request.ClassId == null || request.ClassId == Guid.Empty)
                return (false, "ClassId is required.");

            if (request.Date == default)
                return (false, "Date is required.");

            if (string.IsNullOrWhiteSpace(request.Name))
                return (false, "Lesson name is required.");

            return (true, "Valid request.");
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetLessons()
        {
            try
            {
                var lessons = await _lessonService.GetAllLesson();
                return lessons.Any()
                    ? Ok(new ApiResponse { Message = "Get data successfully", Data = lessons })
                    : NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while getting branch");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Message = "An error occurred while processing your request" });
            }
        }

        [HttpGet("name/{name}")]
        [AllowAnonymous]
        public async Task<IActionResult> FindByName([FromRoute] string name)
        {
            try
            {
                var lessons = await _lessonService.GetByNames(name);
                return lessons.Any()
                    ? Ok(new ApiResponse { Message = "Get data successfully", Data = lessons })
                    : NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Lỗi khi lấy thông tin bài học");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> FindById([FromRoute] string id)
        {

            try
            {

                if (!Guid.TryParse(id, out var lessonId))
                {
                    return BadRequest(new ApiResponse { Message = "Invalid Id Lesson" });
                }
                var lesson = await _lessonService.GetLessonByIdAsync(lessonId);
                return lesson != null
                    ? Ok(new ApiResponse { Message = "Get data successfully", Data = lesson })
                    : NotFound(new ApiResponse { Message = "No lesson was found" });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Lỗi khi lấy thông tin bài học");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));
            }
        }

        [HttpPost("add-class")]
        [Authorize(Policy = "CanAddLesson")]
        public async Task<ActionResult> AddLesson([FromBody] LessonRequest request)
        {
            var (isValid, message) = ValidateLessonRequest(request);
            if (!isValid)
            {
                return BadRequest(new ApiResponse
                {

                    Message = message
                });
            }
            try
            {

                var lessonEntity = _mapper.Map<Domain.Entities.Lesson>(request);
                lessonEntity.Id = Guid.NewGuid();
                await _transactionService.ExecuteTransactionAsync(async () => { await _lessonService.AddLessonAsync(lessonEntity); });
                return Ok(new ApiResponse { Message = "Add lesson successfully" });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Lỗi khi thêm bài học mới");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));
            }
        }


        [HttpPut]
        [Authorize(Policy = "CanUpdateLesson")]
        public async Task<ActionResult> UpdateLesson([FromBody] LessonRequest request)
        {
            var (isValid, message) = ValidateLessonRequest(request);
            if (!isValid)
            {
                return BadRequest(new ApiResponse
                {

                    Message = message
                });
            }

            try
            {
                if (request.Id == Guid.Empty) return BadRequest(new ApiResponse { Message = "Lesson id is null" });

                var lessonEntity = await _lessonService.GetLessonByIdAsync(request.Id);
                if (lessonEntity == null)
                    return NotFound(new ApiResponse { Message = "Lesson not found" });

                _mapper.Map(request, lessonEntity);

                await _transactionService.ExecuteTransactionAsync(async () => { await _lessonService.UpdateLessonAsync(lessonEntity); });

                return Ok(new ApiResponse { Message = "Update lesson successfully" });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Lỗi khi cập nhật bài học");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));
            }
        }


        [HttpDelete("{id}")]
        [Authorize(Policy = "CanDeleteLesson")]
        public async Task<IActionResult> DeleteLesson([FromRoute] string id)
        {
            try
            {
                if (!Guid.TryParse(id, out var lessonId))
                {
                    return BadRequest(new ApiResponse { Message = "Invalid Id Lesson" });
                }
                var lesson = await _lessonService.GetLessonByIdAsync(lessonId);

                if (lesson == null)
                {
                    return NotFound(new ApiResponse { Message = "No lesson was found" });
                }

                await _transactionService.ExecuteTransactionAsync(async () => { await _lessonService.DeleteLessonAsync(lessonId); });
                return Ok(new ApiResponse { Message = "Delete lesson successfully" });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Lỗi khi xoá bài học");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));
            }
        }

    }
}
