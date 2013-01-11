using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ScrumPoker.Filters;
using ScrumPoker.Services;
using WebMatrix.WebData;
using ScrumPoker.Models;

namespace ScrumPoker.Controllers
{
    [Authorize(Roles = "ScrumMaster,SiteAdmin")]
    [InitializeSimpleMembership]
    public class ProjectController : Controller
    {
        private UserProfileSvc _userProfileSvc = new UserProfileSvc();
        private RoleSvc  _roleSvc = new RoleSvc();
        private Entities db = new Entities();

        //
        // GET: /Project/

        public ActionResult Index()
        {
            List<Project> projects = new List<Project>();
            UserProfile userProfile = _userProfileSvc.Find(WebSecurity.GetUserId(User.Identity.Name));  //_userProfileSvc.Find( WebSecurity.GetUserId(User.Identity.Name));
           
            if (_userProfileSvc.IsInRole(userProfile, "SiteAdmin"))
            {
                projects = db.Projects.Include(p => p.UserProfile).ToList();
            }
            else if (_userProfileSvc.IsInRole(userProfile, "ScrumMaster"))
            {
                projects = db.Projects.Include(p => p.UserProfile).Where(p => p.UserProfile.UserName == User.Identity.Name).ToList();
            }

            return View(projects);
        }

        //
        // GET: /Project/Details/5

        public ActionResult Details(int id = 0)
        {
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        //
        // GET: /Project/Create

        public ActionResult Create()
        {
            ViewBag.CreatedUserId = new SelectList(db.UserProfileScrums, "UserId", "UserName");
            return View();
        }

        //
        // POST: /Project/Create

        [HttpPost]
        public ActionResult Create(Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CreatedUserId = new SelectList(db.UserProfileScrums, "UserId", "UserName", project.CreatedUserId);
            return View(project);
        }

        //
        // GET: /Project/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatedUserId = new SelectList(db.UserProfileScrums, "UserId", "UserName", project.CreatedUserId);
            return View(project);
        }

        //
        // POST: /Project/Edit/5

        [HttpPost]
        public ActionResult Edit(Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreatedUserId = new SelectList(db.UserProfileScrums, "UserId", "UserName", project.CreatedUserId);
            return View(project);
        }

        //
        // GET: /Project/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        //
        // POST: /Project/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}