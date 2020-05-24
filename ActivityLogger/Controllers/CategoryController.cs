using ActivityLogger.Dtos;
using ActivityLogger.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActivityLogger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAvailableCategories()
        {
            var result = await _categoryService.GetAvailableCategoriesAsync();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}