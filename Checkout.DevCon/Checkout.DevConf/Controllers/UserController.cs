using System.Globalization;
using System.Linq;
using System.Threading;
using Checkout.DevCon.Models;
using Checkout.DevCon.Validators;
using FluentValidation.Results;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Checkout.DevCon.Controllers
{
    //[RoutePrefix("users")]
    public class UserController : ApiController
    {
        private readonly IUserModelValidator _userModelValidator;

        public UserController(IUserModelValidator userModelValidator)
        {
            _userModelValidator = userModelValidator;
        }

        [Route("users")]
        public object Post(CreateUserModel model)
        {
            var browserLocale = "en";
            try
            {
                browserLocale = Request.Headers.GetValues("Locale").FirstOrDefault();
            }
            catch
            {
            }


            if (!string.IsNullOrWhiteSpace(browserLocale) && browserLocale.Contains("fr"))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("fr");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr");
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }

            const string locale = "en";
            if (model == null)
                return "Required fields missing.";

            ValidationResult validationResults = _userModelValidator.Validate(model);
            if (!validationResults.IsValid)
            {
                return validationResults.Errors.Select(x => x.ErrorMessage);
            }

            var emailResult = VerifyEmail(model.Email, "ev-615b17594828aa4cafffe7eb708a4fdc");

            var residentialAddressResult = VerifyAddress(model.ResidentialAddress, locale, "av-615b17594828aa4cafffe7eb708a4fdc");
            var workAddressResult = VerifyAddress(model.WorkAddress, locale, "av-615b17594828aa4cafffe7eb708a4fdc");

            var mobilePhoneResult = VerifyPhone(model.MobilePhone, locale, "pv-615b17594828aa4cafffe7eb708a4fdc");
            var homePhoneResult = VerifyPhone(model.HomePhone, locale, "pv-615b17594828aa4cafffe7eb708a4fdc");


            //create user if validations are successfull

            return new
            {
                firstName = model.FirstName,
                lastName = model.LastName,
                email = emailResult,
                residentialAddress = residentialAddressResult,
                workAddress = workAddressResult,
                mobilePhone = mobilePhoneResult,
                homePhone = homePhoneResult,
            };
        }

        [Route("users2")]
        public object Post2(CreateUserModel model)
        {
            const string locale = "en";
            if (model == null)
                return "Required fields missing.";

            ValidationResult validationResults = _userModelValidator.Validate(model);
            if (!validationResults.IsValid)
            {
                return validationResults.Errors.Select(x => x.ErrorMessage);
            }

            var task1 = Task.Run(() => VerifyEmail(model.Email, "ev-615b17594828aa4cafffe7eb708a4fdc"));
            var task2 = Task.Run(() => VerifyAddress(model.ResidentialAddress, locale, "av-615b17594828aa4cafffe7eb708a4fdc"));
            var task3 = Task.Run(() => VerifyAddress(model.WorkAddress, locale, "av-615b17594828aa4cafffe7eb708a4fdc"));
            var task4 = Task.Run(() => VerifyPhone(model.MobilePhone, locale, "pv-615b17594828aa4cafffe7eb708a4fdc"));
            var task5 = Task.Run(() => VerifyPhone(model.HomePhone, locale, "pv-615b17594828aa4cafffe7eb708a4fdc"));

            Task.WaitAll();
            var emailResult = task1.Result;
            var residentialAddressResult = task2.Result;
            var workAddressResult = task3.Result;
            var mobilePhoneResult = task4.Result;
            var homePhoneResult = task5.Result;
            //create user if validations are successfull

            return new
            {
                firstName = model.FirstName,
                lastName = model.LastName,
                email = emailResult,
                residentialAddress = residentialAddressResult,
                workAddress = workAddressResult,
                mobilePhone = mobilePhoneResult,
                homePhone = homePhoneResult,
            };
        }

        public EmailResult VerifyEmail(string email, string apiKey)
        {
            try
            {

                const String apiurl = "http://api1.email-validator.net/api/verify";
                var client = new HttpClient();

                var postData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("EmailAddress", email),
                    //new KeyValuePair<string, string>("APIKey", apiKey)
                };

                HttpContent content = new FormUrlEncodedContent(postData);

                var result = client.PostAsync(apiurl, content).Result;
                var resultContent = result.Content.ReadAsStringAsync().Result;

                var emailResult = JsonConvert.DeserializeObject<EmailResult>(resultContent);

                return emailResult;
            }
            catch (Exception exception)
            {
                return new EmailResult
                {

                };
            }
        }

        public class EmailResult
        {
            [JsonProperty("status")]
            public int Status { get; set; }

            [JsonProperty("info")]
            public string Info { get; set; }

            [JsonProperty("details")]
            public string Details { get; set; }
        }

        public AddressResult VerifyAddress(AddressModel addressModel, string locale, string apiKey)
        {
            try
            {
                const String apiurl = "http://api.address-validator.net/api/verify";
                var client = new HttpClient();

                var postData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("StreetAddress", addressModel.Line1),
                    new KeyValuePair<string, string>("City", addressModel.City),
                    new KeyValuePair<string, string>("PostalCode", addressModel.Zip),
                    new KeyValuePair<string, string>("State", addressModel.State),
                    new KeyValuePair<string, string>("CountryCode", addressModel.CountryCode),
                    new KeyValuePair<string, string>("Locale", locale),
                    //new KeyValuePair<string, string>("APIKey", apiKey)
                };

                if (!string.IsNullOrWhiteSpace(addressModel.Line2))
                    postData.Add(new KeyValuePair<string, string>("AdditionalAddressInfo", addressModel.Line2));

                HttpContent content = new FormUrlEncodedContent(postData);

                var result = client.PostAsync(apiurl, content).Result;
                var resultContent = result.Content.ReadAsStringAsync().Result;

                var addressResult = JsonConvert.DeserializeObject<AddressResult>(resultContent);

                return addressResult;
            }
            catch (Exception exception)
            {
                return new AddressResult
                {
                    Status = "FAIL"
                };
            }
        }

        public class AddressResult
        {
            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("streetaddress")]
            public string StreetAddress { get; set; }

            [JsonProperty("streetnumber")]
            public string StreetNumber { get; set; }

            [JsonProperty("postalcode")]
            public string PostalCode { get; set; }

            [JsonProperty("formattedaddress")]
            public string FormattedAddress { get; set; }

            [JsonProperty("city")]
            public string City { get; set; }

            [JsonProperty("country")]
            public string Country { get; set; }
        }

        public PhoneResult VerifyPhone(PhoneModel phoneModel, string locale, string apiKey)
        {
            try
            {
                const String apiurl = "http://api.phone-validator.net/api/v2/verify";
                var client = new HttpClient();

                var postData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("PhoneNumber", phoneModel.Number),
                    new KeyValuePair<string, string>("CountryCode", phoneModel.CountryCode),
                    new KeyValuePair<string, string>("Locale", locale),
                    //new KeyValuePair<string, string>("APIKey", apiKey)
                };

                HttpContent content = new FormUrlEncodedContent(postData);

                var result = client.PostAsync(apiurl, content).Result;
                var resultContent = result.Content.ReadAsStringAsync().Result;

                var phoneResult = JsonConvert.DeserializeObject<PhoneResult>(resultContent);

                return phoneResult;
            }
            catch (Exception exception)
            {
                return new PhoneResult
                {
                    Status = "FAIL"
                };
            }
        }

        public class PhoneResult
        {
            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("linetype")]
            public string LineType { get; set; }

            [JsonProperty("geolocation")]
            public string GeoLocation { get; set; }

            [JsonProperty("regioncode")]
            public string RegionCode { get; set; }

            [JsonProperty("formatnational")]
            public string FormatNational { get; set; }

            [JsonProperty("formatinternational")]
            public string FormatInternational { get; set; }
        }
    }
}
