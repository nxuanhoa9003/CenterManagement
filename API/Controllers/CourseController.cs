using Application.InterfacesServices;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.DTOs.Course;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/course")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ILogger<CourseController> _logger;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly ICourseService _courseService;
        private readonly ICategoryService _categoryService;

        public CourseController(ILogger<CourseController> logger, IMapper mapper, ITransactionService transactionService, ICourseService courseService, ICategoryService categoryService)
        {
            _logger = logger;
            _mapper = mapper;
            _transactionService = transactionService;
            _courseService = courseService;
            _categoryService = categoryService;
        }

        private async Task<(bool success, string message, CourseRequest? model)> IsValidCourse(CourseRequest request, bool IsAdd = true)
        {

            if (request.Id == Guid.Empty)
            {
                if (IsAdd)
                {
                    request.Id = Guid.NewGuid();
                }
                else
                {
                    return (false, "Course id is required", null);
                }
            }
            if (await _courseService.IsCourseExists(request.Id))
            {
                return (false, "Course id already exists", null);
            }

            if (request.CategoryId == Guid.Empty || !(await _categoryService.CategoryExists(request.CategoryId)))
            {
                return (false, "Category id not exists", null);
            }

            return (true, "", request);
        }


        [HttpGet("get-courses")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCourses()
        {
            try
            {
                var courses = await _courseService.GetCourses();
                return courses.Any()
                    ? Ok(new ApiResponse { Message = "Get data successfully", Data = courses })
                    : NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Lỗi khi lấy danh sách khoá học");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));
            }
        }

        [HttpGet("get-courses-name")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCoursesByNames([FromQuery] string? name)
        {
            try
            {
                var courses = await _courseService.GetCoursesByNames(name);
                return courses.Any()
                    ? Ok(new ApiResponse { Message = "Get data successfully", Data = courses })
                    : NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Lỗi khi lấy danh sách khoá học");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));
            }
        }

        [HttpPost]
        [Authorize(Policy = "CanAddCourse")]
        public async Task<IActionResult> AddCourse([FromBody] CourseRequest request)
        {

            var (success, message, model) = await IsValidCourse(request);
            if (!success)
            {
                return BadRequest(new ApiResponse(message));

            }
            try
            {
                var courseEntity = _mapper.Map<Domain.Entities.Course>(request);
                await _transactionService.ExecuteTransactionAsync(async () => { await _courseService.AddCourse(courseEntity); });
                return Ok(new ApiResponse { Message = "Add course successfully" });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Lỗi khi thêm khoá học");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));
            }
        }

        [HttpPut]
        [Authorize(Policy = "CanUpdateCourse")]
        public async Task<IActionResult> UpdateCourse([FromBody] CourseRequest request)
        {
            var (success, message, model) = await IsValidCourse(request, false);
            if (!success)
            {
                return BadRequest(new ApiResponse(message));

            }
            try
            {
                var courseEntity = await _courseService.GetCourseById(request.Id);

                if (courseEntity == null)
                {
                    return BadRequest(new ApiResponse
                    {
                        Message = "Course is not found"
                    });
                }

                _mapper.Map(request, courseEntity);

                await _transactionService.ExecuteTransactionAsync(async () => { await _courseService.UpdateCourse(courseEntity); });
                return Ok(new ApiResponse { Message = "Update course successfully" });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Lỗi khi cập nhật khoá học");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "CanDeleteCourse")]
        public async Task<IActionResult> DeleteCourse([FromRoute] Guid id)
        {
            try
            {
                if (id == Guid.Empty || await _courseService.IsCourseExists(id))
                {
                    return BadRequest(new ApiResponse
                    {
                        
                        Message = "No course id was found"
                    });
                }
                await _transactionService.ExecuteTransactionAsync(async () => { await _courseService.DeleteCourse(id); });
                return Ok(new ApiResponse { Message = "Delete course successfully" });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Lỗi khi xoá khoá học");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));
            }
        }

    }
}
