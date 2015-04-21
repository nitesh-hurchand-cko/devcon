using System;
using TechTalk.SpecFlow;

namespace Checkout.DevCon.Tests
{
    [Binding]
    public class Login
    {
        [Given(@"I have a username (.*)")]
        public void GivenIHaveAValidUsername(string username)
        {
            ScenarioContext.Current.Add("username", username);
        }

        [Given(@"I have a password (.*)")]
        public void GivenIHaveAValidPassword(string password)
        {
            ScenarioContext.Current.Add("password", password);
        }

        [When(@"I send a request to log in")]
        public void WhenISendARequestToLogIn()
        {
            var username = ScenarioContext.Current["username"].ToString();
            var password = ScenarioContext.Current["password"].ToString();
            var endpoint = String.Format("{0}/account/login", "http://localhost/Checkout.DevCon");
            var result = ApiHelper.CreateLogin(endpoint, username, password);
            ScenarioContext.Current.Add("login", result);
        }

        [Then(@"I should be authenticated successfully")]
        public void ThenIShouldBeAuthenticatedSuccessfully()
        {
            var response = ScenarioContext.Current["login"].ToString();
        }

        [Then(@"I should should not be authenticated successfully")]
        public void ThenIShouldNotBeAuthenticatedSuccessfully()
        {
            var response = ScenarioContext.Current["login"].ToString();
        }
    }
}
