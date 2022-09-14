using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineStudentAcademicAdvisory.Models;

namespace OnlineStudentAcademicAdvisory.Controllers
{
    public class StudentProfileController : Controller
    {
        // GET: StudentProfile
        [HttpGet]
        public ActionResult Index()
        {
            OSAAEntities db = new OSAAEntities();


            var Student = db.Students.ToList();

            return View(Student);
        }

        [HttpGet]
        public new ActionResult View()
        {
            OSAAEntities db = new OSAAEntities();

            StudentImage studentImage = new StudentImage();
            int id = int.Parse(Session["StudentId"].ToString());
            studentImage = db.StudentImages.Where(f => f.StudentId == id).FirstOrDefault();
            return View(studentImage);


        }
    }
}