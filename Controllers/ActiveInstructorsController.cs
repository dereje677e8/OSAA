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
    public class ActiveInstructorsController : Controller
    {
        private OSAAEntities db = new OSAAEntities();

        // GET: ActiveInstructors
        public ActionResult Index()
        {
            var currentInstructor = int.Parse(Session["DepartmentId"].ToString());
            var activeInstructors = db.ActiveInstructors.Where(f => f.IsActive == true && f.Instructor.Department.Id == currentInstructor);
            ViewBag.InstructorId = new SelectList(db.Instructors.Where(f => f.DepartmentId == currentInstructor), "Id", "FirstName");
            ViewBag.InstructorId = new SelectList(db.Instructors, "Id", "FirstName");

            return View(activeInstructors.ToList());
        }

        // GET: ActiveInstructors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActiveInstructor activeInstructor = db.ActiveInstructors.Find(id);
            if (activeInstructor == null)
            {
                return HttpNotFound();
            }
            return View(activeInstructor);
        }

        // GET: ActiveInstructors/Create
        public ActionResult Create()
        {
            ViewBag.InstructorId = new SelectList(db.Instructors, "Id", "FirstName");
            return View();
        }

        // POST: ActiveInstructors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int InstructorId)
        {
            ActiveInstructor activeInstructor = new ActiveInstructor();
            if (ModelState.IsValid)
            {
                activeInstructor.InstructorId = InstructorId;
                activeInstructor.IsActive = true;
                db.ActiveInstructors.Add(activeInstructor);
                db.SaveChanges();
                TempData["Successfull"] = "Adding Successful!";
                return RedirectToAction("Index");
            }

            ViewBag.InstructorId = new SelectList(db.Instructors, "Id", "FirstName", activeInstructor.InstructorId);
            return View(activeInstructor);
        }

        // GET: ActiveInstructors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActiveInstructor activeInstructor = db.ActiveInstructors.Find(id);
            if (activeInstructor == null)
            {
                return HttpNotFound();
            }
            ViewBag.InstructorId = new SelectList(db.Instructors, "Id", "FirstName", activeInstructor.InstructorId);
            return View(activeInstructor);
        }

        // POST: ActiveInstructors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,InstructorId,IsActive")] ActiveInstructor activeInstructor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activeInstructor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InstructorId = new SelectList(db.Instructors, "Id", "FirstName", activeInstructor.InstructorId);
            return View(activeInstructor);
        }

        // GET: ActiveInstructors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActiveInstructor activeInstructor = db.ActiveInstructors.Find(id);
            if (activeInstructor == null)
            {
                return HttpNotFound();
            }
            return View(activeInstructor);
        }

        // POST: ActiveInstructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ActiveInstructor activeInstructor = db.ActiveInstructors.Find(id);
            db.ActiveInstructors.Remove(activeInstructor);
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
    }
}
