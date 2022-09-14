using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineStudentAcademicAdvisory.Models;

namespace OnlineStudentAcademicAdvisory.Controllers
{
    public class InstructorImagesController : Controller
    {
        private OSAAEntities db = new OSAAEntities();

        // GET: InstructorImages
        public ActionResult Index()
        {
            var instructorImages = db.InstructorImages.Include(i => i.Instructor);
            return View(instructorImages.ToList());
        }

        // GET: InstructorImages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstructorImage instructorImage = db.InstructorImages.Find(id);
            if (instructorImage == null)
            {
                return HttpNotFound();
            }
            return View(instructorImage);
        }

        // GET: InstructorImages/Create
        public ActionResult Create()
        {
            ViewBag.InstructorId = new SelectList(db.Instructors, "Id", "FirstName");
            return View();
        }

        // POST: InstructorImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UploadDate,BackgroundImage,ProfilePictureURL,ActiveProfilePicture,ActiveBackgroundImage,InstructorId")] InstructorImage instructorImage)
        {
            if (ModelState.IsValid)
            {
                db.InstructorImages.Add(instructorImage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InstructorId = new SelectList(db.Instructors, "Id", "FirstName", instructorImage.InstructorId);
            return View(instructorImage);
        }

        // GET: InstructorImages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstructorImage instructorImage = db.InstructorImages.Find(id);
            if (instructorImage == null)
            {
                return HttpNotFound();
            }
            ViewBag.InstructorId = new SelectList(db.Instructors, "Id", "FirstName", instructorImage.InstructorId);
            return View(instructorImage);
        }

        // POST: InstructorImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UploadDate,BackgroundImage,ProfilePictureURL,ActiveProfilePicture,ActiveBackgroundImage,InstructorId")] InstructorImage instructorImage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(instructorImage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InstructorId = new SelectList(db.Instructors, "Id", "FirstName", instructorImage.InstructorId);
            return View(instructorImage);
        }

        // GET: InstructorImages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstructorImage instructorImage = db.InstructorImages.Find(id);
            if (instructorImage == null)
            {
                return HttpNotFound();
            }
            return View(instructorImage);
        }

        // POST: InstructorImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InstructorImage instructorImage = db.InstructorImages.Find(id);
            db.InstructorImages.Remove(instructorImage);
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
        [HttpGet]
        public ActionResult AddPicture()
        {

            return View();
        }
        [HttpPost]
        public ActionResult AddPicture(InstructorImage imageModel, HttpPostedFileBase ImageFile)
        {
            int InstructorId = int.Parse(Session["InstructorId"].ToString());
            String fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
            String extension = Path.GetExtension(imageModel.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            imageModel.ProfilePictureURL = "~/Resources/Image/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Resources/Image/"), fileName);
            imageModel.ImageFile.SaveAs(fileName);
            var image = db.InstructorImages.Where(x => x.InstructorId == InstructorId);
            if (image.Any())
            {
                var images = image.FirstOrDefault();

                db.Entry(images).State = EntityState.Modified;
                db.SaveChanges();
            }
            using (OSAAEntities db = new OSAAEntities())
            {

                imageModel.InstructorId = InstructorId;
                imageModel.UploadDate = DateTime.Now;
                db.InstructorImages.Add(imageModel);
                db.SaveChanges();

                var Image = db.InstructorImages.Where(x => x.InstructorId == InstructorId);
                if (Image.Any())
                {
                    var Images = Image.FirstOrDefault();
                    int userid = Convert.ToInt32(Session["AdminProfilePictureURL"]);
                    var insr = db.InstructorImages.Find(userid);

                    Session["ProfilePictureURL"] = Images.ProfilePictureURL;
                    Session["InstructorProfilePictureURL"] = Images.ProfilePictureURL;
                    Session["AdminProfilePictureURL"] = Images.ProfilePictureURL;
                }

                return RedirectToAction("ChangePicture", "InstructorImages");

            }
        }
        [HttpGet]
        public ActionResult ChangePicture()
        {
            OSAAEntities db = new OSAAEntities();
            int id = int.Parse(Session["InstructorId"].ToString());


            var imageModel = db.InstructorImages.Where(x => x.InstructorId == id).FirstOrDefault();

            return View(imageModel);
        }
        // POST: Profile/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePicture(InstructorImage imageModel)
        {
            int InstructorId = int.Parse(Session["InstructorId"].ToString());

            if (ModelState.IsValid)
            {

                OSAAEntities db = new OSAAEntities();

                String fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
                String extension = Path.GetExtension(imageModel.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                imageModel.ProfilePictureURL = "~/Resources/Image/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Resources/Image/"), fileName);
                imageModel.ImageFile.SaveAs(fileName);

                db.Entry(imageModel).State = EntityState.Modified;

                db.SaveChanges();
                var Image = db.InstructorImages.Where(x => x.InstructorId == InstructorId);
                if (Image.Any())
                {

                    imageModel.UploadDate = DateTime.Now;
                    var Images = Image.FirstOrDefault();
                    Session["HeadProfilePictureURL"] = Images.ProfilePictureURL;
                    Session["InstructorProfilePictureURL"] = Images.ProfilePictureURL;
                    Session["AdminProfilePictureURL"] = Images.ProfilePictureURL;
                }

                return RedirectToAction("ChangePicture", "InstructorImages");


            }

            return View(imageModel);

        }
    }
}
    

