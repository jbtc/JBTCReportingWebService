using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WebApplication1
{
    public class ReportInfo
    {
        public string ReportAirportCode { get; set; }
        public string ReportName { get; set; }
        public string ReportTitle { get; set; }
        public string ReportFilename { get; set; }
        public string ReportUrl { get; set; }

        public ReportInfo() { }

        public ReportInfo(string filesystempath, string reportTitle = null)
        {
            ReportFilename = Regex.Replace(filesystempath, "^(.+\\\\)*(.+)\\.(.+)$", "$2.$3");
            ReportUrl = MvcRoute(ReportFilename);
            ReportName = Regex.Match(filesystempath, "^(.+\\\\)*(.+)\\.(.+)$").Groups[2].Value;
            if (ReportName.Contains("-"))
                ReportAirportCode = ReportName.Substring(0,7);
            else
                ReportAirportCode = Regex.Match(ReportName, "^[A-Z]{3}").Value; // If first three letters are all capitalized

            if (reportTitle != null)
            {
                ReportTitle = reportTitle;
            }
            else
            {
                ReportTitle = FormatReportTitle(ReportName);
            }
        }

        protected string FormatReportTitle(string reportName)
        {
            var report = reportName;

            if (ReportAirportCode != "")
            {
                report = report.Replace(ReportAirportCode, ""); // Strip off the airport code to get the common report name
            }

            if (reportNameSubstitutions.ContainsKey(report))
            {
                return string.Format("{0} {1}", ReportAirportCode, reportNameSubstitutions[report]).TrimStart(' ');
            }
            if(ReportAirportCode.Contains("-"))
                return reportName;
            else
                return string.Format("{0} {1}", ReportAirportCode, Regex.Replace(report, "(\\B[A-Z]{1})", " $1")).TrimStart(' '); // Break camel case string into words
        }

        protected string MvcRoute(string reportFilename)
        {
            return new System.Web.Mvc.UrlHelper(System.Web.HttpContext.Current.Request.RequestContext)
               .Action("Index", "Reporting", new { report = reportFilename });
        }

        // One simple way to offer report name substitutions is to maintain a list of filename-to-friendly name mappings
        // as a dictionary. You can also do something with config files (i.e., using AppSettings or some other mechanic)
        // to prevent the need to recompile after new reports have been added.
        private static Dictionary<string, string> reportNameSubstitutions = new Dictionary<string, string>
            {
                //{"swaAAopurtunitycost-BothGates", "SWAOppurtunityCost"},
               // {"swaalarmreport", "SWAAlarmReport" },
               // {"swawarningreport", "SWAWarningReport"},
               // {"swaalarmreport", "SWAAlarmReport" }
            };
    }
}