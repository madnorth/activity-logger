using ActivityLogger.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ActivityLogger.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryListDto>> GetSelectableCategoriesAsync(CancellationToken ct = default);
    }
}
