using Entities.Concrete;
using FluentValidation;
using System.Globalization;

namespace Business.ValidationRules.FluenValidation.TestValidators
{
    public class TestValidator : BaseAbstractValidator<Test>
    {
        public TestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("NameIsRequired")
                .NotNull().WithMessage("NameIsRequired");
        }
    }
}
