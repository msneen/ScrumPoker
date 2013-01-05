using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ScrumPoker.Models;

namespace ScrumPoker.Controllers
{
    public class HomeController : Controller
    {

        

        public ActionResult Poker(string estimate, string firstname)
        {
            List<string> colors = new List<string>() { "Red", "Green", "Cyan", "Chartreuse", "Coral" };
            //Session["TaskEstimates"] = null;
            List<TaskEstimate> taskEstimates = Session["TaskEstimates"] as List<TaskEstimate>;
            if (taskEstimates == null)
            {
                taskEstimates = new List<TaskEstimate>();
                Session["TaskEstimates"] = taskEstimates;
            }

            if (!string.IsNullOrEmpty(estimate))
            {


                TaskEstimate currentEstimate = (from e in taskEstimates
                                                where  firstname.Equals(e.Name, StringComparison.OrdinalIgnoreCase)
                                                //&& estimate.Equals(e.Estimate, StringComparison.OrdinalIgnoreCase)
                                               select e).FirstOrDefault<TaskEstimate>();

                if (currentEstimate != null)
                {
                    currentEstimate.Estimate = estimate;
                }
                else
                {
                    taskEstimates.Add(new TaskEstimate() { Name = firstname, Estimate = estimate });
                }

                ViewBag.Estimate = estimate;
                ViewBag.FirstName = firstname;
            }

            ViewBag.Estimates = taskEstimates;
            ViewBag.Colors = colors;

            return View();
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
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
