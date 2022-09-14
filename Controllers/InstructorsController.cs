using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using OnlineStudentAcademicAdvisory.Models;

namespace OnlineStudentAcademicAdvisory.Controllers
{
    public class InstructorsController : Controller
    {
        private OSAAEntities db = new OSAAEntities();

        // GET: Instructors
        public ActionResult Index()
        {
            var instructors = db.Instructors.Include(i => i.Department);
            return View(instructors.ToList());
        }
        public ActionResult DepartmentInstructors(string searchingString)
        {
            int currentInstructor = int.Parse(Session["DepartmentId"].ToString());
            var instructors = db.Instructors.Include(i => i.Department).Where(f => f.DepartmentId == currentInstructor);
            if (!string.IsNullOrEmpty(searchingString))
            {
                instructors = instructors.Where(i => i.FirstName.Contains(searchingString));
            }
            return View(instructors);
        }


        // GET: Instructors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // GET: Instructors/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentName");
            return View();
        }

        // POST: Instructors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,MiddleName,LastName,DepartmentId,PhoneNumber,Email,Username,Password,ResetPasswordCode")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                db.Instructors.Add(instructor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentName", instructor.DepartmentId);
            return View(instructor);
        }

        // GET: Instructors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentName", instructor.DepartmentId);
            return View(instructor);
        }

        // POST: Instructors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,MiddleName,LastName,DepartmentId,PhoneNumber,Email,Username,Password,ResetPasswordCode")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(instructor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentName", instructor.DepartmentId);
            return View(instructor);
        }

        // GET: Instructors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // POST: Instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Instructor instructor = db.Instructors.Find(id);
            db.Instructors.Remove(instructor);
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
        public ActionResult AddInstructor()
        {

            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentName");
            ViewBag.AcademicInstitutionId = new SelectList(db.AcademicInstutions, "Id", "InstitutionName");
            ViewBag.EducationLevelId = new SelectList(db.EducationLevels, "Id", "LevelName");
            ViewBag.FieldOfStudiesId = new SelectList(db.FieldOfStudies, "Id", "FieldName");
            ViewBag.InstructorId = new SelectList(db.Instructors, "Id", "FirstName");
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "RoleName");

            return View();
        }

        [HttpPost]
        public ActionResult AddInstructor
            (
            string FirstName, string MiddleName, string LastName, int DepartmentId, 
            string PhoneNumber, string Email, string Username, string Password, 
             int AcademicInstitutionId, int EducationLevelId,
            int FieldOfStudiesId, DateTime GraduationYear, int RoleId
            )
        {
            AcademicInformation academicInformation = new AcademicInformation();
            Instructor ins = new Instructor();
            InstractorRole instractorRole = new InstractorRole();
            AcademicInformation AcademicInfo = new AcademicInformation();
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "RoleName", instractorRole.RoleId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentName", ins.DepartmentId);
            ins.FirstName = FirstName;
            ins.MiddleName = MiddleName;
            ins.LastName = LastName;
            ins.Username = Username;
            ins.Password = Encrypt(Password);
            ins.Email = Email;
            ins.DepartmentId = DepartmentId;
            ins.PhoneNumber = PhoneNumber;
            db.Instructors.Add(ins);
            db.SaveChanges();

            AcademicInfo.AcademicInstitutionId = AcademicInstitutionId;
            AcademicInfo.EducationLevelId = EducationLevelId;
            AcademicInfo.FieldOfStudiesId = FieldOfStudiesId;
            AcademicInfo.InstructorId = ins.Id;
            AcademicInfo.GaduationYear = GraduationYear;
            db.AcademicInformations.Add(AcademicInfo);
            db.SaveChanges();

            instractorRole.RoleId = RoleId;
            instractorRole.InstractorId = ins.Id;
            db.InstractorRoles.Add(instractorRole);
            db.SaveChanges();
            TempData["Added"] = "Successfully Added!";
            ViewBag.AcademicInstitutionId = new SelectList(db.AcademicInstutions, "Id", "InstitutionName", academicInformation.AcademicInstitutionId);
            ViewBag.EducationLevelId = new SelectList(db.EducationLevels, "Id", "LevelName", academicInformation.EducationLevelId);
            ViewBag.FieldOfStudiesId = new SelectList(db.FieldOfStudies, "Id", "FieldName", academicInformation.FieldOfStudiesId);
            ViewBag.InstructorId = new SelectList(db.Instructors, "Id", "FirstName", academicInformation.InstructorId);
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "RoleName", instractorRole.RoleId);

            return RedirectToAction("Index");
        }

        public ActionResult DisableInstructor( int? id)
        {
            Instructor instructor = db.Instructors.Find(id);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (instructor == null)
            {
                return HttpNotFound();
            }
            else if (ModelState.IsValid)
            {
                if(instructor.Disabled == false)
                {
                    instructor.Disabled = true;
                }
                else if (instructor.Disabled == true)
                {
                    instructor.Disabled = false;
                }
                
                db.Entry(instructor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           
            return RedirectToAction("Index");
        }
        private string Encrypt(string Password)
        {
            using (var md5Hash = MD5.Create())
            {
                var sourceBytes = Encoding.UTF8.GetBytes(Password);

                var hashBytes = md5Hash.ComputeHash(sourceBytes);

                return BitConverter.ToString(hashBytes).Replace("-", string.Empty);


            }
        }
    }
}