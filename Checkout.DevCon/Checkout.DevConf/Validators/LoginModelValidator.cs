using Checkout.DevCon.Models;
using FluentValidation;

namespace Checkout.DevCon.Validators
{
    public class LoginModelValidator : AbstractValidator<LoginModel>, ILoginModelValidator
    {
        public LoginModelValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            //RuleFor(x => x).NotNull().WithMessage("Required fields missing.");

            RuleFor(x => x.Username)
              .NotNull().WithMessage("Username is a required field.")
              .NotEmpty().WithMessage("Username is a required field.")
              .Length(1, 19).WithMessage("Invalid length for Username.");

            RuleFor(x => x.Password)
              .NotNull().WithMessage("Password is a required field.")
              .NotEmpty().WithMessage("Password is a required field.")
              .Length(1, 19).WithMessage("Invalid length for Password.");
        }
    }
}