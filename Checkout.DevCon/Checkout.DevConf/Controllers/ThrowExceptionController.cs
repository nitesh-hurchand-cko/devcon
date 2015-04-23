using System;
using System.Web.Http;

namespace Checkout.DevCon.Controllers
{
    public class ThrowExceptionController : ApiController
    {
        [Route("errors")]
        public object Get()
        {
            throw new Exception("Application Error");
        }
    }
}
