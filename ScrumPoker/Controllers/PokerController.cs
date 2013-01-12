using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ScrumPoker.Models;
using ScrumPoker.Services;
using WebMatrix.WebData;
using ScrumPoker.Filters;
using ScrumPoker.ViewModels.Poker;

namespace ScrumPoker.Controllers
{
     [InitializeSimpleMembership]
    public class PokerController : Controller
    {
        public List<string> colors;
        private UserProfileSvc _userProfileSvc = new UserProfileSvc();
        private RoleSvc _roleSvc = new RoleSvc();
        private Entities db = new Entities();

        public PokerController() 
        {
            colors = new List<string>() { "Red", "Green", "Cyan", "Chartreuse", "Coral" };
            ViewBag.Colors = colors;
            ViewBag.Estimates = TaskEstimates.GetEstimateList();
        }

        public ActionResult Vote(string estimate, string firstname, string id)
        {
            int projectId = 0;
            if (!string.IsNullOrEmpty(id))
            {
                int.TryParse(id, out projectId);
            }
            else
            {
                projectId = GetProjectId(); ;
            }

            if (projectId > 0)
            {
                Session["CurrentProjectId"] = projectId;
                SaveEstimateToSession(estimate, firstname, projectId);

                ViewBag.Estimate = estimate;
                ViewBag.FirstName = Session["FirstName"];
            }

            PokerVm pokerVm = GetPokerVm();
            return View(pokerVm);
        }

        public ActionResult AddProject(string Projects)
        {
            if (!string.IsNullOrEmpty(Projects.Trim()))
            {
                Session["CurrentProjectId"] = Projects.Trim();
            }
            return RedirectToAction("Vote", "Poker", new { @id = GetProjectId() });
        }

        private PokerVm GetPokerVm()
        {
            PokerVm pokerVm = new PokerVm();
            List<Project> projects = new List<Project>();
            UserProfile userProfile = _userProfileSvc.Find(WebSecurity.GetUserId(User.Identity.Name));  //_userProfileSvc.Find( WebSecurity.GetUserId(User.Identity.Name));

            if (_userProfileSvc.IsInRole(userProfile, "ScrumMaster"))
            {
                projects = db.Projects.Include(p => p.UserProfile).Where(p => p.UserProfile.UserName == User.Identity.Name).ToList();
            }

            pokerVm.Projects = projects;
            pokerVm.ProjectId = GetProjectId();
            pokerVm.ProjectName = GetProjectName(pokerVm.ProjectId);
            return pokerVm;
        }

        private int GetProjectId()
        {
            int projectId = 0;
            if (Session["CurrentProjectId"] != null)
            {
                projectId = Convert.ToInt32(Session["CurrentProjectId"]);
            }
            return projectId;
        }

        private void SaveEstimateToSession(string estimate, string firstname, int projectId)
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

                AddEstimate(estimate, firstname, projectId);
            }
        }

        private static void AddEstimate(string estimate, string firstname, int projectId)
        {
            if (!string.IsNullOrEmpty(estimate) && !string.IsNullOrEmpty(firstname))
            {
                TaskEstimate currentEstimate = (from e in TaskEstimates.GetEstimateList(projectId)
                                                where firstname.Equals(e.Name, StringComparison.OrdinalIgnoreCase)
                                                select e).FirstOrDefault<TaskEstimate>();

                if (currentEstimate != null)
                {
                    currentEstimate.Estimate = estimate;
                }
                else
                {
                    TaskEstimates.GetEstimateList(projectId).Add(new TaskEstimate() { Name = firstname, Estimate = estimate });
                }
            }
        }

        [HttpPost]
        public ActionResult AddUser(string userName, string Projects, FormCollection collection)
        {
            
            User user = (from u in Users.UserList
                        where u.UserName == userName
                        select u).FirstOrDefault<User>();
            if (user == null && !string.IsNullOrEmpty(userName))
            {
                Users.UserList.Add(new User() { UserName = userName, IsSelected = true });
            }
            int projectId = Convert.ToInt32(Projects);

            AddEstimate("", userName, projectId);

            return RedirectToAction("Vote", "Poker", new { @id = GetProjectId() });
        }

        public JsonResult ClearAll(PokerGame pokerGame)
        {
            TaskEstimates.SetEstimateList(new List<TaskEstimate>());
            Users.UserList = new List<User>();
            pokerGame.ProjectId = 0;
            pokerGame.ProjectName = "";
            return GetVotes(pokerGame);
        }

        public JsonResult ClearVotes(PokerGame pokerGame)
        {
            ClearAllVotes();

            return GetVotes(pokerGame);
        }

        private void ClearAllVotes()
        {

                foreach (var estimate in TaskEstimates.GetEstimateList())
                {
                    estimate.Estimate = "";
                }
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
                SaveEstimateToSession(pokerGame.UserEstimate.Estimate, pokerGame.UserEstimate.Name, pokerGame.ProjectId);

                pokerGame.UserEstimate.Name = "";
                pokerGame.UserEstimate.Estimate = "";
            }
            using (var db = new ScrumPoker.Models.UsersContext())
            {
                pokerGame.UserProfile = (from up in db.UserProfiles
                                         where up.UserName == User.Identity.Name
                                         select up).FirstOrDefault();
            }
            
            pokerGame.ProjectName = GetProjectName(pokerGame.ProjectId);
            pokerGame.Votes = TaskEstimates.GetEstimateList(pokerGame.ProjectId);
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

        private static string GetProjectName(int projectId)
        {
            string projectName = "";
            if (projectId > 0)
            {
                using (Entities db = new Entities())
                {
                    projectName = db.Projects.Find(projectId).ProjectName;
                }
            }
            return projectName;
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
