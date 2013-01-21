using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ScrumPoker.Models;

namespace ScrumPoker.Controllers
{
    public class FinalVoteController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /FinalVote/

        public ActionResult Index(int id)
        {
            int projectId = Convert.ToInt32(id);
            var finalestimates = db.FinalEstimates.Include(f => f.Project).Where(f => f.ProjectId == projectId);
            return View(finalestimates.ToList());
        }

        //
        // GET: /FinalVote/Details/5

        public ActionResult Details(int id = 0)
        {
            FinalEstimate finalestimate = db.FinalEstimates.Find(id);
            if (finalestimate == null)
            {
                return HttpNotFound();
            }
            return View(finalestimate);
        }

        //
        // GET: /FinalVote/Create

        public ActionResult Create()
        {
            ViewBag.ProjectId = new SelectList(db.Projects, "id", "ProjectName");
            return View();
        }

        //
        // POST: /FinalVote/Create

        [HttpPost]
        public ActionResult Create(FinalEstimate finalestimate)
        {
            if (ModelState.IsValid)
            {
                db.FinalEstimates.Add(finalestimate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectId = new SelectList(db.Projects, "id", "ProjectName", finalestimate.ProjectId);
            return View(finalestimate);
        }

        //
        // GET: /FinalVote/Edit/5

        public ActionResult Edit(int id = 0)
        {
            FinalEstimate finalestimate = db.FinalEstimates.Find(id);
            if (finalestimate == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "id", "ProjectName", finalestimate.ProjectId);
            return View(finalestimate);
        }

        //
        // POST: /FinalVote/Edit/5

        [HttpPost]
        public ActionResult Edit(FinalEstimate finalestimate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(finalestimate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "id", "ProjectName", finalestimate.ProjectId);
            return View(finalestimate);
        }

        //
        // GET: /FinalVote/Delete/5

        public ActionResult Delete(int id = 0)
        {
            FinalEstimate finalestimate = db.FinalEstimates.Find(id);
            if (finalestimate == null)
            {
                return HttpNotFound();
            }
            return View(finalestimate);
        }

        //
        // POST: /FinalVote/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            FinalEstimate finalestimate = db.FinalEstimates.Find(id);
            db.FinalEstimates.Remove(finalestimate);
            db.SaveChanges();
            int projectId = TaskEstimates.GetProjectId();
            return RedirectToAction("Index", new { id=projectId});
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}