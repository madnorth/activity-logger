using ActivityLogger.Dtos;
using ActivityLogger.Entities;
using ActivityLogger.Entities.Models;
using AutoMapper;
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

        public async Task<ActivityDto> CreateAsync(ActivityCreateDto activity, CancellationToken ct = default)
        {
            var newActivity = new Activity
            {
                CategoryId = activity.CategoryId,
                StartDate = DateTime.Parse(activity.StartDateTime),
                EndDate = DateTime.Parse(activity.EndDateTime),
                Comment = activity.Comment
            };

            await _context.Activities.AddAsync(newActivity, ct);
            await _context.SaveChangesAsync(ct);
            _context.Entry(newActivity.Category).Reference(c => c.Parent).Load();

            return _mapper.Map<Activity, ActivityDto>(newActivity);
        }
    }
}
