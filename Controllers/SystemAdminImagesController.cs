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
    public class SystemAdminImagesController : Controller
    {
        private OSAAEntities db = new OSAAEntities();

        // GET: SystemAdminImages
        public ActionResult Index()
        {
            var systemAdminImages = db.SystemAdminImages.Include(s => s.SystemAdministratorInformation);
            return View(systemAdminImages.ToList());
        }

        // GET: SystemAdminImages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemAdminImage systemAdminImage = db.SystemAdminImages.Find(id);
            if (systemAdminImage == null)
            {
                return HttpNotFound();
            }
            return View(systemAdminImage);
        }

        // GET: SystemAdminImages/Create
        public ActionResult Create()
        {
            ViewBag.SystemAdminId = new SelectList(db.SystemAdministratorInformations, "Id", "FirstName");
            return View();
        }

        // POST: SystemAdminImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BackgroundImage,UploadDate,ProfilepictureURL,ActiveProfilePicture,ActiveBackgroundImage,SystemAdminId")] SystemAdminImage systemAdminImage)
        {
            if (ModelState.IsValid)
            {
                db.SystemAdminImages.Add(systemAdminImage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SystemAdminId = new SelectList(db.SystemAdministratorInformations, "Id", "FirstName", systemAdminImage.SystemAdminId);
            return View(systemAdminImage);
        }

        // GET: SystemAdminImages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemAdminImage systemAdminImage = db.SystemAdminImages.Find(id);
            if (systemAdminImage == null)
            {
                return HttpNotFound();
            }
            ViewBag.SystemAdminId = new SelectList(db.SystemAdministratorInformations, "Id", "FirstName", systemAdminImage.SystemAdminId);
            return View(systemAdminImage);
        }

        // POST: SystemAdminImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BackgroundImage,UploadDate,ProfilepictureURL,ActiveProfilePicture,ActiveBackgroundImage,SystemAdminId")] SystemAdminImage systemAdminImage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(systemAdminImage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SystemAdminId = new SelectList(db.SystemAdministratorInformations, "Id", "FirstName", systemAdminImage.SystemAdminId);
            return View(systemAdminImage);
        }

        // GET: SystemAdminImages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemAdminImage systemAdminImage = db.SystemAdminImages.Find(id);
            if (systemAdminImage == null)
            {
                return HttpNotFound();
            }
            return View(systemAdminImage);
        }

        // POST: SystemAdminImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SystemAdminImage systemAdminImage = db.SystemAdminImages.Find(id);
            db.SystemAdminImages.Remove(systemAdminImage);
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
        public ActionResult AddPicture(SystemAdminImage imageModel, HttpPostedFileBase ImageFile)
        {
            int SystemAdminId = int.Parse(Session["SystemAdminId"].ToString());
            String fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
            String extension = Path.GetExtension(imageModel.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            imageModel.ProfilePictureURL = "~/Resources/Image/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Resources/Image/"), fileName);
            imageModel.ImageFile.SaveAs(fileName);
            var image = db.SystemAdminImages.Where(x => x.SystemAdminId == SystemAdminId);
            if (image.Any())
            {
                var images = image.FirstOrDefault();

                db.Entry(images).State = EntityState.Modified;
                db.SaveChanges();
            }
            using (OSAAEntities db = new OSAAEntities())
            {

                imageModel.SystemAdminId = SystemAdminId;
                imageModel.UploadDate = DateTime.Now;
                db.SystemAdminImages.Add(imageModel);
                db.SaveChanges();

                var Image = db.SystemAdminImages.Where(x => x.SystemAdminId == SystemAdminId && x.ActiveProfilePicture == true);
                if (Image.Any())
                {
                    var Images = Image.FirstOrDefault();
                    Session["AdminProfilePictureURL"] = Images.ProfilePictureURL;
                }

                return RedirectToAction("ChangePicture", "SystemAdminImages");

            }
        }
        [HttpGet]
        public ActionResult ChangePicture()
        {
            OSAAEntities db = new OSAAEntities();
            int id = int.Parse(Session["SystemAdminId"].ToString());


            var imageModel = db.SystemAdminImages.Where(x => x.SystemAdminId == id).FirstOrDefault();

            return View(imageModel);
        }
        // POST: Profile/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePicture(SystemAdminImage imageModel)
        {
            int SystemAdminId = int.Parse(Session["SystemAdminId"].ToString());

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
                var Image = db.SystemAdminImages.Where(x => x.SystemAdminId == SystemAdminId);
                if (Image.Any())
                {

                    imageModel.UploadDate = DateTime.Now;
                    var Images = Image.FirstOrDefault();
                    Session["AdminProfilePictureURL"] = Images.ProfilePictureURL;
                }

                return RedirectToAction("Index", "Home");


            }

            return View(imageModel);

        }
    }
}
