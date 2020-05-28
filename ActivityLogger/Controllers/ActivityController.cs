using ActivityLogger.Dtos;
using ActivityLogger.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ActivityLogger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IValidator<ActivityCreateDto> _validator;
        private readonly IActivityService _activityService;

        public ActivityController(IValidator<ActivityCreateDto> validator, IActivityService activityService)
        {
            _validator = validator;
            _activityService = activityService;
        }

        [HttpPost]
        public async Task<ActionResult<ActivityDto>> Create([FromBody] ActivityCreateDto activity)
        {
            await _validator.ValidateAndThrowAsync(activity);
            var result = await _activityService.CreateActivityLogAsync(activity);

            return Ok(result);
        }

        [HttpGet("report")]
        public async Task<ActionResult> Report([FromQuery] DateTime date)
        {
            var result = await _activityService.GetDailyReportAsync(date);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(
                new
                {
                    QueriedDate = date,
                    Data = result
                });
        }
    }
}