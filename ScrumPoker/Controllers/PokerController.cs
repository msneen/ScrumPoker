﻿using System;
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
            SaveEstimateToSession(estimate, firstname);

            ViewBag.Estimate = estimate;
            ViewBag.FirstName = Session["FirstName"];
        
            return View();
        }

        private void SaveEstimateToSession(string estimate, string firstname)
        {
            var user = (from u in Users.UserList
                        where u.UserName == firstname
                        select u).FirstOrDefault<User>();

            if (!string.IsNullOrEmpty(estimate)   //if "estimate" contains a value
                    && !string.IsNullOrEmpty(firstname)  //and "firstname" contains a value
                    && (Users.UserList.Count == 0 ||  //and the userlist doesn't have any values
                                (Users.UserList.Count > 0  // OR the userlist does have values, and the submitted "firstname" is in the list of users.
                                && user != null  //To see the list of users, you must be logged in as one of the developers with firstInitialLastname
                                )
                            )
                )
            {
                Session["FirstName"] = firstname;

                AddEstimate(estimate, firstname);
            }
        }

        private static void AddEstimate(string estimate, string firstname)
        {
            TaskEstimate currentEstimate = (from e in TaskEstimates.EstimateList
                                            where firstname.Equals(e.Name, StringComparison.OrdinalIgnoreCase)
                                            select e).FirstOrDefault<TaskEstimate>();

            if (currentEstimate != null)
            {
                currentEstimate.Estimate = estimate;
            }
            else
            {
                TaskEstimates.EstimateList.Add(new TaskEstimate() { Name = firstname, Estimate = estimate });
            }
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
            AddEstimate("", userName);

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
            if (pokerGame.UserEstimate != null && !string.IsNullOrEmpty(pokerGame.UserEstimate.Name) && !string.IsNullOrEmpty(pokerGame.UserEstimate.Estimate))
            {
                SaveEstimateToSession(pokerGame.UserEstimate.Estimate, pokerGame.UserEstimate.Name);

                pokerGame.UserEstimate.Name = "";
                pokerGame.UserEstimate.Estimate = "";
            }
            using (var db = new ScrumPoker.Models.UsersContext())
            {
                pokerGame.UserProfile = (from up in db.UserProfiles
                                         where up.UserName == User.Identity.Name
                                         select up).FirstOrDefault();
            }
            pokerGame.Votes = TaskEstimates.EstimateList;
            pokerGame.Votes.Sort(delegate(TaskEstimate T1, TaskEstimate T2) 
            { 
                decimal t1 = 0;
                decimal t2 = 0;

                decimal.TryParse(T1.Estimate, out t1);
                decimal.TryParse(T2.Estimate, out t2);

                int estimateCompare = t1.CompareTo(t2);
                if(t1.CompareTo(t2) != 0)
                {
                    return estimateCompare;
                }
                else
                {
                    return T1.Name.CompareTo(T2.Name);
                }
            });

            JsonResult jsonResult = Json(pokerGame, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        private static void AddRole(string roleName)
        {
            using (var db = new ScrumPoker.Entities())
            {
                var developerQuery = from r in db.Roles1
                                     where r.RoleName == roleName
                                     select r;

                if (developerQuery.Count() < 1)
                {
                    Role newRole = new Role();
                    newRole.RoleName = roleName;
                    db.Roles1.Add(newRole);
                    db.SaveChanges();
                }
            }
        }

    }
}
