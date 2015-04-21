using System.Collections.Generic;
using System.Web.Http;

namespace Checkout.DevCon.Controllers
{
    public class LookupController : ApiController
    {
        private readonly IList<string> _countries = new List<string>();

        public LookupController()
        {
            _countries = new List<string> {"Mauritius", "United Kingdom"};
        }

        [Route("countries")]
        public object Get()
        {
            return _countries;
        }
    }
}
