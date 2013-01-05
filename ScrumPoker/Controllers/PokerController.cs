using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ScrumPoker.Models;

namespace ScrumPoker.Controllers
{
    public class PokerController : Controller
    {
        public List<string> colors;

        public PokerController() 
        {
            colors = new List<string>() { "Red", "Green", "Cyan", "Chartreuse", "Coral" };
            ViewBag.Colors = colors;
            ViewBag.Estimates = TaskEstimates.EstimateList;
        }

        public ActionResult Vote(string estimate, string firstname)
        {

            if (!string.IsNullOrEmpty(estimate) && !string.IsNullOrEmpty(firstname))
            {
                Session["FirstName"] = firstname;

                TaskEstimate currentEstimate = (from e in TaskEstimates.EstimateList
                                                where firstname.Equals(e.Name, StringComparison.OrdinalIgnoreCase)
                                                //&& estimate.Equals(e.Estimate, StringComparison.OrdinalIgnoreCase)
                                                select e).FirstOrDefault<TaskEstimate>();

                if (currentEstimate != null)
                {
                    currentEstimate.Estimate = estimate;
                }
                else
                {
                    TaskEstimates.EstimateList.Add(new TaskEstimate() { Name = firstname, Estimate = estimate });
                }

                ViewBag.Estimate = estimate;              
            }

            ViewBag.FirstName = Session["FirstName"];
        
            return View();
        }

        public ActionResult ClearAll()
        {
            TaskEstimates.EstimateList = new List<TaskEstimate>();

            return RedirectToAction("Vote", "Poker");
        }

       
        public ActionResult ClearVotes()
        {
            return ClearAllVotes();
        }

        [HttpPost]
        public ActionResult ClearVotes(string id)
        {
            return ClearAllVotes();
        }

        private ActionResult ClearAllVotes()
        {

                foreach (var estimate in TaskEstimates.EstimateList)
                {
                    estimate.Estimate = "";
                }
            
            return RedirectToAction("Vote", "Poker");
        }

    }
}
