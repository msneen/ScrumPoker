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
            var user = (from u in Users.UserList
                       where u.UserName == firstname
                       select u).FirstOrDefault<User>();

            if (    !string.IsNullOrEmpty(estimate)   //if "estimate" contains a value
                    && !string.IsNullOrEmpty(firstname)  //and "firstname" contains a value
                    && (    Users.UserList.Count == 0 ||  //and the userlist doesn't have any values
                                (Users.UserList.Count > 0  // OR the userlist does have values, and the submitted "firstname" is in the list of users.
                                && user != null  //To see the list of users, you must be logged in as one of the developers with firstInitialLastname
                                )
                            )
                )
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

        [HttpPost]
        public ActionResult AddUser(string userName)
        {
            User user = (from u in Users.UserList
                        where u.UserName == userName
                        select u).FirstOrDefault<User>();
            if (user == null)
            {
                Users.UserList.Add(new User() { UserName = userName, IsSelected = true });
            }

            return RedirectToAction("Vote", "Poker");
        }

        public ActionResult ClearAll()
        {
            TaskEstimates.EstimateList = new List<TaskEstimate>();
            Users.UserList = new List<User>();

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
