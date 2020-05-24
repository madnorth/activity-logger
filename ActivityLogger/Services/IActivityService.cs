using ActivityLogger.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace ActivityLogger.Services
{
    public interface IActivityService
    {
        Task<ActivityDto> CreateAsync(ActivityCreateDto activity, CancellationToken ct = default);
    }
}