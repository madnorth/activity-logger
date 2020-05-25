using ActivityLogger.Dtos;
using ActivityLogger.Entities;
using ActivityLogger.Entities.Models;
using ActivityLogger.Infrastructure.Exceptions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task<ActivityDto> CreateAsync(ActivityCreateDto activity, CancellationToken ct = default)
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