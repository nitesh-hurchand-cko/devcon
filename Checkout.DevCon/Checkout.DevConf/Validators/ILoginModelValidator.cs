﻿using Checkout.DevCon.Models;
using FluentValidation;

namespace Checkout.DevCon.Validators
{
    public interface ILoginModelValidator : IValidator<LoginModel>
    {
    }
}