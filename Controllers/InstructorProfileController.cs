using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineStudentAcademicAdvisory.Models;

namespace OnlineStudentAcademicAdvisory.Controllers
{
    public class InstructorProfileController : Controller
    {
        // GET: InstructorProfile
        public ActionResult Index()
        {

            OSAAEntities db = new OSAAEntities();


            var Instructor = db.Instructors.ToList();

            return View(Instructor);
            
        }
        [HttpGet]
        public new ActionResult View()
        {
            OSAAEntities db = new OSAAEntities();

            //Instructor imageModel = new Instructor();
            //int id = int.Parse(Session["InstructorId"].ToString());

            //imageModel = db.Instructors.Where(x => x.Id == id).FirstOrDefault();

            //return View(imageModel);

            InstructorImage instructortImage = new InstructorImage();
            int id = int.Parse(Session["InstructorId"].ToString());
            instructortImage = db.InstructorImages.Where(f => f.InstructorId == id).FirstOrDefault();
            return View(instructortImage);


        }
    }
}