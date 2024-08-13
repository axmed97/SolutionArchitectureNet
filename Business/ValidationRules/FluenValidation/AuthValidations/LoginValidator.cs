using Entities.DTOs.AuthDTOs;
using FluentValidation;
using System.Globalization;

namespace Business.ValidationRules.FluenValidation.AuthValidations
{
    public class LoginValidator : BaseAbstractValidator<LoginDTO>
    {
        public LoginValidator()
        {
            RuleFor(x => x.EmailOrUsername)
                .NotEmpty().WithMessage(GetTranslation("EmailIsRequired"))
                .NotNull().WithMessage(GetTranslation("EmailIsRequired"));
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(GetTranslation("PasswordIsRequired"))
                .NotNull().WithMessage(GetTranslation("PasswordIsRequired"));
        }
    }
}
