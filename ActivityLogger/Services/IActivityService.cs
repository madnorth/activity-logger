using ActivityLogger.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ActivityLogger.Services
{
    public interface IActivityService
    {
        Task<ActivityDto> CreateActivityLogAsync(ActivityCreateDto activity, CancellationToken ct = default);
        Task<IEnumerable<ReportItemDto>> GetDailyReportAsync(DateTime dateTime, CancellationToken ct = default);
    }
}