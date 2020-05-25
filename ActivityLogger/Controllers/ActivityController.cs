using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActivityLogger.Dtos;
using ActivityLogger.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<ActivityDto>> Create([FromBody]ActivityCreateDto activity)
        {
            await _validator.ValidateAndThrowAsync(activity);
            var result = await _activityService.CreateAsync(activity);

            return Ok(result);
        }
    }
}