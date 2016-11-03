using System.Web.Http;

namespace WebApplication1.api
{
    public class PingController : ApiController
    {
        // GET: api/Ping  This is just to be able to test that WebAPI is working correctly
        public string Get()
        {
            return "OK";
        }
    }
}
