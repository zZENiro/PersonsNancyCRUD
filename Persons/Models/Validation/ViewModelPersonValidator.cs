using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persons.Models.Validation
{
    public class ViewModelPersonValidator : AbstractValidator<ViewModelPerson>
    {
        public ViewModelPersonValidator()
        {
            RuleFor(req => req.Name).NotNull().WithMessage("Name is required");
            RuleFor(req => req.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(req => req.Name).Length(3, 20).WithMessage("Name too short/big");
            RuleFor(req => req.Name).Must(name => !name.Any(char.IsDigit)).WithMessage("Name contains numbers");

            RuleFor(req => req.Birthday).NotNull().WithMessage("Birthday date is required");
            RuleFor(req => req.Birthday).NotEmpty().WithMessage("Birthday date is required");
            RuleFor(req => req.Birthday).Must(birthday => { DateTime date; return DateTime.TryParse(birthday, out date); })
                .WithMessage("YYYY-MM-DD HH:MM:SS.SSS type is required");
        }
    }
}
