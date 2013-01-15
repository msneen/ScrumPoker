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
        private ProjectSvc _projectSvc = new ProjectSvc();
        private FinalEstimateSvc _finalEstimateSvc = new FinalEstimateSvc();
        private Entities db = new Entities();

        public PokerController() 
        {
            //colors = new List<string>() { "White", "Red", "Green", "Pink", "Blue", "Magenta", "Cyan", "Chartreuse", "Coral" };
            colors = new List<string>() { "#5ABFC6", "#CE95C8", "#D1C57E", "#E85AAA", "#FF2626", "#009ACD", "#FFFF00", "#FBBF51", "#9EFC7D" };
            ViewBag.Colors = colors;
            ViewBag.Estimates = TaskEstimates.GetEstimateList();
        }

         //this is called to render the main voting "view" back to the browser
        public ActionResult Vote(string estimate, string firstname, string id)
        {
            int projectId = 0;
            if (!string.IsNullOrEmpty(id))
            {
                int.TryParse(id, out projectId);
            }
            else
            {
                projectId = TaskEstimates.GetProjectId(); ;
            }

            if (projectId > 0)
            {
                TaskEstimates.SetProject(projectId);
                SaveEstimateToSession(estimate, firstname, projectId);

                ViewBag.Estimate = estimate;
                ViewBag.FirstName = Session["FirstName"];
            }

            PokerVm pokerVm = GetPokerVm();
            return View(pokerVm);
        }

         //This is called by the page to add a project to vote on from the ScrumMaster dropdown list
        public ActionResult AddProject(string Projects)
        {
            if (!string.IsNullOrEmpty(Projects.Trim()))
            {
                TaskEstimates.SetProject(Projects.Trim());
                int projectId = TaskEstimates.GetProjectId(); //Convert.ToInt32(Projects);

                Project project = _projectSvc.Find(projectId);
                if (project != null)
                {
                    foreach (var teamMember in project.TeamMembers)
                    {
                        AddEstimate("", teamMember.NickName, projectId, IsEmptyEstimate: true);
                    }
                }
                             
            }
            return RedirectToAction("Vote", "Poker", new { @id = TaskEstimates.GetProjectId() });
        }

         //this builds the Poker viewModel and should be moved to a helper class
        private PokerVm GetPokerVm()
        {
            PokerVm pokerVm = new PokerVm();
            List<Project> projects = new List<Project>();
            UserProfile userProfile = _userProfileSvc.Find(WebSecurity.GetUserId(User.Identity.Name));  //_userProfileSvc.Find( WebSecurity.GetUserId(User.Identity.Name));

            if (_userProfileSvc.IsInRole(userProfile, "ScrumMaster"))
            {
                projects = db.Projects.Include(p => p.UserProfile).Where(p => p.UserProfile.UserName == User.Identity.Name).ToList();
            }
            if (Session["FirstName"] != null)
            { 
                pokerVm.FirstName = Session["FirstName"].ToString(); 
            }             

            pokerVm.Colors = colors;
            pokerVm.Projects = projects;
            pokerVm.ProjectId = TaskEstimates.GetProjectId();
            pokerVm.ProjectName = GetProjectName(pokerVm.ProjectId);
            pokerVm.CurrentProject = _projectSvc.Find(pokerVm.ProjectId);
            return pokerVm;
        }

         





        private void SaveEstimateToSession(string estimate, string firstname, int projectId)
        {
            if (!string.IsNullOrEmpty(estimate)   //if "estimate" contains a value
                    && !string.IsNullOrEmpty(firstname)  //and "firstname" contains a value                    
                )
            {
                Session["FirstName"] = firstname;

                AddEstimate(estimate, firstname, projectId);
            }
        }

        private static void AddEstimate(string estimate, string firstname, int projectId, bool IsEmptyEstimate = false)
        {
            if (( IsEmptyEstimate == true || (IsEmptyEstimate == false &&  !string.IsNullOrEmpty(estimate))) && !string.IsNullOrEmpty(firstname))
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
            //List<TaskEstimate> taskEstimates = TaskEstimates.GetEstimateList(); 




            int projectId = TaskEstimates.GetProjectId(); //Convert.ToInt32(Projects);

            AddEstimate("", userName, projectId, IsEmptyEstimate:true);

            return RedirectToAction("Vote", "Poker", new { @id = TaskEstimates.GetProjectId() });
        }

        public JsonResult ClearAll(PokerGame pokerGame)
        {
            TaskEstimates.SetVotingTaskId("");
            TaskEstimates.SetEstimateList(new List<TaskEstimate>());
            TaskEstimates.ResetProject();
            pokerGame.ProjectId = 0;
            pokerGame.ProjectName = "";
            return GetVotes(pokerGame);
        }

        public JsonResult ClearVotes(PokerGame pokerGame)
        {
            ClearAllVotes();

            return GetVotes(pokerGame);
        }

        private void ClearAllVotes(string finalVote = "")
        {
            TaskEstimates.SetVotingTaskId("");
            foreach (var estimate in TaskEstimates.GetEstimateList())
            {
                estimate.Estimate = finalVote;
            }
        }

         //This is called by the ScrumMaster "Save Team" button under the Votes
        public JsonResult SaveTeamMembers(PokerGame pokerGame)
        {
            if (pokerGame.ProjectId > 0)
            {
                List<TaskEstimate> taskEstimates = TaskEstimates.GetEstimateList(pokerGame.ProjectId);
                using (Entities db = new Entities())
                {
                    var savedTeamMembers = (from tm in db.TeamMembers
                                            where tm.ProjectId == pokerGame.ProjectId
                                            select tm).ToList<TeamMember>();

                    foreach (var taskEstimate in taskEstimates)
                    {
                        TeamMember foundTeamMember = savedTeamMembers.Find(m => m.NickName == taskEstimate.Name);
                        if (foundTeamMember == null)
                        {
                            TeamMember newMember = new TeamMember();
                            newMember.ProjectId = pokerGame.ProjectId;
                            newMember.NickName = taskEstimate.Name;
                            db.TeamMembers.Add(newMember);
                            db.SaveChanges();
                        }
                    }
                }
                
            }
            return GetVotes(pokerGame);
        }
        public JsonResult SaveMyVote(PokerGame pokerGame)
        {
            if (pokerGame.ProjectId > 0)
            {
                decimal? nullableEstimate = -1;
                decimal estimate = -1;
                if (pokerGame.UserEstimate.Estimate.Trim().Equals("?"))
                    nullableEstimate = null;
                else if (pokerGame.UserEstimate.Estimate.Trim().Equals("1/4"))
                    nullableEstimate = .25m;
                else if (pokerGame.UserEstimate.Estimate.Trim().Equals("1/2"))
                    nullableEstimate = .5m;
                else
                {
                    decimal.TryParse(pokerGame.UserEstimate.Estimate, out estimate);
                    nullableEstimate = estimate;
                }
                
                //Save final Vote Here
                if (pokerGame.UserEstimate.Estimate.Trim().Equals("?") || estimate >= -1)
                {
                    FinalEstimate finalEstimate = new FinalEstimate();
                    finalEstimate.ProjectId = pokerGame.ProjectId;
                    finalEstimate.TaskId = pokerGame.TaskId;
                    finalEstimate.Estimate = nullableEstimate;
                    _finalEstimateSvc.Add(finalEstimate);

                    pokerGame.UserEstimate.Estimate = "";
                    ClearAllVotes();
                }
            }
            return GetVotes(pokerGame);
        }

        public JsonResult SetVotingTaskId(PokerGame pokerGame)
        {
            if (pokerGame.ProjectId > 0)
            {
                ClearAllVotes();
                TaskEstimates.SetVotingTaskId(pokerGame.TaskId);                
            }
            return GetVotes(pokerGame);
        }



        //This is the main method of the whole app.
        public JsonResult GetVotes(PokerGame pokerGame)
        {
 
            if (pokerGame.UserEstimate != null && !string.IsNullOrEmpty(pokerGame.UserEstimate.Name) && !string.IsNullOrEmpty(pokerGame.UserEstimate.Estimate))
            {
                decimal? nullableEstimate = -1;
                decimal estimate = -1;
                if (pokerGame.UserEstimate.Estimate.Trim().Equals("?"))
                    nullableEstimate = null;
                else if (pokerGame.UserEstimate.Estimate.Trim().Equals("1/4"))
                    nullableEstimate = .25m;
                else if (pokerGame.UserEstimate.Estimate.Trim().Equals("1/2"))
                    nullableEstimate = .5m;
                else
                {
                    decimal.TryParse(pokerGame.UserEstimate.Estimate, out estimate);
                    nullableEstimate = estimate;
                }
                if (pokerGame.UserEstimate.Estimate.Trim().Equals("?") || estimate >= -1)
                    {
                        SaveEstimateToSession(pokerGame.UserEstimate.Estimate, pokerGame.UserEstimate.Name, pokerGame.ProjectId);

                        pokerGame.UserEstimate.Name = "";
                        pokerGame.UserEstimate.Estimate = "";
                    }

            }
            using (var db = new ScrumPoker.Models.UsersContext())
            {
                pokerGame.UserProfile = (from up in db.UserProfiles
                                         where up.UserName == User.Identity.Name
                                         select up).FirstOrDefault();
            }
            
            pokerGame.ProjectName = GetProjectName(pokerGame.ProjectId);
            pokerGame.TaskId = TaskEstimates.GetVotingTaskId();
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

        //This is currently unused.  It will make the 3 necessary roles in your database
        [HttpPost]
        public ActionResult MakeRoles()
        {
            AddRole("Developer");
            AddRole("ScrumMaster");
            AddRole("SiteAdmin");

            return RedirectToAction("Vote", "Poker");
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
