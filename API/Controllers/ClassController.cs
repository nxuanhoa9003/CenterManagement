using Application.InterfacesServices;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.DTOs.Class;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/class")]
    [ApiController]
    [Authorize]
    public class ClassController : ControllerBase
    {
        private readonly ILogger<ClassController> _logger;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IClassService _classService;

        public ClassController(ILogger<ClassController> logger, IMapper mapper, ITransactionService transactionService, IClassService classService)
        {
            _logger = logger;
            _mapper = mapper;
            _transactionService = transactionService;
            _classService = classService;
        }

        private async Task<(bool status, string message, ClassRequest? modelrs)> IsValidClass(ClassRequest model, bool IsAdd = true)
        {

            if (model.Id == Guid.Empty)
            {
                if (IsAdd)
                {
                    model.Id = Guid.NewGuid();
                }
                else
                {
                    return (false, "Id không được để trống", null);
                }
            }


            if (string.IsNullOrEmpty(model.Name))
            {
                return (false, "Tên lớp không được để trống", null);
            }

            if (await _classService.ClassNameExistsAsync(model.Name))
            {
                return (false, "Tên lớp đã tồn tại", null);
            }

            if (model.TeacherId == Guid.Empty)
            {
                return (false, "Lớp chưa có giáo viên", null);
            }

            if (model.TeacherId == Guid.Empty)
            {
                return (false, "Lớp chưa có giáo viên", null);
            }

            if (model.CourseId == Guid.Empty)
            {
                return (false, "Lớp chưa có khoá học", null);
            }

            // tìm khoá học thoả mãn

            return (true, "", model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetClasses()
        {
            try
            {
                var classes = await _classService.GetClasses();
                return classes.Any()
                    ? Ok(new ApiResponse
                    {
                        Message = "Lấy danh sách lớp thành công",
                        Data = classes
                    })
                    : NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Lỗi khi lấy danh sách lớp");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));
            }
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> FindClassById([FromRoute] string? id)
        {

            try
            {
                if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var classId))
                {
                    return BadRequest(new ApiResponse { Message = "Id không hợp lệ" });
                }


                var classrs = await _classService.FindClassById(classId);
                return classrs != null
                    ? Ok(new ApiResponse { Message = "Lấy dữ liệu thành công", Data = classrs })
                    : NotFound(new ApiResponse { Message = "Không tìm thấy lớp" });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Lỗi khi lấy thông tin lớp");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));
            }
        }

        [HttpGet("name/{name}")]
        [AllowAnonymous]
        public async Task<IActionResult> FindClassByName([FromRoute] string? name)
        {
            if (name == null)
                return BadRequest(new ApiResponse { Message = "Tên lớp là bắt buộc" });
            try
            {
                var classrs = await _classService.FindClassByName(name);
                if (classrs != null)
                {
                    return Ok(new ApiResponse { Message = "Tìm lớp thành công", Data = classrs });
                }
                return NotFound(new ApiResponse { Message = "Không tìm thấy lớp" });

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Lỗi khi lấy danh sách lớp");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));
            }
        }


        /*  Ongoing,   // Đang diễn ra
          Upcoming,  // Sắp diễn ra
          Completed  // Đã kết thúc*/
        [HttpGet("status/{status:regex(Ongoing|Upcoming|Completed)}")]
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> FindClassByStatus([FromRoute] string? status)
        {
            if (string.IsNullOrEmpty(status))
                return BadRequest(new ApiResponse { Message = "Trạng thái không được để trống" });

            if (!Enum.TryParse<ClassStatus>(status, true, out var classStatus))
                return BadRequest(new ApiResponse { Message = "Không xác định được giá trị trạng thái" });

            try
            {

                var classEntity = await _classService.GetClassesByStatus(classStatus);
                return !classEntity.Any()
                    ? NotFound(new ApiResponse { Message = "Không tìm thấy lớp với trạng thái đã cho" })
                    : Ok(new ApiResponse { Message = "Tìm lớp thành công", Data = classEntity });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Lỗi khi lấy danh sách lớp");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));
            }
        }

        [HttpGet("teacher/{Id}")]
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> GetClassesByTeacherId([FromRoute] string? Id)
        {
            if (Id == null)
                return BadRequest(new ApiResponse { Message = "Id giáo viên để được để trống" });

            try
            {
                if (!Guid.TryParse(Id, out var teacherId))
                {
                    return BadRequest(new ApiResponse { Message = "Id giáo viên không hợp lệ" });
                }
                var classEntity = await _classService.FindClassByTeacher(teacherId);
                return !classEntity.Any()
                    ? NotFound(new ApiResponse { Message = "Không tìm thấy lớp theo giáo viên" })
                    : Ok(new ApiResponse { Message = "Tìm lớp thành công", Data = classEntity });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Lỗi khi lấy danh sách lớp");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));
            }
        }

        [HttpGet("student/{classId}")]
        [Authorize(Policy = "CanGetStudentClass")]
        public async Task<IActionResult> GetStudentList([FromRoute] string? classId)
        {
            if (string.IsNullOrEmpty(classId))
                return BadRequest(new { status = false, message = "ClassId is null" });

            try
            {

                if (!Guid.TryParse(classId, out var classIdrs))
                {
                    return BadRequest(new ApiResponse { Message = "Id giáo viên không hợp lệ" });
                }
                var studentAmount = await _classService.CountStudentInClass(classIdrs);
                if (studentAmount == 0)
                    return NoContent();
                var studentList = _classService.GetStudentsInClass(classIdrs);
                return Ok(new ApiResponse
                {

                    Message = "Danh sách sinh viên",
                    Data = new
                    {
                        amount = studentAmount,
                        students = studentList
                    }
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Lỗi khi lấy danh sách sinh viên");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));
            }
        }



        [HttpPost]
        [Authorize(Policy = "CanAddClass")]
        public async Task<IActionResult> AddClass([FromBody] ClassRequest request)
        {
            var (status, message, model) = await IsValidClass(request);
            if (status == false)
            {
                return BadRequest(new ApiResponse(message));
            }

            try
            {
                if (await _classService.CheckClassIdExist(model.Id))
                {
                    return BadRequest(

                        new ApiResponse
                        {
                            Message = "Id đã tồn tại",
                            Data = model
                        });
                }
                var newClass = _mapper.Map<Domain.Entities.Class>(request);
                await _transactionService.ExecuteTransactionAsync(async () =>
                {
                    await _classService.AddClass(newClass);
                });
                return Ok(new ApiResponse { Message = "Thêm lớp thành công" });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Lỗi khi thêm lớp");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));
            }
        }


        [HttpPut]
        [Authorize(Policy = "CanUpdateClass")]
        public async Task<IActionResult> UpdateClass([FromBody] ClassRequest request)
        {

            if (!await _classService.CheckClassIdExist(request.Id))
            {
                return BadRequest(

                    new ApiResponse
                    {

                        Message = "Id không tồn tại",
                        Data = request
                    });
            }

            var (isValid, message, model) = await IsValidClass(request, IsAdd: false);
            if (isValid == false)
            {
                return BadRequest(new ApiResponse(message));
            }

            try
            {
                var classEntity = await _classService.FindClassById(request.Id);
                if (classEntity == null)
                    return NotFound(new ApiResponse { Message = "Class not found" });

                _mapper.Map(request, classEntity);

                await _transactionService.ExecuteTransactionAsync(async () =>
                {
                    await _classService.UpdateClass(classEntity);
                });
                return Ok(new ApiResponse { Message = "Cập nhật lớp thành công", Data = classEntity });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Lỗi khi cập nhật lớp");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));
            }
        }


        [HttpDelete("{id}")]
        [Authorize(Policy = "CanDeleteClass")]
        public async Task<ActionResult> DeleteClass([FromRoute] string id)
        {
            try
            {
                if (!Guid.TryParse(id, out var classId))
                {
                    return BadRequest(new ApiResponse { Message = "Invalid Id Class" });
                }
                if (!await _classService.CheckClassIdExist(classId))
                {
                    return BadRequest(new ApiResponse { Message = "Id không tồn tại" });

                }
                await _transactionService.ExecuteTransactionAsync(async () =>
                {
                    await _classService.DeleteClass(classId);
                });
                return Ok(new ApiResponse { Message = "Xoá lớp thành công" });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while deleting class");
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = "An error occurred while processing your request" });
            }
        }


        [HttpDelete("delete-student")]
        [Authorize(Policy = "CanDeleteStudentFromClass")]
        public async Task<ActionResult> DeleteStudentFromClass([FromBody] DeleteStudentClassRequest request)
        {
            if (request.classId == Guid.Empty)
                return BadRequest(new ApiResponse { Message = "Class id is required" });
            if (request.studentId == Guid.Empty)
                return BadRequest(new ApiResponse { Message = "Student id is required" });

            try
            {
                if (!await _classService.CheckStudentInClass(request.classId, request.studentId))
                    return BadRequest(new ApiResponse { Message = "Lớp không tồn tại" });
                await _transactionService.ExecuteTransactionAsync(async () =>
                   {
                       await _classService.DeleteStudentFromClass(request.classId, request.studentId);
                   });
                return Ok(new ApiResponse { Message = "Xoá sinh viên khỏi lớp thành công" });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while deleting student from class");
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = "An error occurred while processing your request" });
            }
        }

    }
}
