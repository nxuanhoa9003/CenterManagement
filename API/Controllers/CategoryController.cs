using Application.InterfacesServices;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.DTOs.Category;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/category")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;
        private readonly ITransactionService _transactionService;

        public CategoryController(ILogger<CategoryController> logger, IMapper mapper, ICategoryService categoryService, ITransactionService transactionService)
        {
            _logger = logger;
            _mapper = mapper;
            _categoryService = categoryService;
            _transactionService = transactionService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await _categoryService.GetCategories();
                return categories.Any()
                    ? Ok(new ApiResponse { Message = "Danh sách danh mục", Data = categories })
                    : NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while getting all categories");
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = "An error occurred while processing your request" });
            }
        }


        [HttpGet("by-name")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategoryByName([FromQuery] string? name)
        {
            try
            {
                
                var category = await _categoryService.GetCategoryByName(name ?? "");
                return category != null
                    ? Ok(new ApiResponse { Message = "Lấy dữ liệu thành công", Data = category })
                    : NotFound(new ApiResponse { Message = "Không tìm thấy danh mục" });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while getting category by id");
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = "An error occurred while processing your request" });
            }
        }


        [HttpPost]
        [Authorize(Policy = "CanAddCategory")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryRequest request)
        {

            if (string.IsNullOrEmpty(request.Name))
            {
                return BadRequest(new ApiResponse("Tên danh mục không hợp lệ"));
            }

            if (request.Id != Guid.Empty)
            {
                var isExist = await _categoryService.CategoryExists(request.Id);
                if (isExist)
                {
                    return Ok(new ApiResponse("Danh mục đã tồn tại."));
                }
            }

            try
            {
                var categoryEntity = _mapper.Map<Domain.Entities.Category>(request);
                await _transactionService.ExecuteTransactionAsync(async () => { await _categoryService.AddCategory(categoryEntity); });
                return Ok(new ApiResponse { Message = "Thêm danh mục thành công" });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm danh mục");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));
            }
        }

        [HttpPut]
        [Authorize(Policy = "CanUpdateCategory")]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryRequest request)
        {

            if (request.Id == Guid.Empty) // Kiểm tra Id hợp lệ
            {
                return BadRequest(new ApiResponse("Id danh mục không hợp lệ"));
            }


            var existingCategory = await _categoryService.GetCategoryById(request.Id);
            if (existingCategory == null)
            {
                return NotFound(new ApiResponse("Không tìm thấy danh mục"));
            }

            // Kiểm tra nếu tên mới đã tồn tại trong DB (trừ trường hợp giữ nguyên)
            if (!string.IsNullOrWhiteSpace(request.Name) && request.Name != existingCategory.Name)
            {
                var isNameExist = await _categoryService.CategoryNameExistsAsync(request.Name);
                if (isNameExist)
                {
                    return BadRequest(new ApiResponse("Tên danh mục đã tồn tại\""));
                }
            }

            try
            {
                existingCategory.Name = request.Name ?? existingCategory.Name;
                existingCategory.Description = request.Description ?? existingCategory.Description;

                await _transactionService.ExecuteTransactionAsync(async () => { await _categoryService.UpdateCategory(existingCategory); });
                return Ok(new ApiResponse { Message = "Cập nhật danh mục thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật danh mục");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));

            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "CanDeleteCategory")]
        public async Task<IActionResult> DeleteCategory([FromRoute] string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid categoryId))
                {
                    return BadRequest(new ApiResponse { Message = "Id danh mục không hợp lệ" });
                }
                var IsExist = await _categoryService.CategoryExists(categoryId);
                
                if (!IsExist)
                {
                    return NotFound(new { status = false, message = "Danh mục không tồn tại" });
                }
                await _transactionService.ExecuteTransactionAsync(async () => { await _categoryService.DeleteCategory(categoryId); });
                return Ok(new ApiResponse { Message = "Xoá danh mục thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xoá danh mục");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse("Lỗi hệ thống, vui lòng thử lại sau."));

            }
        }

    }
}
