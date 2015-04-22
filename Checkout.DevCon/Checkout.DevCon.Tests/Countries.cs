using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using TechTalk.SpecFlow;
using System.Web.Http;
namespace Checkout.DevCon.Tests
{
    [Binding]
    public class Countries
    {
      
        [When(@"I send a Get request to the countries API")]
        public void SendAGetRequestToTheCountriesAPI()
        {
            var count = ScenarioContext.Current.ContainsKey("count") ? (int?)ScenarioContext.Current["count"] : null;
            var offset = ScenarioContext.Current.ContainsKey("offset") ? (int?)ScenarioContext.Current["offset"] : null;
            IList<string> countries=null;
            //if (count != null && offset == null)
          countries= JsonConvert.DeserializeObject<IList<string>>(
              ApiHelper.ProcessRequestAndReturnResultAsString(
              (String.Format("{0}/countries", "http://localhost/Checkout.DevCon")),null,null,"GET",null));
         ScenarioContext.Current.Add("countriesList",countries);
        }

        [Then(@"the result should be list of countries")]
        public void ThenTheResultShouldBeListCountries()
        {
            var countries = ScenarioContext.Current["countriesList"] as IList<string>;

            Assert.NotNull(countries);
            Assert.Greater(countries.Count, 0);
        }
    }
}
