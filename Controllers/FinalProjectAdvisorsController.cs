using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineStudentAcademicAdvisory.Models;

namespace OnlineStudentAcademicAdvisory.Controllers
{
    public class FinalProjectAdvisorsController : Controller
    {
        private OSAAEntities db = new OSAAEntities();

        // GET: FinalProjectAdvisors
        public ActionResult Index()
        {
            var finalProjectAdvisors = db.FinalProjectAdvisors.Include(f => f.AdvisorType).Include(f => f.FinalProject).Include(f => f.Instructor);
            return View(finalProjectAdvisors.ToList());
        }

        // GET: FinalProjectAdvisors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FinalProjectAdvisor finalProjectAdvisor = db.FinalProjectAdvisors.Find(id);
            if (finalProjectAdvisor == null)
            {
                return HttpNotFound();
            }
            return View(finalProjectAdvisor);
        }

        // GET: FinalProjectAdvisors/Create
        public ActionResult Create()
        {
            ViewBag.AdvisorTypeId = new SelectList(db.AdvisorTypes, "Id", "TypeName");
            ViewBag.FinalProjectId = new SelectList(db.FinalProjects, "Id", "ProjectTitle");
            ViewBag.InstractorId = new SelectList(db.Instructors, "Id", "FirstName");
            return View();
        }

        // POST: FinalProjectAdvisors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FinalProjectId,InstractorId,AdvisorTypeId")] FinalProjectAdvisor finalProjectAdvisor)
        {
            if (ModelState.IsValid)
            {
                db.FinalProjectAdvisors.Add(finalProjectAdvisor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AdvisorTypeId = new SelectList(db.AdvisorTypes, "Id", "TypeName", finalProjectAdvisor.AdvisorTypeId);
            ViewBag.FinalProjectId = new SelectList(db.FinalProjects, "Id", "ProjectTitle", finalProjectAdvisor.FinalProjectId);
            ViewBag.InstractorId = new SelectList(db.Instructors, "Id", "FirstName", finalProjectAdvisor.InstractorId);
            return View(finalProjectAdvisor);
        }

        // GET: FinalProjectAdvisors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FinalProjectAdvisor finalProjectAdvisor = db.FinalProjectAdvisors.Find(id);
            if (finalProjectAdvisor == null)
            {
                return HttpNotFound();
            }
            ViewBag.AdvisorTypeId = new SelectList(db.AdvisorTypes, "Id", "TypeName", finalProjectAdvisor.AdvisorTypeId);
            ViewBag.FinalProjectId = new SelectList(db.FinalProjects, "Id", "ProjectTitle", finalProjectAdvisor.FinalProjectId);
            ViewBag.InstractorId = new SelectList(db.Instructors, "Id", "FirstName", finalProjectAdvisor.InstractorId);
            return View(finalProjectAdvisor);
        }

        // POST: FinalProjectAdvisors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FinalProjectId,InstractorId,AdvisorTypeId")] FinalProjectAdvisor finalProjectAdvisor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(finalProjectAdvisor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdvisorTypeId = new SelectList(db.AdvisorTypes, "Id", "TypeName", finalProjectAdvisor.AdvisorTypeId);
            ViewBag.FinalProjectId = new SelectList(db.FinalProjects, "Id", "ProjectTitle", finalProjectAdvisor.FinalProjectId);
            ViewBag.InstractorId = new SelectList(db.Instructors, "Id", "FirstName", finalProjectAdvisor.InstractorId);
            return View(finalProjectAdvisor);
        }

        // GET: FinalProjectAdvisors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FinalProjectAdvisor finalProjectAdvisor = db.FinalProjectAdvisors.Find(id);
            if (finalProjectAdvisor == null)
            {
                return HttpNotFound();
            }
            return View(finalProjectAdvisor);
        }

        // POST: FinalProjectAdvisors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FinalProjectAdvisor finalProjectAdvisor = db.FinalProjectAdvisors.Find(id);
            db.FinalProjectAdvisors.Remove(finalProjectAdvisor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult AdvisorOngoingProjects()
        {
            int CurrentInstructor = int.Parse(Session["InstructorId"].ToString());
            var finalProjects = db.FinalProjectAdvisors.Where(f => f.InstractorId == CurrentInstructor && f.FinalProject.IsTerminated == false);
            return View(finalProjects.ToList());
        }
        public ActionResult AdvisorPreviousProjects()
        {
            int CurrentAdvisor = int.Parse(Session["InstructorId"].ToString());
            var finalProjects = db.FinalProjectAdvisors.Where(f => f.InstractorId == CurrentAdvisor && f.FinalProject.IsTerminated == true);
            return View(finalProjects.ToList());
        }
    }
}
