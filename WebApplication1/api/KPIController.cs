using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApplication1.api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class KPIController : ApiController
    {
        [HttpGet]
        [Route("api/KPI")]
        public IEnumerable<lineData> GetKPI(string airport = null)
        {
            // If applicable to your application: Somehow get a list of airport codes that the user has permission 
            // for so you can do security trimming on the discovered reports
            string[] permittedAirportCodesForUser = new string[] { "JFK", "CID", "DAL", "EWR", "" }; // Empty string for generic reports

            List<lineData> set = new List<lineData>() {
                new lineData() { TimeStamp = "2002", Value = 1 },
                new lineData() { TimeStamp = "2003", Value = 2 },
                new lineData() { TimeStamp = "2004", Value = 3 },
                new lineData() { TimeStamp = "2005", Value = 4 },
                new lineData() { TimeStamp = "2006", Value = 5 },
                new lineData() { TimeStamp = "2007", Value = 6 },
                new lineData() { TimeStamp = "2008", Value = 7 },
                new lineData() { TimeStamp = "2009", Value = 8 },
                new lineData() { TimeStamp = "2010", Value = 7 },
                new lineData() { TimeStamp = "2011", Value = 6 },
                new lineData() { TimeStamp = "2012", Value = 5 },
                new lineData() { TimeStamp = "2013", Value = 4 },
                new lineData() { TimeStamp = "2014", Value = 3 },
                new lineData() { TimeStamp = "2015", Value = 2 },
                new lineData() { TimeStamp = "2016", Value = 1 }
            };
            return set.ToArray();
        }
    }

    public class lineData
    {
        public string TimeStamp { get; set; }
        public int Value { get; set; }
    }
}
