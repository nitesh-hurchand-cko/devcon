using Checkout.DevCon.Models;
using FluentValidation;

namespace Checkout.DevCon.Validators
{
    public interface IUserModelValidator : IValidator<CreateUserModel>
    {
    }
}
