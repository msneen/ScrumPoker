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

        [HttpPost]
        public ActionResult MakeRoles()
        {            
            AddRole("Developer");
            AddRole("ScrumMaster");
            AddRole("SiteAdmin");

            return RedirectToAction("Vote", "Poker");
        }

        public JsonResult GetVotes(PokerGame pokerGame)
        {
            using (var db = new ScrumPoker.Models.UsersContext())
            {
                pokerGame.UserProfile = (from up in db.UserProfiles
                                         where up.UserName == User.Identity.Name
                                         select up).FirstOrDefault();
            }
            pokerGame.Votes = TaskEstimates.EstimateList;

            JsonResult jsonResult = Json(pokerGame, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        private static void AddRole(string roleName)
        {
            using (var db = new ScrumPoker.Entities())
            {
                var developerQuery = from r in db.webpages_Roles
                                     where r.RoleName == roleName
                                     select r;

                if (developerQuery.Count() < 1)
                {
                    webpages_Roles newRole = new webpages_Roles();
                    newRole.RoleName = roleName;
                    db.webpages_Roles.Add(newRole);
                    db.SaveChanges();
                }
            }
        }

    }
}
