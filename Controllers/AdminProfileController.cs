using OnlineStudentAcademicAdvisory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineStudentAcademicAdvisory.Controllers
{
    public class AdminProfileController : Controller
    {
        // GET: AdminProfile
        public ActionResult Index()
        {
            return View();
        }
        public new ActionResult View()
        {
            OSAAEntities db = new OSAAEntities();

            SystemAdministratorInformation imageModel = new SystemAdministratorInformation();
            int id = int.Parse(Session["SystemAdminId"].ToString());

            imageModel = db.SystemAdministratorInformations.Where(x => x.Id == id).FirstOrDefault();

            return View(imageModel);


        }
    }
}