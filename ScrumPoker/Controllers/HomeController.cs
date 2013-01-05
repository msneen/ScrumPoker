using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ScrumPoker.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Poker()
        {
            return RedirectToAction("Vote", "Poker");
        }
        public ActionResult Index()
        {
            return RedirectToAction("Vote", "Poker");

            //return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


    }
}
