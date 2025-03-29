using Application.InterfacesServices;
using Application.Services;
using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.DTOs.Email;
using Shared.DTOs.User;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IStudentService _studentService;
        private readonly IEnrollmentService _enrollmentService;
        private readonly IAuthenticationService _authenticationService;

        public StudentController(ILogger<StudentController> logger, IMapper mapper, ITransactionService transactionService, IStudentService studentService, IEnrollmentService enrollmentService, IAuthenticationService authenticationService)
        {
            _logger = logger;
            _mapper = mapper;
            _transactionService = transactionService;
            _studentService = studentService;
            _enrollmentService = enrollmentService;
            _authenticationService = authenticationService;
        }


        private async Task<(bool success, string message)> IsValidStudent(StudentRequest request, bool IsAdd = true)
        {
            if (request == null)
            {
                return (false, "Invalid data");
            }

            if (string.IsNullOrEmpty(request.UserId))
            {
                return (false, "Invalid User Id");
            }

            if (request.StudentId == Guid.Empty && !IsAdd)
            {
                return (false, "Invalid Student Id");
            }

            if (!await _authenticationService.CheckUserExistById(request.UserId))
            {
                return (false, "User is not exist");
            }
            return (true, "");
        }


        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            try
            {
                var students = await _studentService.GetAllStudentsAsync();
                return students.Any()
                    ? Ok(new ApiResponse { Message = "Get data successfully", Data = students })
                    : NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while getting all teacher");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Message = "An error occurred while processing your request" });
            }
        }


        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetStudentsByName([FromRoute] string? name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    return BadRequest(new ApiResponse
                    {

                        Message = "Invalid name student",

                    });
                }

                var students = await _studentService.GetStudentsByNames(name);
                return students.Any()
                    ? Ok(new ApiResponse { Message = "Get data successfully", Data = students })
                    : NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while getting all teacher");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Message = "An error occurred while processing your request" });
            }
        }


        [HttpGet("phone/{phone:regex(^\\d{{10}}$)}")]
        public async Task<IActionResult> GetStudentsByPhone([FromRoute] string? phone)
        {
            try
            {
                if (string.IsNullOrEmpty(phone))
                {
                    return BadRequest(new ApiResponse
                    {

                        Message = "Invalid name student",

                    });
                }
                var students = await _studentService.GetStudentsByPhone(phone);
                return students.Any()
                    ? Ok(new ApiResponse { Message = "Get data successfully", Data = students })
                    : NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while getting all teacher");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Message = "An error occurred while processing your request" });
            }
        }


        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public IActionResult GetStudentById([FromRoute] Guid? id)
        {
            try
            {
                if (id == null || id == Guid.Empty)
                {
                    return BadRequest(new ApiResponse { Message = "Invalid student ID" });
                }

                var student = _studentService.GetStudentById(id);
                return student != null
                    ? Ok(new ApiResponse { Message = "Get data successfully", Data = student })
                    : NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while getting all teacher");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Message = "An error occurred while processing your request" });
            }
        }


        [HttpGet("class/{id:guid}")]
        public async Task<IActionResult> GetClassByStudentId([FromRoute] Guid? id)
        {
            if (id == null)
                return BadRequest(new { status = false, message = "Id is required" });
            try
            {
                var count = await _enrollmentService.CountClassBuyStudentAsync(id.Value);
                if (count == 0)
                    return NoContent();
                var classes = await _enrollmentService.GetClassesByStudentIdAsync(id.Value);
                return Ok(new ApiResponse
                {

                    Message = "Get data successfully",
                    Data = new
                    {
                        amount = count,
                        classes = classes
                    }
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while getting all teacher");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Message = "An error occurred while processing your request" });
            }
        }



        [HttpPost]
        [Authorize(Policy = "CanAddStudent")]
        public async Task<IActionResult> AddStudent([FromBody] StudentRequest student)
        {
            try
            {

                var (success, message) = await IsValidStudent(student);
                if (!success)
                {
                    return BadRequest(new ApiResponse
                    {

                        Message = message,
                    });
                }

                var newStudent = _mapper.Map<Domain.Entities.Student>(student);
                await _transactionService.ExecuteTransactionAsync(async () => { await _studentService.AddStudent(newStudent); });
                return Ok(new ApiResponse { Message = "Add student successfully" });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while getting all teacher");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Message = "An error occurred while processing your request" });
            }
        }


        [HttpPut]
        [Authorize(Policy = "CanUpdateStudent")]
        public async Task<IActionResult> UpdateStudent([FromBody] StudentRequest student, [FromRoute] Guid id)
        {
            try
            {
                var (success, message) = await IsValidStudent(student);
                if (!success)
                {
                    return BadRequest(new ApiResponse
                    {

                        Message = message,
                    });
                }

                var studentEntity = await _studentService.GetStudentById(id);
                if (studentEntity == null)
                {
                    return NotFound(new ApiResponse { Message = "Student not found" });
                }
                _mapper.Map(student, studentEntity);
                await _transactionService.ExecuteTransactionAsync(async () => { await _studentService.UpdateStudent(studentEntity); });
                return Ok(new ApiResponse { Message = "Update teacher successfully" });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while getting all teacher");
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = "An error occurred while processing your request" });
            }
        }


        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "CanDeleteStudent")]
        public async Task<ActionResult> DeleteStudent([FromRoute] Guid id)
        {
            try
            {
                var studentEntity = await _studentService.GetStudentById(id);
                if (studentEntity == null)
                {
                    return NotFound(new ApiResponse { Message = "Student not found" });
                }
                await _transactionService.ExecuteTransactionAsync(async () => { await _studentService.DeleteStudent(id); });
                return Ok(new ApiResponse { Message = "Delete student successfully" });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while getting all teacher");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Message = "An error occurred while processing your request" });
            }
        }

    }
}
