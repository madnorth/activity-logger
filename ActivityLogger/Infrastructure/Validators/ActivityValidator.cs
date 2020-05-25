using ActivityLogger.Dtos;
using ActivityLogger.Services;
using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ActivityLogger.Infrastructure.Validators
{
    public class ActivityValidator : AbstractValidator<ActivityCreateDto>
    {
        private readonly ICategoryService _categoryService;

        public ActivityValidator(ICategoryService categoryService)
        {
            _categoryService = categoryService;

            RuleFor(a => a.CategoryId)
                .NotEmpty()
                .MustAsync(BeValidCategoryId).WithMessage("Not a valid Category.")
                .WithName("Category");

            RuleFor(a => a.StartDateTime)
                .NotEmpty()
                .Must(d => DateTime.TryParse(d, out DateTime result)).WithMessage("'{PropertyName}' not a valid DateTime.")
                .WithName("Start Date");

            RuleFor(a => a.EndDateTime)
                .NotEmpty()
                .Must(d => DateTime.TryParse(d, out DateTime result)).WithMessage("'{PropertyName}' not a valid DateTime.")
                .WithName("End Date");

            RuleFor(a => a.Comment)
                .NotEmpty();
        }

        private async Task<bool> BeValidCategoryId(int id, CancellationToken cancellationToken)
        {
            return await _categoryService.IsValidCategoryAsync(id, cancellationToken);
        }
    }
}