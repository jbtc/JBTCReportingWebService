using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.ReportFiles = System.IO.Directory.EnumerateFiles(Server.MapPath("~/Reports/"), "*.trdx").Select(f => new ReportInfo(f));
            return View();
        }
    }
}