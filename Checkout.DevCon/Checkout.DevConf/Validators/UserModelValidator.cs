using System;
using System.Text.RegularExpressions;
using Checkout.DevCon.Models;
using Checkout.DevCon.Resources;
using FluentValidation;

namespace Checkout.DevCon.Validators
{
    public class UserModelValidator : AbstractValidator<CreateUserModel>, IUserModelValidator
    {
        private readonly IPhoneModelValidator _phoneModelValidator;
        private readonly IAddressModelValidator _addressModelValidator;

        public UserModelValidator(IPhoneModelValidator phoneModelValidator,
            IAddressModelValidator addressModelValidator)
        {
            _phoneModelValidator = phoneModelValidator;
            _addressModelValidator = addressModelValidator;

            //NotNull Validator
            RuleFor(createUserModel => createUserModel.FirstName)
                .NotNull()
                .WithLocalizedMessage(() => UserModelResources.FirstNameRequired);
            //NotEmpty Validator
            RuleFor(createUserModel => createUserModel.FirstName)
                .NotEmpty()
                .WithLocalizedMessage(() => UserModelResources.FirstNameRequired);

            //Chaining Validators for the Same Property
            RuleFor(createUserModel => createUserModel.LastName)
                .NotNull()
                .NotEmpty()
                .WithLocalizedMessage(() => UserModelResources.LastNameRequired);

            //Regular Expression Validator
            //Predicate Validator (must)
            RuleFor(createUserModel => createUserModel.Email)
                .Must(IsValidEmail)
                .WithLocalizedMessage(() => UserModelResources.EmailInvalid);

            //Email Validator
            RuleFor(createUserModel => createUserModel.Email)
                .EmailAddress()
                .WithLocalizedMessage(() => UserModelResources.EmailInvalid);

            //Length Validator
            RuleFor(createUserModel => createUserModel.Password).NotNull().WithLocalizedMessage(() => UserModelResources.PasswordRequired)
                .NotEmpty().WithMessage("Password is required.")
                .Length(1, 19).WithLocalizedMessage(() => UserModelResources.PasswordLengthInvalid);

            //Localisation
            RuleFor(createUserModel => createUserModel.ResidentialAddress)
                .NotNull()
                .WithLocalizedMessage(() => UserModelResources.ResidentialAddressErrorMessage);

            //complex properties
            RuleFor(createUserModel => createUserModel.ResidentialAddress).SetValidator(_addressModelValidator);
            RuleFor(createUserModel => createUserModel.HomePhone).SetValidator(_phoneModelValidator);
        }


        #region Private methods

        private bool IsValidEmail(string url)
        {
            if (url == null)
            {
                return true;
            }

            var regex =
                new Regex(
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            return regex.IsMatch(url);
        }

        #endregion
    }
}