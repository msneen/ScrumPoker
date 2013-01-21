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
        private Entities _db; // = new Entities();

        public FinalVoteController(Entities db)
        {
            _db = db;
        }

        //
        // GET: /FinalVote/

        public ActionResult Index(int id)
        {
            int projectId = Convert.ToInt32(id);
            var finalestimates = _db.FinalEstimates.Include(f => f.Project).Where(f => f.ProjectId == projectId);
            ViewBag.ProjectId = projectId;
            return View(finalestimates.ToList());
        }

        //
        // GET: /FinalVote/Details/5

        public ActionResult Details(int id = 0)
        {
            FinalEstimate finalestimate = _db.FinalEstimates.Find(id);
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
            ViewBag.ProjectId = new SelectList(_db.Projects, "id", "ProjectName");
            return View();
        }

        //
        // POST: /FinalVote/Create

        [HttpPost]
        public ActionResult Create(FinalEstimate finalestimate)
        {
            if (ModelState.IsValid)
            {
                _db.FinalEstimates.Add(finalestimate);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectId = new SelectList(_db.Projects, "id", "ProjectName", finalestimate.ProjectId);
            return View(finalestimate);
        }

        //
        // GET: /FinalVote/Edit/5

        public ActionResult Edit(int id = 0)
        {
            FinalEstimate finalestimate = _db.FinalEstimates.Find(id);
            if (finalestimate == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = new SelectList(_db.Projects, "id", "ProjectName", finalestimate.ProjectId);
            return View(finalestimate);
        }

        //
        // POST: /FinalVote/Edit/5

        [HttpPost]
        public ActionResult Edit(FinalEstimate finalestimate)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(finalestimate).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(_db.Projects, "id", "ProjectName", finalestimate.ProjectId);
            return View(finalestimate);
        }

        //
        // GET: /FinalVote/Delete/5

        public ActionResult Delete(int id = 0)
        {
            FinalEstimate finalestimate = _db.FinalEstimates.Find(id);
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
            FinalEstimate finalestimate = _db.FinalEstimates.Find(id);
            _db.FinalEstimates.Remove(finalestimate);
            _db.SaveChanges();
            int projectId = TaskEstimates.GetProjectId();
            return RedirectToAction("Index", new { id=projectId});
        }
        
        //[HttpPost]
        public ActionResult DeleteAll(int projectId)
        {
            if (projectId == TaskEstimates.GetProjectId())
            {
                var finalEstimatesQuery = from f in _db.FinalEstimates
                            where f.ProjectId == projectId
                            select f;

                foreach (var finalEstimate in finalEstimatesQuery.ToList())
                {
                    _db.FinalEstimates.Remove(finalEstimate);
                }
                _db.SaveChanges();

            }
            return RedirectToAction("Index", new { id = projectId });
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}