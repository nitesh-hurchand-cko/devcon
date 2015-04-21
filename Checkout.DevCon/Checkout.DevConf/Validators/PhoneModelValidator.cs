using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Checkout.DevCon.Resources;
using FluentValidation;

namespace Checkout.DevCon.Validators
{
    public class PhoneModelValidator : AbstractValidator<Models.PhoneModel>
    {
        public PhoneModelValidator()
        {
            //number cannot be null when country has been entered
            RuleFor(phoneModel => phoneModel.Number)
                .NotNull()
                .When(phoneModel => !string.IsNullOrEmpty(phoneModel.CountryCode))
                .WithLocalizedMessage(() => UserModelResources.PhoneNumberRequired);
        }
    }
}