﻿using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using Checkout.DevCon.Models;
using Checkout.DevCon.Validators;
using Newtonsoft.Json;

namespace Checkout.DevCon.Controllers
{
    [RoutePrefix("account")]
    public class AccountController : ApiController
    {
        private readonly ILoginModelValidator _loginModelValidator;

        public AccountController(ILoginModelValidator loginModelValidator)
        {
            _loginModelValidator = loginModelValidator;
        }

        [Route("jsonp/login/")]
        public object Get()
        {
            if (Request.RequestUri == null)
                return "Required fields missing.";

            var decodedUrl = HttpUtility.UrlDecode(Request.RequestUri.Query);
            if (decodedUrl == null)
                return "Required fields missing.";

            var paramaters = decodedUrl.Split('&');
            if (paramaters.Length <= 1 || !paramaters[1].StartsWith("jsonp="))
                 return "Required fields missing.";

            var loginModel = JsonConvert.DeserializeObject<LoginModel>(paramaters[1].Split('=')[1]);

            if (loginModel == null)
                return "Required fields missing.";

            var validationResults = _loginModelValidator.Validate(loginModel);

            if (!validationResults.IsValid)
                return validationResults.Errors.Select(x => x.ErrorMessage);

            if (string.IsNullOrWhiteSpace(loginModel.Username)
                || string.IsNullOrWhiteSpace(loginModel.Password))
                return new { valid = false, token = "", name = "" };

            if (loginModel.Username.Equals("admin", StringComparison.InvariantCultureIgnoreCase)
                && loginModel.Password == "admin")
            {
                return new { valid = true, token = Guid.NewGuid().ToString(), name = loginModel.Username };
            }
            return new { valid = false, token = "", name = "" };
        }

        [Route("login")]
        public object Post([FromBody] LoginModel loginModel)
        {
            if (loginModel == null)
                return "Required fields missing.";

            var validationResults = _loginModelValidator.Validate(loginModel);

            if (!validationResults.IsValid)
                return validationResults.Errors.Select(x => x.ErrorMessage);

            if (string.IsNullOrWhiteSpace(loginModel.Username)
                || string.IsNullOrWhiteSpace(loginModel.Password))
                return new { valid = false, token = "", name = "" };

            if (loginModel.Username.Equals("admin", StringComparison.InvariantCultureIgnoreCase)
                && loginModel.Password == "admin")
            {
                return new { valid = true, token = Guid.NewGuid().ToString(), name = loginModel.Username };
            }
            return new { valid = false, token = "", name = "" };
        }
    }
}
