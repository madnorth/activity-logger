using ActivityLogger.Data;
using ActivityLogger.Data.Models;
using ActivityLogger.Dtos;
using ActivityLogger.Infrastructure.Exceptions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ActivityLogger.Services
{
    public class ActivityService : IActivityService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ActivityService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ActivityDto> CreateActivityLogAsync(ActivityCreateDto activity, CancellationToken ct = default)
        {
            var newActivity = new Activity
            {
                CategoryId = activity.CategoryId,
                StartDate = ParseDateTimeWithoutSeconds(activity.StartDateTime),
                EndDate = ParseDateTimeWithoutSeconds(activity.EndDateTime),
                Comment = activity.Comment
            };

            if (!await IsValidIntervalAsync(newActivity.StartDate, newActivity.EndDate, ct))
            {
                throw new BusinessLogicException("Invalid activity interval.");
            }

            await _context.Activities.AddAsync(newActivity, ct);
            await _context.SaveChangesAsync(ct);
            _context.Entry(newActivity.Category).Reference(c => c.Parent).Load();

            return _mapper.Map<Activity, ActivityDto>(newActivity);
        }

        public async Task<IEnumerable<ReportItemDto>> GetDailyReportAsync(DateTime reportDate, CancellationToken ct = default)
        {
            var queryResult = await _context.Activities
                .Include(a => a.Category)
                .ThenInclude(c => c.Parent)
                .Where(a => a.StartDate.Date == reportDate.Date || a.EndDate.Date == reportDate.Date)
                .ToListAsync();

            var result = queryResult
                .Select(a => new
                {
                    MainCategoryId = a.Category.Parent == null ? a.CategoryId : a.Category.Parent.Id,
                    MainCategory = a.Category.Parent == null ? a.Category.Name : a.Category.Parent.Name,
                    Duration = CalculateDurationHour(a.StartDate, a.EndDate, reportDate),
                    a.Comment
                })
                .GroupBy(
                    x => x.MainCategoryId,
                    x => x,
                    (key, g) => new ReportItemDto
                    {
                        CategoryName = g.First().MainCategory,
                        Duration = g.Sum(d => d.Duration),
                        Comments = g.Select(d => d.Comment).ToList()
                    })
                .OrderBy(r => r.CategoryName)
                .ToList();

            return result;
        }

        private double CalculateDurationHour(DateTime startDate, DateTime endDate, DateTime reportDate)
        {
            var year = reportDate.Year;
            var month = reportDate.Month;
            var day = reportDate.Day;

            var tempStartDate = startDate.Date != endDate.Date && endDate.Date == reportDate.Date
                                ?
                                new DateTime(year, month, day, 0, 0, 0, startDate.Kind)
                                :
                                startDate;

            var tempEndDate = startDate.Date != endDate.Date && startDate.Date == reportDate.Date
                                ?
                                new DateTime(year, month, day + 1, 0, 0, 0, endDate.Kind)
                                :
                                endDate;

            return (tempEndDate - tempStartDate).TotalHours;
        }

        private async Task<bool> IsValidIntervalAsync(DateTime startDateTime, DateTime endDateTime, CancellationToken ct)
        {
            if (startDateTime >= endDateTime)
            {
                return false;
            }

            var result = await _context.Activities
                .Where(a =>
                    (a.StartDate == startDateTime && a.EndDate == endDateTime)
                    ||
                    (a.StartDate > startDateTime && a.StartDate < endDateTime)
                    ||
                    (a.EndDate > startDateTime && a.EndDate < endDateTime)
                )
                .FirstOrDefaultAsync(ct);

            return result == null;
        }

        private DateTime ParseDateTimeWithoutSeconds(string dateTime)
        {
            var tempDateTime = DateTime.Parse(dateTime);

            return new DateTime(
                tempDateTime.Year,
                tempDateTime.Month,
                tempDateTime.Day,
                tempDateTime.Hour,
                tempDateTime.Minute,
                0,
                tempDateTime.Kind);
        }
    }
}