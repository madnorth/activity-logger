using ActivityLogger.Dtos;
using ActivityLogger.Entities;
using ActivityLogger.Entities.Models;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public CategoryService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAvailableCategoriesAsync(CancellationToken ct = default)
        {
            var result = await _context.Categories
                .Include(c => c.Parent)
                .Where(c => c.Children.Count == 0)
                .OrderBy(c => c.Name)
                .ToListAsync(ct);

            return result.Select(c => _mapper.Map<Category, CategoryDto>(c));
        }

        public async Task<bool> IsValidCategoryAsync(int categoryId, CancellationToken ct = default)
        {
            var result = await _context.Categories.Where(c => c.Id == categoryId).FirstOrDefaultAsync(ct);
            
            return result != null;
        }
    }
}