using System;
using System.Web.Http;

namespace Checkout.DevCon.Controllers
{
    //[RoutePrefix("")]
    public class ThrowExceptionController : ApiController
    {
        [Route("errors")]
        public object Get()
        {
            throw new Exception("Application Error");
        }
    }
}
