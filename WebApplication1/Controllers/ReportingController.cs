using System;
using System.Web.Mvc;

namespace TelerikReportingSample.Controllers
{
    public class ReportingController : Controller
    {
        public ActionResult Index(string report)
        {
            // Example route: http://localhost:34059/Reporting?report=AnotherReport.trdx
            if (!String.IsNullOrWhiteSpace(report))
            {
                ViewBag.Report = report;
                return View();
            }

            // Compensating action to preform if the report parameter is not provided
            return RedirectToAction("Index", "Home");
        }
    }
}