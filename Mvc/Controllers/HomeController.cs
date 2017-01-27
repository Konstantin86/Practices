using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc.Models;

namespace Mvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //[AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChangeCulture(TestViewModel lang)
        {
            var langCookie = new HttpCookie("lang", lang.Choice) { HttpOnly = true, Expires = DateTime.Now.AddYears(2) };
            Response.AppendCookie(langCookie);
            return RedirectToAction("Login", "Account", new { culture = lang });
        }
    }
}