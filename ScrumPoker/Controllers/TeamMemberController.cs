using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScrumPoker.Controllers
{
    public class TeamMemberController : Controller
    {
        private Entities _db; // = new Entities();

        public TeamMemberController(Entities db)
        {
            _db = db;
        }
        //
        // GET: /TeamMember/

        public ActionResult Index()
        {
            var teammembers = _db.TeamMembers.Include(t => t.Project).Include(t => t.UserProfile);
            return View(teammembers.ToList());
        }

        //
        // GET: /TeamMember/Details/5

        public ActionResult Details(int id = 0)
        {
            TeamMember teammember = _db.TeamMembers.Find(id);
            if (teammember == null)
            {
                return HttpNotFound();
            }
            return View(teammember);
        }

        //
        // GET: /TeamMember/Create

        public ActionResult Create()
        {
            ViewBag.ProjectId = new SelectList(_db.Projects, "id", "ProjectName");
            ViewBag.UserId = new SelectList(_db.UserProfileScrums, "UserId", "UserName");
            return View();
        }

        //
        // POST: /TeamMember/Create

        [HttpPost]
        public ActionResult Create(TeamMember teammember)
        {
            if (ModelState.IsValid)
            {
                _db.TeamMembers.Add(teammember);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectId = new SelectList(_db.Projects, "id", "ProjectName", teammember.ProjectId);
            ViewBag.UserId = new SelectList(_db.UserProfileScrums, "UserId", "UserName", teammember.UserId);
            return View(teammember);
        }

        //
        // GET: /TeamMember/Edit/5

        public ActionResult Edit(int id = 0)
        {
            TeamMember teammember = _db.TeamMembers.Find(id);
            if (teammember == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = new SelectList(_db.Projects, "id", "ProjectName", teammember.ProjectId);
            ViewBag.UserId = new SelectList(_db.UserProfileScrums, "UserId", "UserName", teammember.UserId);
            return View(teammember);
        }

        //
        // POST: /TeamMember/Edit/5

        [HttpPost]
        public ActionResult Edit(TeamMember teammember)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(teammember).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(_db.Projects, "id", "ProjectName", teammember.ProjectId);
            ViewBag.UserId = new SelectList(_db.UserProfileScrums, "UserId", "UserName", teammember.UserId);
            return View(teammember);
        }

        //
        // GET: /TeamMember/Delete/5

        public ActionResult Delete(int id = 0)
        {
            TeamMember teammember = _db.TeamMembers.Find(id);
            if (teammember == null)
            {
                return HttpNotFound();
            }
            return View(teammember);
        }

        //
        // POST: /TeamMember/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            TeamMember teammember = _db.TeamMembers.Find(id);
            _db.TeamMembers.Remove(teammember);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}