using ActivityLogger.Dtos;
using ActivityLogger.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ActivityLogger.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryListDto>> GetSelectableCategoriesAsync(CancellationToken ct = default)
        {
            var result = await _context.Categories
                .Include(c => c.Parent)
                .Where(c => c.Children.Count == 0)
                .OrderBy(c => c.Name)
                .ToListAsync(ct);

            return result.Select(c => new CategoryListDto
            {
                Id = c.Id,
                Name = c.Name,
                ParentName = c.Parent?.Name
            });
        }
    }
}