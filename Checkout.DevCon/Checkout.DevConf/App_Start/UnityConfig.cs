using Checkout.DevCon.Validators;
using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;

namespace Checkout.DevCon
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            container.RegisterType<ILoginModelValidator, LoginModelValidator>();
            container.RegisterType<IPhoneModelValidator, PhoneModelValidator>();
            container.RegisterType<IAddressModelValidator, AddressModelValidator>();
            container.RegisterType<IUserModelValidator, UserModelValidator>();
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}