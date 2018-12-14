using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace FileProcessor.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(List<HttpPostedFileBase> files)
        {
           
                return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "My contact details...";

            return View();
        }
    }
}