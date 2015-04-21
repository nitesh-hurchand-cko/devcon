using System;
using System.Linq;
using System.Web.Http;
using Checkout.DevCon.Validators;

namespace Checkout.DevCon.Controllers
{
    [RoutePrefix("account")]
    public class AccountController : ApiController
    {
        public class LoginDto
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        [Route("login")]
        public object Post([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
                return "Required fields missing.";

            var validator = new AccountValidator();
            var validationResults = validator.Validate(loginDto);

            if (!validationResults.IsValid)
                return validationResults.Errors.Select(x => x.ErrorMessage);

            if (loginDto == null 
                || string.IsNullOrWhiteSpace(loginDto.Username)
                || string.IsNullOrWhiteSpace(loginDto.Password))
                return new { valid = false, token = "", name = "" };

            if (loginDto.Username.Equals("admin", StringComparison.InvariantCultureIgnoreCase)
                && loginDto.Password == "admin")
            {
                return new { valid = true, token = Guid.NewGuid().ToString(), name = loginDto.Username };
            }
            return new { valid = false, token = "", name = "" };
        }
    }
}
