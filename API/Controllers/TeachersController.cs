using Application.InterfacesServices;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.DTOs.User;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/teacher")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly ILogger<TeachersController> _logger;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly ITeacherService _teacherService;
        private readonly IAuthenticationService _authenticationService;

        public TeachersController(ILogger<TeachersController> logger, IMapper mapper, ITransactionService transactionService, ITeacherService teacherService, IAuthenticationService authenticationService)
        {
            _logger = logger;
            _mapper = mapper;
            _transactionService = transactionService;
            _teacherService = teacherService;
            _authenticationService = authenticationService;
        }

        private async Task<(bool success, string message)> IsValidTeacher(TeacherRequest request, bool IsAdd = true)
        {
            if (request == null)
            {
                return (false, "Invalid data");
            }

            if (string.IsNullOrEmpty(request.UserId))
            {
                return (false, "Invalid User Id");
            }

            if (request.TeacherId == Guid.Empty && !IsAdd)
            {
                return (false, "Invalid Teacher Id");
            }

            if (!await _authenticationService.CheckUserExistById(request.UserId))
            {
                return (false, "User is not exist");
            }

            if (string.IsNullOrEmpty(request.Specialty))
            {
                return (false, "Invalid Specialty");
            }

            return (true, "");
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTeacher(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new ApiResponse
                {

                    Message = "Id is not valid"
                });
            }

            try
            {
                var student = await _teacherService.GetTeacherByIdAsync(id);
                if (student == null) return NotFound();

                return Ok(new ApiResponse
                {

                    Message = "Get data successfully",
                    Data = student
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting teacher by name");
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = "An error occurred while processing your request" });
            }


        }


        [HttpGet("get-teachers")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTeachers()
        {

            try
            {
                var students = await _teacherService.GetAllTeachersAsync();


                return Ok(new ApiResponse
                {

                    Message = "Get data successfully",
                    Data = students
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting teacher by name");
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = "An error occurred while processing your request" });
            }


        }


        [HttpPost]
        [Authorize(Policy = "CanAddTeacher")]
        public async Task<ActionResult> AddTeacher([FromBody] TeacherRequest request)
        {
            try
            {
                var (success, message) = await IsValidTeacher(request);
                if (!success)
                {
                    return BadRequest(new ApiResponse
                    {

                        Message = message,
                    });
                }
                var newTeacher = _mapper.Map<Domain.Entities.Teacher>(request);
                await _transactionService.ExecuteTransactionAsync(async () =>
                {
                    await _teacherService.AddTeacherAsync(newTeacher);
                });
                return Ok(new ApiResponse { Message = "Add teacher successfully" });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while adding teacher");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Message = "An error occurred while processing your request" });
            }
        }


        [HttpPut("{id:guid}")]
        [Authorize(Policy = "CanUpdateTeacher")]
        public async Task<IActionResult> UpdateTeacher([FromBody] TeacherRequest request, [FromRoute] Guid id)
        {
            try
            {
                var (success, message) = await IsValidTeacher(request);
                if (!success)
                {
                    return BadRequest(new ApiResponse
                    {

                        Message = message,
                    });
                }
                var teacherEntity = await _teacherService.GetTeacherByIdAsync(id);
                if (teacherEntity == null)
                {
                    return NotFound(new ApiResponse { Message = "Teacher not found" });
                }

                _mapper.Map(request, teacherEntity);
                await _transactionService.ExecuteTransactionAsync(async () => { await _teacherService.UpdateTeacherAsync(teacherEntity); });
                return Ok(new { status = true, message = "Update teacher successfully" });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while updating teacher");
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = "An error occurred while processing your request" });
            }
        }


        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "CanDeleteTeacher")]
        public async Task<IActionResult> DeleteTeacher([FromRoute] Guid id)
        {
            try
            {
                if (!(await _teacherService.IsTeacherExist(id)))
                {
                    return NotFound(new ApiResponse { Message = "Teacher not found" });
                }
                await _transactionService.ExecuteTransactionAsync(async () => { await _teacherService.DeleteTeacherByIdAsync(id); });
                return Ok(new ApiResponse { Message = "Delete teacher successfully" });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while deleting teacher");
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = "An error occurred while processing your request" });
            }
        }

    }
}
