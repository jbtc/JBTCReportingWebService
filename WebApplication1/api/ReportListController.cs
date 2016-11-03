using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApplication1.api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ReportListController : ApiController
    {
        [HttpGet]
        [Route("api/reports")]
        public IEnumerable<ReportInfo> GetReports(string airport = null)
        {
            // If applicable to your application: Somehow get a list of airport codes that the user has permission 
            // for so you can do security trimming on the discovered reports
            string[] permittedAirportCodesForUser = new string[] { "HAS-IAH","RDU", "PHX", "JFK", "CID", "DAL", "EWR", "" }; // Empty string for generic reports

            var qry = System.IO.Directory
                .EnumerateFiles(System.Web.Hosting.HostingEnvironment.MapPath("~/Reports/"), "*.trdx")
                .Select(f => new ReportInfo(f))
                .Where(ri => permittedAirportCodesForUser.Contains(ri.ReportAirportCode));

            if (airport != null)
            {
                qry = qry.Where(ri => ri.ReportAirportCode == airport);
            }

            return qry;

            /* GET /api/reports    Note: Everything that the user has access to
             * Returns:
             * 
             * [
             *   { 
             *     "ReportAirportCode":"EWR",
             *     "ReportName":"EWRHelloReport",
             *     "ReportTitle":"EWR Hello Report",
             *     "ReportFilename":"EWRHelloReport.trdx",
             *     "ReportUrl":"/Reporting?report=EWRHelloReport.trdx"
             *   },
             *   {
             *     "ReportAirportCode":"",
             *     "ReportName":"swaASimpleReport",
             *     "ReportTitle":"A simple report",
             *     "ReportFilename":"swaASimpleReport.trdx",
             *     "ReportUrl":"/Reporting?report=swaASimpleReport.trdx"
             *    }
             *  ]
             * 
             * 
             * GET /api/reports?airport=EWR   Note: User has access to this airport
             * Returns:
             * 
             * [
             *   { 
             *     "ReportAirportCode":"EWR",
             *     "ReportName":"EWRHelloReport",
             *     "ReportTitle":"EWR Hello Report",
             *     "ReportFilename":"EWRHelloReport.trdx",
             *     "ReportUrl":"/Reporting?report=EWRHelloReport.trdx"
             *   }
             * ]             
             * 
             * GET /api/reports?airport=DTW   Note: User does not have access to this airport
             * Returns:
             * 
             * []              
             *      
             */
        }

    }
}
