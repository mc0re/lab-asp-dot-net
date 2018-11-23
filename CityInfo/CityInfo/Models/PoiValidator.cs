using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.Models
{
    public class PoiValidator : AbstractValidator<PoiUpdateDto>
    {
        public PoiValidator()
        {
            RuleFor(p => p).NotNull();
            RuleFor(p => p.Name).NotEmpty().MaximumLength(Constants.MaxNameLength);
            RuleFor(p => p.Description).NotEqual(p => p.Name).MaximumLength(Constants.MaxDescriptionLength);
        }
    }
}
