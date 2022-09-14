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
    public class FinalProjectsController : Controller
    {
        private OSAAEntities db = new OSAAEntities();

        // GET: FinalProjects
        public ActionResult Index()
        {
            var finalProjects = db.FinalProjects.Include(f => f.Student);
            return View(finalProjects.ToList());
        }

        // GET: FinalProjects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FinalProject finalProject = db.FinalProjects.Find(id);
            if (finalProject == null)
            {
                return HttpNotFound();
            }
            return View(finalProject);
        }
      

        // GET: FinalProjects/Create
        public ActionResult Create()
        {
            ViewBag.CreatedBy = new SelectList(db.Students, "Id", "IdNumber");
            return View();
        }

        // POST: FinalProjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProjectTitle,Description,CreatedBy,IsActive,NoOfStudents,IsTerminated")] FinalProject finalProject)
        {
            if (ModelState.IsValid)
            {
                db.FinalProjects.Add(finalProject);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CreatedBy = new SelectList(db.Students, "Id", "IdNumber", finalProject.CreatedBy);
            return View(finalProject);
        }

        // GET: FinalProjects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FinalProject finalProject = db.FinalProjects.Find(id);
            if (finalProject == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatedBy = new SelectList(db.Students, "Id", "IdNumber", finalProject.CreatedBy);
            return View(finalProject);
        }

        // POST: FinalProjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProjectTitle,Description,CreatedBy,IsActive,NoOfStudents,IsTerminated")] FinalProject finalProject)
        {
            if (ModelState.IsValid)
            {
                db.Entry(finalProject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreatedBy = new SelectList(db.Students, "Id", "IdNumber", finalProject.CreatedBy);
            return View(finalProject);
        }

        // GET: FinalProjects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FinalProject finalProject = db.FinalProjects.Find(id);
            if (finalProject == null)
            {
                return HttpNotFound();
            }
            return View(finalProject);
        }

        // POST: FinalProjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FinalProject finalProject = db.FinalProjects.Find(id);
            db.FinalProjects.Remove(finalProject);
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
        public ActionResult ActivateGroup()
        {

            var finalProjects = db.FinalProjects;
            return View(finalProjects.ToList());
        }
        public ActionResult EditIsActive(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FinalProject finalProject = db.FinalProjects.Find(id);
            if (finalProject == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatedBy = new SelectList(db.Students, "Id", "FirstName", finalProject.CreatedBy);
            return View(finalProject);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditIsActive([Bind(Include = "Id,ProjectTitle,Description,CreatedBy,IsActive,NoOfStudents")] FinalProject finalProject)
        {
            if (ModelState.IsValid)
            {
                db.Entry(finalProject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ActivateGroup");
            }
            ViewBag.CreatedBy = new SelectList(db.Students, "Id", "FirstName", finalProject.CreatedBy);
            return View(finalProject);
        }
        public ActionResult StudentDetails(int? id)
        {
            FinalProjectStudent Project = new FinalProjectStudent();
            FinalProjectAdvisor Instructor = new FinalProjectAdvisor();
            Session["ProjectId"] = id;
            ViewBag.studentDocument = db.StudentDocuments.Where(x => x.FinalProjectStudent.FinalProjectId == id).FirstOrDefault();
            ViewBag.advisorDocument = db.AdvisorDocuments.Where(x => x.FinalProjectAdvisor.FinalProjectId == id).FirstOrDefault();
            ViewData["doc"] = TempData["Document"];
            ViewBag.StudentId = new SelectList(db.Students, "Id", "FirstName", Project.StudentId);
            ViewBag.InstructorId = new SelectList(db.Instructors, "Id", "FirstName", Instructor.InstractorId);


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FinalProject finalProject = db.FinalProjects.Find(id);//used when the return parameter is final project, so uncomment when project chat model is removed
            if (finalProject == null)
            {
                return HttpNotFound();
            }
            return View(finalProject);
        }
        [HttpPost]
        public ActionResult StudentAddFile(int ProjectId, int ProjectStudentId, string uploadDate, string message, string attachement, HttpPostedFileBase file)
        {

            StudentDocument document = new StudentDocument();
            string path = uploadfile(file);

            document.Attachement = path;
            if (file == null)
            {
                document.DocumentTitle = null;
            }
            else
            {
                document.DocumentTitle = file.FileName;

            }
            document.UploadDate = DateTime.Now;
            if (message != null && message != "")
            {
            document.Message = message;

            }
            else
            {
                document.Message = "-1";
            }

            document.ProjectStudentId = ProjectStudentId;
            db.StudentDocuments.Add(document);
            db.SaveChanges();
            ViewBag.msg = "data added...";




            return RedirectToAction("StudentDetails", new { id = ProjectId });
        }
        public string uploadfile(HttpPostedFileBase f)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (f != null && f.ContentLength > 0)
            {
                string fileName = Path.GetFileNameWithoutExtension(f.FileName);
                string extension = Path.GetExtension(f.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;

                if (extension.ToLower().Equals(".pdf") || extension.ToLower().Equals(".jpg") || extension.ToLower().Equals("jpeg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {
                        path = Path.Combine(Server.MapPath("~/Resources/Uploaded_File"), Path.GetFileName(f.FileName));
                        f.SaveAs(path);
                        path = "~/Resources/Uploaded_File/" + Path.GetFileName(f.FileName);
                        if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals("jpeg") || extension.ToLower().Equals(".png"))
                        {
                            ViewBag.msg = "image";
                        }
                        else
                        {
                            ViewBag.msg = "Document";
                        }
                        // viewBag.Message = "File uploaded successfully";
                    }
                    catch (Exception ex)
                    {
                        path = "-1";
                    }
                }
                else
                {
                    Response.Write("No file");
                }
            }
            else
            {
                Response.Write("No file");
            }
            return path;
        }


        public FileResult DownloadFile(string Name)

        {
            StudentDocument document = new StudentDocument();
            string path = Server.MapPath(Name);
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            return File(bytes, "application/octet-stream", Name);

        }
        public ActionResult InstructorDetails(int? id)
        {
            ViewBag.studentDocument = db.StudentDocuments.Where(x => x.FinalProjectStudent.FinalProjectId == id).FirstOrDefault();
            ViewBag.advisorDocument = db.AdvisorDocuments.Where(x => x.FinalProjectAdvisor.FinalProjectId == id).FirstOrDefault();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FinalProject finalProject = db.FinalProjects.Find(id);
            if (finalProject == null)
            {
                return HttpNotFound();
            }
            return View(finalProject);
        }

        public ActionResult InstructorAddFile(int ProjectId, int ProjectAdvisorId, string uploadDate, string message, string attachement, HttpPostedFileBase file)
        {

            AdvisorDocument document = new AdvisorDocument();
            string path = uploadfile(file);

            document.Attachement = path;
            if (file == null)
            {
                document.DocumentTitle = null;
            }
            else
            {
                document.DocumentTitle = file.FileName;

            }
            document.UploadDate = DateTime.Now;
            if (message != null && message != "")
            {
                document.Message = message;

            }
            else
            {
                document.Message = "-1";
            }

            document.ProjectAdvisorId = ProjectAdvisorId;
            db.AdvisorDocuments.Add(document);
            db.SaveChanges();
            ViewBag.msg = "data added...";




            return RedirectToAction("InstructorDetails", new { id = ProjectId });
        }
        public ActionResult Membership()
        {
            int currentStudent = int.Parse(Session["StudentId"].ToString());
            var membership = db.FinalProjectStudents.Where(f => f.StudentId == currentStudent && f.FinalProject.IsTerminated == false);// returns the activwgroups where this individual is made to be a member
            return View(membership);

        }
        public ActionResult ActiveProjects()
        {
            var project = db.FinalProjects.FirstOrDefault();

            ViewBag.count = db.FinalProjectStudents.Where(g => g.FinalProjectId == project.Id).Count();
            ViewBag.Members = db.FinalProjectStudents.GroupBy(s => s.StudentId)
                                .Where(grp => grp.Count() > 1)
                                .Select(grp => new
                                {
                                    NoOfStudents = grp.Count(),
                                    FinalprojectId = grp.Key
                                }).ToList();
            int CurrentStudent = int.Parse(Session["StudentId"].ToString());
            var finalProjects = db.FinalProjects.Where(f => f.CreatedBy == CurrentStudent && f.IsActive == true && f.IsTerminated == false);
            return View(finalProjects.ToList());

        }
        public ActionResult PreviousProjects()
        {

            //ViewBag.Members = db.FinalProjectStudents.GroupBy(s => s.StudentId)
            //                    .Where(grp => grp.Count() > 1)
            //                    .Select(grp => new
            //                    {
            //                        NoOfStudents = grp.Count(),
            //                        FinalprojectId = grp.Key
            //                    }).ToList();
            int CurrentStudent = int.Parse(Session["StudentId"].ToString());
            var finalProjects = db.FinalProjectStudents.Where(f => f.StudentId == CurrentStudent && f.FinalProject.IsTerminated == true);
            return View(finalProjects.ToList());
        }
        [HttpGet]
        public ActionResult SelectAdvisor()
        {
            ViewBag.FinalProjectAdvisorId = new SelectList(db.FinalProjectAdvisors, "Id", "FirstName", "MiddleName");
            return View();
        }
        [HttpPost]
      
        public ActionResult SelectAdvisor(int ProjectId, int instructorId)
        {

            Instructor instructor = new Instructor();
            FinalProjectAdvisor Assigned = new FinalProjectAdvisor();
            ViewBag.InstractorId = new SelectList(db.Instructors, "Id", "FirstName", "MiddleName", instructor.Id);
            var finalProjectAdvisor = db.FinalProjectAdvisors.Where(f => f.FinalProjectId == ProjectId);
            if (finalProjectAdvisor.Any())
            {
                return View("OverAllError");

            }
            else
            {
                Assigned.InstractorId = instructorId;
                Assigned.AdvisorTypeId = 1;
                Assigned.FinalProjectId = ProjectId;

                db.FinalProjectAdvisors.Add(Assigned);
                db.SaveChanges();
                return RedirectToAction("StudentDetails", "FinalProjects", new { id = ProjectId });

            }

        }
       
        public ActionResult TerminateProject(int id)
        {
                FinalProject finalProject = db.FinalProjects.Find(id);

            if (ModelState.IsValid)
            {
                finalProject.IsTerminated = true;

                db.Entry(finalProject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AdvisorOngoingProjects", "FinalProjectAdvisors");
            }
            ViewBag.CreatedBy = new SelectList(db.Students, "Id", "FirstName", finalProject.CreatedBy);
            return View(finalProject);
        }

    }
}
