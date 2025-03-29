using Application.InterfacesServices;
using Application.Services;
using AutoMapper;
using Azure.Core;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shared.DTOs;
using Shared.DTOs.Autho;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/author")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin")]
    public class AuthorizationController : ControllerBase
    {
        private readonly ILogger<AuthorizationController> _logger;
        private readonly ITransactionService _transactionService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IAuthorizationServices _authorizationServices;
        private readonly IMapper _mapper;

        public AuthorizationController(ILogger<AuthorizationController> logger, ITransactionService transactionService, IAuthenticationService authenticationService, IAuthorizationServices authorizationServices, IMapper mapper)
        {
            _logger = logger;
            _transactionService = transactionService;
            _authenticationService = authenticationService;
            _authorizationServices = authorizationServices;
            _mapper = mapper;
        }

        [Authorize(Policy = "CanCreateRole")]
        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole([FromBody] RoleRequest role)
        {
            if (string.IsNullOrEmpty(role.rolename))
            {
                return BadRequest(new ApiResponse("Role không hợp lệ"));
            }

            try
            {
                var isSuccess = await _authorizationServices.AddRoleAsync(role.rolename);
                if (!isSuccess)
                {
                    return BadRequest(new ApiResponse("Tạo role thất bại"));
                }
                return Ok(new ApiResponse("Tạo role thành công", role));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo role");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau"));
            }
        }

        [Authorize(Policy = "CanUpdateRole")]
        [HttpPut("update-role")]
        public async Task<IActionResult> UpdateRole([FromBody] RoleUpdateRequest role)
        {
            if (string.IsNullOrEmpty(role.oldRolename) || string.IsNullOrEmpty(role.newRolename))
            {
                return BadRequest(new ApiResponse("Role không hợp lệ"));
            }

            try
            {
                var isSuccess = await _authorizationServices.EditRoleAsync(role.oldRolename, role.newRolename);
                if (!isSuccess)
                {
                    return BadRequest(new ApiResponse("Cập nhật role thất bại"));
                }

                return Ok(new ApiResponse("Cập nhật role thành công"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật role");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau"));
            }
        }

        [Authorize(Policy = "CanDeleteRole")]
        [HttpDelete("remove-role")]
        public async Task<IActionResult> DeleteRole([FromBody] RoleRequest role)
        {
            if (string.IsNullOrEmpty(role.rolename))
            {
                return BadRequest(new ApiResponse("Role không hợp lệ"));
            }

            try
            {
                await _transactionService.ExecuteTransactionAsync(async () =>
                {
                    await _authorizationServices.DeleteRoleAsync(role.rolename);

                });

                return Ok(new ApiResponse("Xóa role thành công", role));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa role");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau"));
            }

        }

        [Authorize(Policy = "CanAssignRoleToUser")]
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRoleToUserAsync([FromBody] RoleToUserRequest request)
        {
            try
            {
                var user = await _authenticationService.GetUserById(request.UserId);
                if (user == null)
                {
                    return NotFound(new ApiResponse("User không tồn tại"));
                }

                await _transactionService.ExecuteTransactionAsync(async () =>
                {
                    await _authorizationServices.AssignRoleAsync(user, request.RoleNames);

                });

                var listRoles = await _authorizationServices.GetUserRolesAsync(user);
                return Ok(new ApiResponse("Gán role thành công", listRoles));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi gán role");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau"));
            }
        }

        [Authorize(Policy = "CanRemoveRoleFromUser")]
        [HttpDelete("remove-role-user")]
        public async Task<IActionResult> RemoveRoleFromUserAsync([FromBody] RemoveRolesRequest request)
        {
            try
            {
                var user = await _authenticationService.GetUserById(request.UserId);
                if (user == null)
                {
                    return NotFound(new ApiResponse("User không tồn tại"));
                }

                if (user.UserName.ToUpper().Equals("ADMIN"))
                {
                    return BadRequest(new ApiResponse("Không thể xoá role tài khoản admin."));
                }

                // Lấy danh sách roles hiện tại của user
                var existingRoles = await _authorizationServices.GetUserRolesAsync(user);
                // Chỉ xóa những roles user đang có
                var rolesToRemove = request.RoleNames.Intersect(existingRoles).ToList();
                if (!rolesToRemove.Any())
                {
                    return BadRequest(new ApiResponse("User không có các role này"));
                }

                await _transactionService.ExecuteTransactionAsync(async () =>
                {
                    await _authorizationServices.RemoveRoleFromUserAsync(user, rolesToRemove);
                });

                var updatedRoles = await _authorizationServices.GetUserRolesAsync(user);
                return Ok(new ApiResponse("Xóa role thành công", updatedRoles));

            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa role của user");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau"));
            }
        }


        [Authorize(Policy = "CanAddClaimToUser")]
        [HttpPost("assign-claim")]
        public async Task<IActionResult> AddClaimToUserAsync([FromBody] AssignClaimsRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrWhiteSpace(request.UserId) || request.Claims == null || !request.Claims.Any())
                {
                    return BadRequest(new ApiResponse("Dữ liệu không hợp lệ"));
                }

                var user = await _authenticationService.GetUserById(request.UserId);

                if (user == null)
                {
                    return BadRequest(new ApiResponse("User không tồn tại"));
                }

                // Lọc bỏ những claims không hợp lệ (trống hoặc null)
                var claims = request.Claims
                    .Where(c => !string.IsNullOrWhiteSpace(c.Type) && !string.IsNullOrWhiteSpace(c.Value))
                    .Select(c => new Claim(c.Type, c.Value))
                    .ToList();
                // Kiểm tra xem user đã có claim nào rồi
                var existingClaims = await _authorizationServices.GetUserClaimsAsync(user);
                var newClaims = claims.Where(c => !existingClaims.Any(ec => ec.Type == c.Type && ec.Value == c.Value)).ToList();

                if (!newClaims.Any())
                {
                    return BadRequest(new ApiResponse("User đã có các claims này"));
                }

                var result = await _authorizationServices.AddClaimToUserAsync(user, newClaims);
                return result ? Ok(new ApiResponse("Gán claims thành công")) : BadRequest(new ApiResponse("Gán claim thất bại"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi gán claims cho user");
                return StatusCode(StatusCodes.Status500InternalServerError, "Có lỗi xảy ra");
            }
        }


        [Authorize(Policy = "CanRemoveClaimFromUser")]
        [HttpDelete("remove-claims")]
        public async Task<IActionResult> RemoveClaimsFromUserAsync([FromBody] RemoveClaimsRequest request)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào
                if (request == null || string.IsNullOrWhiteSpace(request.UserId) || request.Claims == null || !request.Claims.Any())
                {
                    return BadRequest(new ApiResponse("Dữ liệu không hợp lệ"));
                }

                var user = await _authenticationService.GetUserById(request.UserId);
                if (user == null)
                {
                    return BadRequest(new ApiResponse("User không tồn tại"));
                }

                if (user.UserName.ToUpper().Equals("ADMIN"))
                {
                    return BadRequest(new ApiResponse("Không thể xoá claims của tài khoản admin."));
                }

                var claims = request.Claims
                    .Where(c => !string.IsNullOrWhiteSpace(c.Type) && !string.IsNullOrWhiteSpace(c.Value))
                    .Select(c => new Claim(c.Type, c.Value))
                    .ToList();

                if (!claims.Any())
                {
                    return BadRequest(new ApiResponse("Không có claim hợp lệ để xóa"));
                }

                // Kiểm tra xem user có các claims cần xóa không
                var existingClaims = await _authorizationServices.GetUserClaimsAsync(user);
                var claimsToRemove = claims.Where(c => existingClaims.Any(ec => ec.Type == c.Type && ec.Value == c.Value)).ToList();
                if (!claimsToRemove.Any())
                {
                    return BadRequest(new ApiResponse("User không có các claims này"));
                }
                var result = await _authorizationServices.RemoveClaimFromUserAsync(user, claimsToRemove);
                return result ? Ok(new ApiResponse("Xóa claims thành công")) : BadRequest(new ApiResponse("Xóa claims thất bại"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa claims của user");
                return StatusCode(StatusCodes.Status500InternalServerError, "Có lỗi xảy ra");
            }
        }

        [Authorize(Policy = "CanAddClaimToRole")]
        [HttpPost("add-claim-role")]
        public async Task<IActionResult> AddClaimToRoleAsync([FromBody] ClaimToRoleRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.RoleName) || request.Claims == null || !request.Claims.Any())
                {
                    return BadRequest(new ApiResponse("Dữ liệu không hợp lệ"));
                }

                //Chuyển đổi danh sách ClaimDto thành Claim
                var claims = request.Claims.Select(c => new Claim(c.Type, c.Value)).ToList();

                await _transactionService.ExecuteTransactionAsync(async () =>
                {
                    await _authorizationServices.AddClaimToRoleAsync(request.RoleName, claims);
                });

                return Ok(new ApiResponse("Thêm claim vào role thành công"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm claims vào role");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Có lỗi xảy ra"));
            }
        }

        [Authorize(Policy = "CanRemoveClaimToRole")]
        [HttpDelete("remove-claim-role")]
        public async Task<IActionResult> RemoveClaimToRoleAsync([FromBody] ClaimToRoleRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.RoleName) || request.Claims == null || !request.Claims.Any())
                {
                    return BadRequest(new ApiResponse("Dữ liệu không hợp lệ"));
                }

                if (request.RoleName.ToUpper().Equals(RoleUser.SuperAdmin.ToUpper()))
                {
                    return BadRequest(new ApiResponse("Không thể xoá claim của role superadmin"));
                }

                var roleExists = await _authorizationServices.RoleExistsAsync(request.RoleName);
                if (!roleExists)
                {
                    return BadRequest(new ApiResponse("Role không tồn tại"));
                }

                //Chuyển đổi danh sách ClaimDto thành Claim
                var claims = request.Claims.Select(c => new Claim(c.Type, c.Value)).ToList();

                await _transactionService.ExecuteTransactionAsync(async () =>
                {
                    await _authorizationServices.RemoveClaimFromRoleAsync(request.RoleName, claims);
                });
                return Ok(new ApiResponse("Xóa claim thành công"));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa claims khỏi role");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Có lỗi xảy ra"));
            }

        }

    }
}
