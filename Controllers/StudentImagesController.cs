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
    public class StudentImagesController : Controller
    {
        private OSAAEntities db = new OSAAEntities();

        // GET: StudentImages
        public ActionResult Index()
        {
            var studentImages = db.StudentImages.Include(s => s.Student);
            return View(studentImages.ToList());
        }

        // GET: StudentImages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentImage studentImage = db.StudentImages.Find(id);
            if (studentImage == null)
            {
                return HttpNotFound();
            }
            return View(studentImage);
        }

        // GET: StudentImages/Create
        public ActionResult Create()
        {
            ViewBag.StudentId = new SelectList(db.Students, "Id", "IdNumber");
            return View();
        }

        // POST: StudentImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BackgroundImage,ProfilePictureURL,StudentId,UplaodDate,ActiveProfilePicture,ActiveBackgroundImage")] StudentImage studentImage)
        {
            if (ModelState.IsValid)
            {
                db.StudentImages.Add(studentImage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StudentId = new SelectList(db.Students, "Id", "IdNumber", studentImage.StudentId);
            return View(studentImage);
        }

        // GET: StudentImages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentImage studentImage = db.StudentImages.Find(id);
            if (studentImage == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentId = new SelectList(db.Students, "Id", "IdNumber", studentImage.StudentId);
            return View(studentImage);
        }

        // POST: StudentImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BackgroundImage,ProfilePictureURL,StudentId,UplaodDate,ActiveProfilePicture,ActiveBackgroundImage")] StudentImage studentImage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentImage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StudentId = new SelectList(db.Students, "Id", "IdNumber", studentImage.StudentId);
            return View(studentImage);
        }

        // GET: StudentImages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentImage studentImage = db.StudentImages.Find(id);
            if (studentImage == null)
            {
                return HttpNotFound();
            }
            return View(studentImage);
        }

        // POST: StudentImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentImage studentImage = db.StudentImages.Find(id);
            db.StudentImages.Remove(studentImage);
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
        public ActionResult AddPicture(StudentImage imageModel, HttpPostedFileBase ImageFile)
        {
            int StudentId = int.Parse(Session["StudentId"].ToString());
            String fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
            String extension = Path.GetExtension(imageModel.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            imageModel.ProfilePictureURL = "~/Resources/Image/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Resources/Image/"), fileName);
            imageModel.ImageFile.SaveAs(fileName);
            var image = db.StudentImages.Where(x => x.StudentId == StudentId);
            if (image.Any())
            {
                var images = image.FirstOrDefault();

                db.Entry(images).State = EntityState.Modified;
                db.SaveChanges();

            }
            using (OSAAEntities db = new OSAAEntities())
            {

                imageModel.StudentId = StudentId;
                imageModel.UplaodDate = DateTime.Now;
                db.StudentImages.Add(imageModel);
                db.SaveChanges();

                var Image = db.StudentImages.Where(x => x.StudentId == StudentId );
                if (Image.Any())
                {
                    var Images = Image.FirstOrDefault();
                    Session["ProfilePictureURL"] = Images.ProfilePictureURL;
                }

                return RedirectToAction("Index", "Home");

            }
        }
        [HttpGet]
        public ActionResult ChangePicture()
        {
            OSAAEntities db = new OSAAEntities();
            int id = int.Parse(Session["StudentId"].ToString());


            var imageModel = db.StudentImages.Where(x => x.StudentId == id).FirstOrDefault();

            return View(imageModel);
        }
        // POST: Profile/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePicture(StudentImage imageModel)
        {
            int StudentId = int.Parse(Session["StudentId"].ToString());

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
                var Image = db.StudentImages.Where(x => x.StudentId == StudentId);
                if (Image.Any())
                {

                    imageModel.UplaodDate = DateTime.Now;
                    var Images = Image.FirstOrDefault();
                    Session["ProfilePictureURL"] = Images.ProfilePictureURL;
                }

                return RedirectToAction("View", "StudentProfile");


            }

            return View(imageModel);

        }

        [HttpGet]
        public ActionResult Add()
        {

            return View();
        }
        [HttpPost]

        public ActionResult Add(StudentImage imageModel)
        {
            int StudentId = int.Parse(Session["StudentId"].ToString());

            String fileName = Path.GetFileNameWithoutExtension(imageModel.BackgroundsImage.FileName);
            String extension = Path.GetExtension(imageModel.BackgroundsImage.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            imageModel.BackgroundImage = "~/Resources/Image/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Resources/Image/"), fileName);
            imageModel.BackgroundsImage.SaveAs(fileName);
            var image = db.StudentImages.Where(x => x.StudentId == StudentId);
            if (image.Any())
            {
                var images = image.FirstOrDefault();

                db.Entry(images).State = EntityState.Modified;
                db.SaveChanges();
            }
            using (OSAAEntities db = new OSAAEntities())
            {

                imageModel.StudentId = StudentId;

                imageModel.UplaodDate = DateTime.Now;
                db.StudentImages.Add(imageModel);
                db.SaveChanges();
                var Image = db.StudentImages.Where(x => x.StudentId == StudentId);
                if (Image.Any())
                {
                    var Images = Image.FirstOrDefault();
                    Session["BackgroundImage"] = Images.BackgroundImage;
                }

                return RedirectToAction("View", "StudentProfile");
            }


        }



        [HttpGet]
        public ActionResult BackgroundEdit()
        {
            OSAAEntities db = new OSAAEntities();

            //BackgroundPicture imageModel = new BackgroundPicture();
            int id = int.Parse(Session["StudentId"].ToString());

            var imageModel = db.StudentImages.Where(x => x.StudentId == id).FirstOrDefault();

            return View(imageModel);


        }

        // POST: Profile/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult BackgroundEdit(StudentImage imageModel, HttpPostedFileBase BackgroundImage)
        {
            int id = int.Parse(Session["StudentId"].ToString());

            if (ModelState.IsValid)
            {

                OSAAEntities db = new OSAAEntities();
                String fileName = Path.GetFileNameWithoutExtension(imageModel.BackgroundsImage.FileName);
                String extension = Path.GetExtension(imageModel.BackgroundsImage.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                imageModel.BackgroundImage = "~/Resources/Image/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Resources/Image/"), fileName);
                imageModel.BackgroundsImage.SaveAs(fileName);

                db.Entry(imageModel).State = EntityState.Modified;

                db.SaveChanges();
                var Image = db.StudentImages.Where(x => x.StudentId == id);
                if (Image.Any())
                {
                    imageModel.UplaodDate = DateTime.Now;
                    var Images = Image.FirstOrDefault();
                    Session["BackgroundImage"] = Images.BackgroundImage;
                }
                return RedirectToAction("View", "StudentProfile");
            }

            return View(imageModel);


        }


    }
}
