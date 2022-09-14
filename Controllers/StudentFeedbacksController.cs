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
    public class StudentFeedbacksController : Controller
    {
        private OSAAEntities db = new OSAAEntities();

        // GET: StudentFeedbacks
        public ActionResult Index()
        {
            var studentFeedbacks = db.StudentFeedbacks.Include(s => s.Student);
            return View(studentFeedbacks.ToList());
        }

        // GET: StudentFeedbacks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentFeedback studentFeedback = db.StudentFeedbacks.Find(id);
            if (studentFeedback == null)
            {
                return HttpNotFound();
            }
            return View(studentFeedback);
        }

        // GET: StudentFeedbacks/Create
        public ActionResult Create()
        {
            ViewBag.StudentId = new SelectList(db.Students, "Id", "IdNumber");
            return View();
        }

        // POST: StudentFeedbacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StudentId,FeedbackContent,FeedDate")] StudentFeedback studentFeedback)
        {
            if (ModelState.IsValid)
            {
                db.StudentFeedbacks.Add(studentFeedback);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StudentId = new SelectList(db.Students, "Id", "IdNumber", studentFeedback.StudentId);
            return View(studentFeedback);
        }

        // GET: StudentFeedbacks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentFeedback studentFeedback = db.StudentFeedbacks.Find(id);
            if (studentFeedback == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentId = new SelectList(db.Students, "Id", "IdNumber", studentFeedback.StudentId);
            return View(studentFeedback);
        }

        // POST: StudentFeedbacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StudentId,FeedbackContent,FeedDate")] StudentFeedback studentFeedback)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentFeedback).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StudentId = new SelectList(db.Students, "Id", "IdNumber", studentFeedback.StudentId);
            return View(studentFeedback);
        }

        // GET: StudentFeedbacks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentFeedback studentFeedback = db.StudentFeedbacks.Find(id);
            if (studentFeedback == null)
            {
                return HttpNotFound();
            }
            return View(studentFeedback);
        }

        // POST: StudentFeedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentFeedback studentFeedback = db.StudentFeedbacks.Find(id);
            db.StudentFeedbacks.Remove(studentFeedback);
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
        public ActionResult StudentFeedback()
        {
            ViewBag.StudentId = new SelectList(db.Students, "Id", "IdNumber");

            return View();
        }

        // POST: InstructorFeedbacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StudentFeedback(int StudentId, string FeedbackContent)
        {
            StudentFeedback studentFeedback = new StudentFeedback();
            if (ModelState.IsValid)
            {
                studentFeedback.StudentId = StudentId;
                studentFeedback.FeedbackContent = FeedbackContent;
                studentFeedback.FeedDate = DateTime.Now;
                db.StudentFeedbacks.Add(studentFeedback);
                db.SaveChanges();
                TempData["Sent"] = "Feedback sent successfully!";

                return RedirectToAction("Index","Home");
            }
            TempData["NotSent"] = "Feeback not sent! some error occured!";
            ViewBag.StudentId = new SelectList(db.Students, "Id", "IdNumber", studentFeedback.StudentId);

            return View();
        }
    }
}
