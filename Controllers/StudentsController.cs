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
    public class StudentsController : Controller
    {
        private OSAAEntities db = new OSAAEntities();

        // GET: Students
        public ActionResult Index()
        {
            return View(db.Students.ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdNumber,FirstName,MiddleName,LastName,Username,Password,Sex,Email")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdNumber,FirstName,MiddleName,LastName,Username,Password,Sex,Email")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
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
        public ActionResult InstructorsList()
        {
            return View(db.FinalProjectAdvisors.ToList());

        }
        [HttpGet]
        public ActionResult CreateGroup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateGroup(string ProjectTitle, string Description)
        {

            int CreatedBy = int.Parse(Session["StudentId"].ToString());

            FinalProject Pro = new FinalProject();
            FinalProjectStudent firstMember = new FinalProjectStudent();


            Pro.CreatedBy = CreatedBy;
            Pro.ProjectTitle = ProjectTitle;
            Pro.Description = Description;
            Pro.NoOfStudents = 1;
            Pro.IsActive = false;

            db.FinalProjects.Add(Pro);
            db.SaveChanges();
            firstMember.StudentId = CreatedBy;
            firstMember.FinalProjectId = Pro.Id;
            db.FinalProjectStudents.Add(firstMember);
            db.SaveChanges();
            TempData["Success"] = "Succesful";
            return RedirectToAction("Index", "Home");
        }
     
        public ActionResult AddMembers(int ProjectId, int StudentId)
        {
            FinalProjectStudent Project = new FinalProjectStudent();

            var finalProject = db.FinalProjects.Where(f => f.Id == ProjectId);
            var finalPeojectDetail = finalProject.FirstOrDefault();
            int NoOfMember = (int)finalPeojectDetail.NoOfStudents;
           
            Project.StudentId = StudentId;
            Project.FinalProjectId = ProjectId;
            db.FinalProjectStudents.Add(Project);
            db.SaveChanges();
            finalPeojectDetail.NoOfStudents = NoOfMember + 1;
            db.Entry(finalPeojectDetail).State = EntityState.Modified;
            db.SaveChanges();

            ViewBag.StudentId = new SelectList(db.Students, "Id", "FirstName", Project.StudentId);
            
            return RedirectToAction("StudentDetails", "FinalProjects", new { id = ProjectId });

        }
        public ActionResult StudentList()
        {
            int department = int.Parse(Session["DepartmentId"].ToString());
            Instructor instructor = new Instructor();
            var List = db.StudentBatches.Where(f => f.Batch.DepartmentId == department).ToList();


            return View(List);
        }
        public ActionResult DepartmentInformation(int? id)
        {
            Instructor Ins = new Instructor();
            Department department = new Department();
            var dept = db.Departments.Where(d => d.DepartmentName == "Electrical");

            int InstructorDepartment = int.Parse(Session["DepartmentId"].ToString());
            var studentsInDepartment = db.StudentBatches.Where(f => f.Batch.DepartmentId == InstructorDepartment);
            var instructor = db.Instructors.Where(f => f.DepartmentId == InstructorDepartment);
          
            List<StudentBatch> studentBatch = new List<StudentBatch>();
            studentBatch = studentsInDepartment.ToList();
            List<Instructor> instructors = new List<Instructor>();
            instructors = instructor.ToList();
            DepartmentInfo dif = new DepartmentInfo();
            dif.instructor = instructors;
            dif.studentBatch = studentBatch;
            return View(dif);
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
        public ActionResult AddStudent()
        {
            if(Session["SystemAdminId"] == null)
            {
                TempData["Error"] = "Unauthorized action!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
            ViewBag.AdmissionId = new SelectList(db.AdmissionTypes, "Id", "AdmissionTypeName");
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentName");
            ViewBag.ProgramId = new SelectList(db.Programs, "Id", "ProgramName");
            return View();
            }
           
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddStudent( string IdNumber, string FirstName,string MiddleName,string LastName,string Password,string Sex,string Email,int ProgramId,int AdmissionId, int DepartmentId, string EntryYear, int Section, string PhoneNumber)
        {
            var student = new Student();
            var studentBatchInfo = new StudentBatch();
            var batchData = new Batch();
            var studentImage = new StudentImage();


            if (ModelState.IsValid)
            {
                student.IdNumber = IdNumber;
                student.FirstName = FirstName;
                student.MiddleName = MiddleName;
                student.LastName = LastName;
                student.Password = Encrypt(Password);
                student.Sex = Sex;
                student.Email = Email;
                student.PhoneNumber = PhoneNumber;

                db.Students.Add(student);
                db.SaveChanges();

                batchData.ProgramId = ProgramId;
                batchData.AdmissionId = AdmissionId;
                batchData.DepartmentId = DepartmentId;
                batchData.EntryYear = DateTime.Now;
                db.Batches.Add(batchData);
                db.SaveChanges();


                studentBatchInfo.StudentId = student.Id;
                studentBatchInfo.BatchId = batchData.Id;
                studentBatchInfo.Section = Section.ToString();
                db.StudentBatches.Add(studentBatchInfo);
                db.SaveChanges();
                studentImage.StudentId = student.Id;
                studentImage.ProfilePictureURL = "~/Resources/assets/Blue-back.png";
                studentImage.UplaodDate = DateTime.Now;
                db.StudentImages.Add(studentImage);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.AdmissionId = new SelectList(db.AdmissionTypes, "Id", "AdmissionTypeName", AdmissionId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentName", DepartmentId);
            ViewBag.ProgramId = new SelectList(db.Programs, "Id", "ProgramName", ProgramId);
            return View(student);
        }
      
        

    }
}
