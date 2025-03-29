using Application.Services;
using Domain.Entities;
using Application.InterfacesServices;
using FluentEmail.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.DTOs.Auth;
using Shared.DTOs.Email;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static System.Net.WebRequestMethods;
using Microsoft.AspNetCore.Authorization;
using Domain.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Azure.Core;
using System.Drawing;

namespace API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly ITransactionService _transactionService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IAuthorizationServices _authorizationServices;
        private readonly IStudentService _studentService;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IOtpService _otpService;

        public AuthenticationController(ILogger<AuthenticationController> logger, ITransactionService transactionService, IAuthenticationService authenticationService, IAuthorizationServices authorizationServices, IStudentService studentService, ITokenService tokenService, IEmailService emailService, IOtpService otpService)
        {
            _logger = logger;
            _transactionService = transactionService;
            _authenticationService = authenticationService;
            _authorizationServices = authorizationServices;
            _studentService = studentService;
            _tokenService = tokenService;
            _emailService = emailService;
            _otpService = otpService;
        }

        private bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) return false;
            var hasUpperChar = password.Any(char.IsUpper);
            var hasLowerChar = password.Any(char.IsLower);
            var hasDigit = password.Any(char.IsDigit);
            var hasSpecialChar = password.Any(ch => !char.IsLetterOrDigit(ch));
            var minLength = password.Length >= 6;
            return hasUpperChar && hasLowerChar && hasDigit && hasSpecialChar && minLength;
        }

        private async Task<(bool state, string message, ApplicationUser? user)> IsValidUserLogin(LoginRequest model)
        {
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                return (false, "Username or password is empty", null);
            }

            if (!await _authenticationService.CheckUserExist(model.Username))
            {
                return (false, "Tài khoản không tồn tại", null);
            }

            var usermodel = await _authenticationService.GetUserByUserName(model.Username);

            if (usermodel == null)
            {
                return (false, "Không tìm thấy tài khoản", null);
            }

            if (usermodel.LockoutEnd.HasValue && usermodel.LockoutEnd > DateTime.UtcNow)
            {
                return (false, "Tài khoản đã bị khóa", usermodel);
            }

            if (!usermodel.EmailConfirmed)
            {
                return (false, "Tài khoản chưa xác thực", usermodel);
            }

            /* if (await _authenticationService.IsLockedAccount(model.Username))
             {
                 return (false, "Tài khoản đã bị khóa", usermodel);
             }

             if (!await _authenticationService.IsConfirmEmailAccount(model.Username))
             {
                 return (false, "Tài khoản chưa xác thực", usermodel);
             }*/

            return (true, "", usermodel);
        }

        private (bool state, string message) IsValidRefreshToken(RefreshToken token, string accessTokenId, string userId)
        {
            if (token == null)
            {
                return (false, "Refresh token không tồn tại.");
            }

            if (token.AccessTokenId != accessTokenId)
            {
                return (false, "Refresh token không khớp với Access token.");
            }
            if (token.UserId != userId)
            {
                return (false, "Refresh token không thuộc về người dùng này.");
            }

            if (token.ExpiryDate < DateTime.UtcNow)
            {
                return (false, "Refresh token đã hết hạn.");
            }

            if (token.IsRevoked)
            {
                return (false, "Refresh token đã bị thu hồi.");
            }

            return (true, "");
        }

        private async Task<(bool state, string message)> IsValidUserRegister(RegisterRequest model)
        {
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                return (false, "Username or password is empty");
            }

            if (model.Username == model.Password)
            {
                return (false, "Username and password cannot be the same");
            }

            if (IsValidPassword(model.Password))
            {
                return (false, "Invalid password");
            }

            if (string.IsNullOrEmpty(model.Email))
            {
                return (false, "Mail is empty");
            }
            if (string.IsNullOrEmpty(model.FullName))
            {
                return (false, "Fullname is empty");
            }

            if (await _authenticationService.CheckUserExist(model.Username))
            {
                return (false, "Username is exist");
            }

            if (await _authenticationService.CheckMailUserExist(model.Email))
            {
                return (false, "Mail is exist");
            }

            return (true, "");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            try
            {
                var (state, message, user) = await IsValidUserLogin(model);
                if (!state)
                {
                    return BadRequest(new ApiResponse(message));
                }

                if (user == null)
                {
                    return Unauthorized(new ApiResponse("Đăng nhập thất bại"));
                }
                // kiểm tra username và password 
                if (!await _authenticationService.CheckAccountValid(model))
                {
                    await _transactionService.ExecuteTransactionAsync(async () =>
                    {
                        await _authenticationService.FailLogin(user);
                    });

                    return Unauthorized(new ApiResponse("Thông tin đăng nhập không chính xác"));
                }


                await _transactionService.ExecuteTransactionAsync(async () =>
                {
                    await _authenticationService.ResetFailLogin(user);
                });

                var userRespone = await _authenticationService.LoginAsync(model);

                var accessToken = "";
                var refreshToken = "";

                await _transactionService.ExecuteTransactionAsync(async () =>
                 {
                     (string Token, string AccessTokenId) = await _tokenService.GenerateToken(userRespone);
                     //accessToken = _tokenService.GenerateToken(userRespone, out string accessTokenId);
                     accessToken = Token;
                     refreshToken = _tokenService.GenerateRefreshToken();
                     await _tokenService.SaveRefreshTokenAsync(userRespone.UserId, refreshToken, AccessTokenId);
                 });

                return Ok(new ApiResponse
                {
                    Message = "Đăng nhập thành công",
                    Data = new
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        Info = userRespone
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when login");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                // Giải mã Access Token để lấy `jti`
                var principal = _tokenService.ValidateAccessToken(request.AccessToken);
                if (principal == null)
                {
                    return Unauthorized(new ApiResponse("Access Token không hợp lệ"));
                }

                var expClaim = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);
                if (expClaim == null)
                {
                    return Unauthorized(new ApiResponse("Access Token không hợp lệ"));
                }
                var expTimestamp = long.Parse(expClaim.Value);
                var expiryDate = DateTimeOffset.FromUnixTimeSeconds(expTimestamp).UtcDateTime;

                if (expiryDate > DateTime.UtcNow.AddMinutes(3))
                {
                    return BadRequest(new ApiResponse("Access Token vẫn còn hiệu lực, không cần làm mới."));
                }


                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var accessTokenId = principal.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

                if (userId == null || accessTokenId == null)
                {
                    return Unauthorized(new ApiResponse("Token claims không hợp lệ"));
                }

                // Kiểm tra Refresh Token có khớp với Access Token không?
                var existingToken = await _tokenService.GetRefreshTokenAsync(request.RefreshToken);

                var (isValid, message) = IsValidRefreshToken(existingToken, accessTokenId, userId);

                if (!isValid)
                {
                    return Unauthorized(new ApiResponse(message));
                }

                string newAccessToken = "", newRefreshToken = "";

                await _transactionService.ExecuteTransactionAsync(async () =>
                {
                    // Đánh dấu Refresh Token cũ đã sử dụng
                    existingToken.IsRevoked = true;
                    existingToken.IsUsed = true;
                    await _tokenService.UpdateTokenUser(existingToken);
                    var user = await _authenticationService.GetUserById(userId);
                    if (user == null)
                    {
                        throw new Exception("Người dùng không tồn tại");
                    }
                    var loginResponse = new LoginResponse
                    {
                        UserId = user.Id,
                        Username = user.UserName,
                        Email = user.Email,
                        FullName = user.FullName
                    };

                    //newAccessToken = _tokenService.GenerateToken(loginResponse, out string newAccessTokenId);
                    (string token, string newAccessTokenId) = await _tokenService.GenerateToken(loginResponse);
                    newRefreshToken = _tokenService.GenerateRefreshToken();

                    await _tokenService.SaveRefreshTokenAsync(user.Id, newRefreshToken, newAccessTokenId);

                });

                return Ok(new ApiResponse("Refresh Token thành công", new
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi làm mới token");
                // Trả về lỗi chung để tránh lộ thông tin
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau"));
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request)
        {
            try
            {
                // Kiểm tra Refresh Token có tồn tại không?
                var existingToken = await _tokenService.GetRefreshTokenAsync(request.RefreshToken);
                if (existingToken == null)
                {
                    return Unauthorized(new ApiResponse("Refresh token không hợp lệ."));
                }

                await _transactionService.ExecuteTransactionAsync(async () =>
                {
                    existingToken.IsRevoked = true; // thu hồi token
                    await _tokenService.UpdateTokenUser(existingToken);
                });

                return Ok(new ApiResponse("Đăng xuất thành công."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when register");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));
            }

        }


        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("lock-account")] // ban vĩnh viễn
        public async Task<IActionResult> LockAccount([FromBody] LockAccountRequest model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Email))
                {
                    return BadRequest(new ApiResponse("Email không hợp lệ."));
                }

                var user = _authenticationService.GetUserByMail(model.Email);
                if (user == null)
                {
                    return NotFound(new ApiResponse("Không tìm thấy tài khoản."));
                }

                if (user.Result.UserName.ToUpper().Equals("ADMIN"))
                {
                    return BadRequest(new ApiResponse("Không thể khoá tài khoản admin."));
                }


                await _authenticationService.LockAccountAsync(user.Result);
                return Ok(new ApiResponse("Khoá tài khoản thành công."));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi khóa tài khoản");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("unlock-account")] // mở tài khoản bị ban
        public async Task<IActionResult> UnLockAccount([FromBody] LockAccountRequest model)
        {
            try
            {

                if (string.IsNullOrEmpty(model.Email))
                {
                    return BadRequest(new ApiResponse("Email không hợp lệ."));
                }

                var user = _authenticationService.GetUserByMail(model.Email);
                if (user == null)
                {
                    return NotFound(new ApiResponse("Không tìm thấy tài khoản."));
                }

                await _authenticationService.UnLockAccountAsync(user.Result);
                return Ok(new ApiResponse("Mở khoá tài khoản thành công."));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi mở khóa tài khoản");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));
            }
        }


        [HttpPut("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null) return Unauthorized();

                var user = await _authenticationService.GetUserById(userId);
                if (user == null)
                {
                    return NotFound(new ApiResponse("Không tìm thấy tài khoản."));
                }

                if (request.NewPassword != request.ConfirmPassword)
                {
                    return BadRequest(new ApiResponse("Mật khẩu xác nhận không khớp"));
                }

                var isPasswordValid = await _authenticationService.CheckAccountValid(new LoginRequest
                {
                    Username = user.UserName,
                    Password = request.CurrentPassword
                });

                if (!isPasswordValid)
                {
                    return BadRequest(new ApiResponse("Mật khẩu cũ không đúng."));
                }

                await _transactionService.ExecuteTransactionAsync(async () =>
                {
                    var isChanged = await _authenticationService.ChangePassword(user, request);
                    if (!isChanged)
                    {
                        throw new Exception("Đổi mật khẩu không thành công.");
                    }

                    // Thu hồi tất cả Refresh Token khi đổi mật khẩu
                    await _tokenService.RevokeAllRefreshTokenAsync(userId);

                });

                return Ok(new ApiResponse("Đổi mật khẩu thành công."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi đổi mật khẩu");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassowrd([FromBody] ResetPasswordRequest model)
        {
            try
            {
                var user = await _authenticationService.GetUserByMail(model.Email);

                if (user == null)
                {
                    return NotFound(new ApiResponse("Không tìm thấy tài khoản."));
                }

                var (isValid, message) = _otpService.ValidateOtpAsync(user, model.Otp);
                if (!isValid)
                {
                    return BadRequest(new ApiResponse(message));
                }

                await _transactionService.ExecuteTransactionAsync(async () =>
                  {
                      var isReset = await _authenticationService.ResetPassword(user, model.NewPassword);
                      if (!isReset)
                      {
                          throw new Exception("Đặt lại mật khẩu thất bại.");
                      }

                      // Xóa OTP sau khi đặt lại mật khẩu thành công
                      user.Otp = null;
                      user.OtpExpiryDate = null;
                      await _authenticationService.UpdateUser(user);
                  });

                return Ok(new ApiResponse("Đặt lại mật khẩu thành công."));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi đặt lại mật khẩu");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            try
            {
                // Kiểm tra dữ liệu đăng ký
                var (isValid, message) = await IsValidUserRegister(model);
                if (!isValid)
                {
                    return BadRequest(new ApiResponse(message));
                }

                // Tạo mã OTP và mã hóa nó
                var otp = _otpService.CreateOtp();
                var otphash = _otpService.HashOtpModel(otp);

                await _transactionService.ExecuteTransactionAsync(async () =>
                {
                    var user = await _authenticationService.RegisterAsync(model, otphash);
                    // Gán vai trò Student
                    await _authorizationServices.AssignRoleAsync(user, RoleUser.Student);

                    var student = new Student
                    {
                        StudentId = Guid.NewGuid(),
                        UserId = user.Id,
                        CreatedAt = DateTime.UtcNow,
                    };

                    await _studentService.AddStudent(student);
                    // Gửi email OTP
                    var isSuccess = await SendMailOtp(model.Email, otp.OtpCode);
                    if (!isSuccess)
                    {
                        throw new Exception("Không thể gửi email xác thực.");
                    }
                });
                return Ok(new ApiResponse("Đăng ký tài khoản thành công, vui lòng kiểm tra email để xác thực."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi đăng ký tài khoản");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));
            }
        }

        [HttpPost("create-account-employees")]
        [Authorize(Policy = "CanCreateAccount")]
        public async Task<IActionResult> CreateAccount([FromBody] RegisterRequest model)
        {
            try
            {

                // Kiểm tra dữ liệu đăng ký
                var (isValid, message) = await IsValidUserRegister(model);
                if (!isValid)
                {
                    return BadRequest(new ApiResponse(message));
                }

                // Gán vai trò Employee
                await _transactionService.ExecuteTransactionAsync(async () =>
                {
                    var user = await _authenticationService.RegisterAsync(model, null);
                    // Gán vai trò Employee
                    await _authorizationServices.AssignRoleAsync(user, RoleUser.Employee);
                });

                return Ok(new ApiResponse("Tài khoản nhân viên được tạo thành công, vui lòng kiểm tra email để kích hoạt tài khoản."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo tài khoản nhân viên");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));
            }
        }


        [HttpPost("send-otp")] // use confirm and reset password
        public async Task<IActionResult> SendOtp([FromBody] SendOtpRequest model)
        {
            try
            {
                var user = await _authenticationService.GetUserByMail(model.Email);
                if (user == null)
                {
                    return BadRequest(new ApiResponse("Email không tồn tại"));
                }

                // Kiểm tra nếu OTP vừa được gửi trong 60 giây

                if (user.LastOtpSentTime.HasValue && (DateTime.UtcNow - user.LastOtpSentTime.Value).TotalSeconds < 60)
                {
                    return BadRequest(new ApiResponse("Vui lòng chờ 60 giây trước khi gửi lại OTP"));
                }

                await _transactionService.ExecuteTransactionAsync(async () =>
                  {
                      var otp = _otpService.CreateOtp();
                      user.Otp = _otpService.HashOtp(otp.OtpCode);
                      user.OtpExpiryDate = otp.ExpiryTime;
                      user.LastOtpSentTime = DateTime.UtcNow;// Cập nhật thời gian gửi OTP

                      await _authenticationService.UpdateUser(user);

                      var isSuccess = await SendMailOtp(model.Email, otp.OtpCode);

                      if (!isSuccess)
                      {
                          throw new Exception("Không thể gửi email xác thực.");
                      }
                  });

                return Ok(new ApiResponse("Gửi mã OTP thành công"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi gửi OTP");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau"));
            }
        }


        [HttpPost("verify-otp")] // use forgot password and confirm mail
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto model)
        {
            try
            {
                var user = await _authenticationService.GetUserByMail(model.Email);

                if (user == null)
                {
                    return BadRequest(new ApiResponse("Email không tồn tại"));
                }

                var (isValid, message) = _otpService.ValidateOtpAsync(user, model.Otp);
                if (!isValid)
                {
                    return BadRequest(new ApiResponse(message));
                }
                return Ok(new ApiResponse("OTP hợp lệ, tiếp tục đặt lại mật khẩu."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xác minh OTP");
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi hệ thống, vui lòng thử lại sau");
            }

        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest model)
        {
            try
            {
                var user = await _authenticationService.GetUserByMail(model.Email);

                if (user == null)
                {
                    return BadRequest(new ApiResponse("Email không tồn tại"));
                }

                var (isValid, message) = _otpService.ValidateOtpAsync(user, model.Otp);

                if (!isValid)
                {
                    return BadRequest(new ApiResponse(message));
                }
                await _transactionService.ExecuteTransactionAsync(async () =>
                {
                    user.EmailConfirmed = true;
                    user.Otp = null;
                    user.OtpExpiryDate = null;
                    await _authenticationService.UpdateUser(user);
                });



                return Ok(new ApiResponse("Xác thực email thành công"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xác nhận email");
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi hệ thống, vui lòng thử lại sau");

            }
        }


        // handel send mail otp
        private async Task<bool> SendMailOtp(string mail, string otp, string subject = "Xác thực tài khoản", string template = "email_verify_otp")
        {
            EmailRequest request = new EmailRequest
            {
                ToEmail = mail,
                Subject = subject,
                TemplateName = template,
                Placeholders = new Dictionary<string, string>
                {
                        { "OTP_CODE", otp },
                    }
            };

            var isSuccess = await _emailService.SendEmailAsync(request);
            return isSuccess;
        }
    }
}
