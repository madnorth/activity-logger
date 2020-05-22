using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActivityLogger.Dtos;
using ActivityLogger.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<IEnumerable<CategoryListDto>>> GetAvailableCategories()
        {
            try
            {
                var result = await _categoryService.GetSelectableCategoriesAsync();
                if(result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}