using Checkout.DevCon.Resources;
using FluentValidation;

namespace Checkout.DevCon.Validators
{
    public class AddressModelValidator : AbstractValidator<Models.AddressModel>, IAddressModelValidator
    {
        public AddressModelValidator()
        {
            RuleFor(addressModel => addressModel.Line1).NotNull().NotEmpty().WithLocalizedMessage(() => UserModelResources.AddressLine1Required);

            RuleFor(addressModel => addressModel.City).NotNull().NotEmpty().WithLocalizedMessage(() => UserModelResources.CityRequired);

            RuleFor(addressModel => addressModel.State).NotNull().NotEmpty().WithLocalizedMessage(() => UserModelResources.StateRequired);

            RuleFor(addressModel => addressModel.CountryCode).NotNull().NotEmpty().WithLocalizedMessage(() => UserModelResources.CountryRequired);
        }
    }
}