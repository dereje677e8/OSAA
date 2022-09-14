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
    public class InstructorFeedbacksController : Controller
    {
        private OSAAEntities db = new OSAAEntities();

        // GET: InstructorFeedbacks
        public ActionResult Index()
        {
            var instructorFeedbacks = db.InstructorFeedbacks.Include(i => i.Instructor);
            return View(instructorFeedbacks.ToList());
        }

        // GET: InstructorFeedbacks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstructorFeedback instructorFeedback = db.InstructorFeedbacks.Find(id);
            if (instructorFeedback == null)
            {
                return HttpNotFound();
            }
            return View(instructorFeedback);
        }

        // GET: InstructorFeedbacks/Create
        public ActionResult Create()
        {
            ViewBag.InstructorId = new SelectList(db.Instructors, "Id", "FirstName");
            return View();
        }

        // POST: InstructorFeedbacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,InstructorId,FeedbackContent,FeedDate")] InstructorFeedback instructorFeedback)
        {
            if (ModelState.IsValid)
            {
                db.InstructorFeedbacks.Add(instructorFeedback);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InstructorId = new SelectList(db.Instructors, "Id", "FirstName", instructorFeedback.InstructorId);
            return View(instructorFeedback);
        }

        // GET: InstructorFeedbacks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstructorFeedback instructorFeedback = db.InstructorFeedbacks.Find(id);
            if (instructorFeedback == null)
            {
                return HttpNotFound();
            }
            ViewBag.InstructorId = new SelectList(db.Instructors, "Id", "FirstName", instructorFeedback.InstructorId);
            return View(instructorFeedback);
        }

        // POST: InstructorFeedbacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,InstructorId,FeedbackContent,FeedDate")] InstructorFeedback instructorFeedback)
        {
            if (ModelState.IsValid)
            {
                db.Entry(instructorFeedback).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InstructorId = new SelectList(db.Instructors, "Id", "FirstName", instructorFeedback.InstructorId);
            return View(instructorFeedback);
        }

        // GET: InstructorFeedbacks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstructorFeedback instructorFeedback = db.InstructorFeedbacks.Find(id);
            if (instructorFeedback == null)
            {
                return HttpNotFound();
            }
            return View(instructorFeedback);
        }

        // POST: InstructorFeedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InstructorFeedback instructorFeedback = db.InstructorFeedbacks.Find(id);
            db.InstructorFeedbacks.Remove(instructorFeedback);
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
        public ActionResult InstructorFeedback()
        {
            ViewBag.InstructorId = new SelectList(db.Instructors, "Id", "FirstName");
            return View();
        }

        // POST: InstructorFeedbacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InstructorFeedback(int InstructorId, string FeedbackContent)
        {
            InstructorFeedback instructorFeedback = new InstructorFeedback();
            if (ModelState.IsValid)
            {
                instructorFeedback.InstructorId = InstructorId;
                instructorFeedback.FeedbackContent = FeedbackContent;
                instructorFeedback.FeedDate = DateTime.Now;
                db.InstructorFeedbacks.Add(instructorFeedback);
                db.SaveChanges();
                TempData["Sent"] = "Feedback sent successfully!";
                return RedirectToAction("Index","Home");
            }
            TempData["NotSent"] = "Feeback not sent! some error occured!";

            ViewBag.InstructorId = new SelectList(db.Instructors, "Id", "FirstName", instructorFeedback.InstructorId);
            return View();
        }
        public ActionResult ViewDepartmentFeedback()
        {
            int currentHead = int.Parse(Session["DepartmentId"].ToString());
            var instructorFeedbacks = db.InstructorFeedbacks.Where(f => f.Instructor.DepartmentId == currentHead);
            return View(instructorFeedbacks.ToList());
        }
    }
}
