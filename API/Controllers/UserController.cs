using Application.Services;
using Domain.Enums;
using Application.InterfacesServices;
using FluentEmail.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.DTOs.User;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using Application.Mapper;
using AutoMapper;

namespace API.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {

        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthenticationService _authenticationService;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IOtpService _otpService;
        public readonly ICloudinaryService _cloudinaryService;
        public readonly IMapper _mapper;

        public UserController(ILogger<AuthenticationController> logger, IAuthenticationService authenticationService, ITokenService tokenService, IEmailService emailService, IOtpService otpService, ICloudinaryService cloudinaryService, IMapper mapper)
        {
            _logger = logger;
            _authenticationService = authenticationService;
            _tokenService = tokenService;
            _emailService = emailService;
            _otpService = otpService;
            _cloudinaryService = cloudinaryService;
            _mapper = mapper;
        }

        private (bool status, string message) IsValidUser(UserDTO model)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(model.Id))
            {
                errors.Add("Invalid User Id.");
            }

            if (string.IsNullOrWhiteSpace(model.FullName) || model.FullName.Length > 100)
            {
                errors.Add("Full Name is required and cannot exceed 100 characters.");
            }

            if (string.IsNullOrWhiteSpace(model.DateOfBirth) || !DateTime.TryParse(model.DateOfBirth, out _))
            {
                errors.Add("Date of Birth is required and must be a valid date (YYYY-MM-DD).");
            }
            if (model.Gender == null || !Enum.IsDefined(typeof(Gender), model.Gender))
            {
                errors.Add("Gender must be 1: Male, 2: Female, or 3: Other.");
            }
            if (string.IsNullOrWhiteSpace(model.Email) || !new EmailAddressAttribute().IsValid(model.Email))
            {
                errors.Add("Invalid email format.");

            }
            if (string.IsNullOrWhiteSpace(model.PhoneNumber) || !model.PhoneNumber.All(char.IsDigit))
            {
                errors.Add("Phone Number is required and must contain only digits.");
            }

            if (errors.Any())
            {
                return (false, string.Join(",", errors));
            }
            return (true, string.Empty);

        }

        [HttpGet("get-info-profile")]
        [Authorize]
        [AllowAnonymous]
        public async Task<IActionResult> GetInfoProfile([FromQuery] string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest(new ApiResponse
                    {

                        Message = "Giá trị không hợp lệ",
                    });
                }

                var user = await _authenticationService.GetUserByMail(email);
                if (user == null)
                {
                    return BadRequest(new ApiResponse
                    {

                        Message = "Không tìm thấy tài khoản",
                    });
                }

                var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (Id == null || Id != user.Id)
                {
                    return BadRequest(new ApiResponse
                    {

                        Message = "Tài khoản không khớp"
                    });
                }

                var userDto = new UserDTO();
                _mapper.Map(user, userDto);
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when register");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("update-profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UserDTO model)
        {
            try
            {
                var (status, message) = IsValidUser(model);
                if (!status)
                {
                    return BadRequest(new ApiResponse
                    {
                        Message = message
                    });
                }
                var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (Id == null || Id != model.Id)
                {
                    return BadRequest(new ApiResponse
                    {

                        Message = "Tài khoản không khớp"
                    });
                }

                var user = await _authenticationService.GetUserById(model.Id);
                if (user == null)
                {
                    return NotFound(new ApiResponse
                    {

                        Message = "Người dùng không tồn tại"
                    });
                }

                if (user.Email != model.Email)
                {
                    var isExist = await _authenticationService.CheckMailUserExist(model.Email);
                    if (isExist)
                    {
                        return BadRequest(new ApiResponse
                        {

                            Message = "Email đã tồn tại"
                        });
                    }
                    // cập nhật lại trạng thái xác thực khi đổi mail
                    user.EmailConfirmed = false;
                }

                // Ánh xạ từ UserDTO sang User
                _mapper.Map(model, user);
                await _authenticationService.UpdateUser(user);

                return Ok(new ApiResponse
                {

                    Message = "Cập nhật thành công"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when update profile");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("change-avatar")]
        [Authorize]
        public async Task<IActionResult> ChangeAvatar([FromForm] UploadProfileModel model)
        {
            try
            {
                var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (Id == null)
                {
                    return BadRequest(new ApiResponse
                    {

                        Message = "Tài khoản không khớp"
                    });
                }

                var user = await _authenticationService.GetUserById(Id);
                if (user == null)
                {
                    return NotFound(new ApiResponse
                    {

                        Message = "Người dùng không tồn tại"
                    });
                }

                if (!string.IsNullOrEmpty(user.AvatarUrl))
                {
                    var publicIdImage = _cloudinaryService.GetPublicIdFromUrl(user.AvatarUrl);
                    await _cloudinaryService.DeleteImageAsync(publicIdImage);
                }

                if (model.File == null || model.File.Length == 0)
                {
                    return BadRequest("No file uploaded");
                }
                string imageUrl = await _cloudinaryService.UploadImageAsync(model.File);
                user.AvatarUrl = imageUrl;
                await _authenticationService.UpdateUser(user);

                return Ok(new ApiResponse
                {

                    Message = "Cập nhật ảnh thành công",
                    Data = imageUrl
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when change avatar");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }

        // student
        [HttpGet("list-student")]
        [Authorize(Policy = "CanGetStudents")]
        public async Task<IActionResult> GetStudents()
        {
            var listUser = await _authenticationService.GetUserByRole(RoleUser.Student);
            return Ok(new ApiResponse
            {

                Message = "Danh sách sinh viên",
                Data = listUser ?? []
            });
        }

        //teacher
        [HttpGet("list-teacher")]
        [Authorize(Policy = "CanGetTeachers")]
        public async Task<IActionResult> GetTeachers()
        {
            var listUser = await _authenticationService.GetUserByRole(RoleUser.Teacher);
            return Ok(new ApiResponse
            {

                Message = "Danh sách giảng viên",
                Data = listUser ?? []
            });
        }

        //teacher
        [HttpGet("list-employee")]
        //[Authorize(Policy = "CanGetEmployees")]
        public async Task<IActionResult> GetEmployees()
        {
            var listUser = await _authenticationService.GetUserByRole(RoleUser.Employee);
            return Ok(new ApiResponse
            {

                Message = "Danh sách nhân viên",
                Data = listUser ?? []
            });
        }
    }
}
