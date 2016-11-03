using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using Telerik.Reporting.Cache.File;
using Telerik.Reporting.Services;
using Telerik.Reporting.Services.WebApi;

namespace TelerikReportingSample.api
{
    public class ReportsController : ReportsControllerBase
    {
        static ReportServiceConfiguration configurationInstance;

        static ReportsController()
        {
            var reportsPath = HttpContext.Current.Server.MapPath("~/Reports");
            var resolver = new ReportFileResolver(reportsPath)
                .AddFallbackResolver(new ReportTypeResolver());

            configurationInstance = new ReportServiceConfiguration
            {
                HostAppId = "DemoApp", // This is arbitrary, and is primarily used when multiple web apps share this WebAPI
                Storage = new FileStorage("C:\\Temp"), // Directory used for caching purposes. Be sure to set permissions so IIS can write here
                ReportResolver = resolver,
                // ReportSharingTimeout = 0,
                // ClientSessionTimeout = 15,
            };
        }

        public ReportsController()
        {
            this.ReportServiceConfiguration = configurationInstance;
        }

        public override HttpResponseMessage CreateInstance(string clientID, ClientReportSource reportSource)
        {
            if (!CanUserAccessReport(reportSource.Report))
            {
                // Prevents the report viewer from showing the report
                return new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.BadRequest, ReasonPhrase = "User does not have permission for that airport code" };
            }

            return base.CreateInstance(clientID, reportSource);
        }

        public override HttpResponseMessage GetParameters(string clientID, ClientReportSource reportSource)
        {
            if (!CanUserAccessReport(reportSource.Report))
            {
                // Prevents the report viewer from showing parameters for the report
                return new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.BadRequest, ReasonPhrase = "User does not have permission for that airport code" };
            }

            return base.GetParameters(clientID, reportSource);
        }


        // Telerik Reporting doesn't have a good way that I could find to filter access to individual reports. This function will
        // show a way to determine whether the user can see a report or not based on a list of airport codes (i.e., queried from the
        // database for a user, etc).
        private bool CanUserAccessReport(string reportName)
        {
            // Get this from a database for the current this.User
            string[] permittedAirportCodesForUser = new string[] { "HAS-IAH", "RDU", "PHX", "JFK", "CID", "DAL", "EWR", "" }; // Empty string for generic reports

            var ReportAirportCode = "";
            if (reportName.Contains("-"))
                ReportAirportCode = reportName.Substring(0, 7);
            else
                ReportAirportCode = Regex.Match(reportName, "^[A-Z]{3}").Value; // If first three letters are all capitalized

            if (!string.IsNullOrWhiteSpace(ReportAirportCode) && Array.IndexOf(permittedAirportCodesForUser, ReportAirportCode) == -1)
            {
                return false;
            }

            return true;
        }
    }
}
