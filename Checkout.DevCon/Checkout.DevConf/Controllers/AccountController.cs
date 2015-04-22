using System;
using System.Linq;
using System.Web.Http;
using Checkout.DevCon.Models;
using Checkout.DevCon.Validators;

namespace Checkout.DevCon.Controllers
{
    [RoutePrefix("account")]
    public class AccountController : ApiController
    {
        [Route("login")]
        public object Post([FromBody] LoginModel loginModel)
        {
            if (loginModel == null)
                return "Required fields missing.";

            var validator = new LoginModelValidator();
            var validationResults = validator.Validate(loginModel);

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
