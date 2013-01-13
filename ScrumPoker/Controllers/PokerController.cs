﻿using System;
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
        private Entities db = new Entities();

        public PokerController() 
        {
            colors = new List<string>() { "White", "Red", "Green", "Pink", "Blue", "Magenta", "Cyan", "Chartreuse", "Coral" };
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
                SetProject( projectId);
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
                SetProject( Projects.Trim());
                int projectId = GetProjectId(); //Convert.ToInt32(Projects);

                Project project = _projectSvc.Find(projectId);
                if (project != null)
                {
                    foreach (var teamMember in project.TeamMembers)
                    {
                        AddEstimate("", teamMember.NickName, projectId, IsEmptyEstimate: true);
                    }
                }
                             
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
            if (Session["FirstName"] != null)
            { 
                pokerVm.FirstName = Session["FirstName"].ToString(); 
            }             

            pokerVm.Colors = colors;
            pokerVm.Projects = projects;
            pokerVm.ProjectId = GetProjectId();
            pokerVm.ProjectName = GetProjectName(pokerVm.ProjectId);
            return pokerVm;
        }

        private void ResetProject()
        {
            Session.Remove("CurrentProjectId");
        }

        private void SetProject(object projectId)
        {
            Session["CurrentProjectId"] = projectId;
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




            int projectId = GetProjectId(); //Convert.ToInt32(Projects);

            AddEstimate("", userName, projectId, IsEmptyEstimate:true);

            return RedirectToAction("Vote", "Poker", new { @id = GetProjectId() });
        }

        public JsonResult ClearAll(PokerGame pokerGame)
        {
            TaskEstimates.SetEstimateList(new List<TaskEstimate>());
            ResetProject();
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
                    int intestimate = 0;
                    int.TryParse(pokerGame.UserEstimate.Estimate, out intestimate);
                    if (intestimate > 0)
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
