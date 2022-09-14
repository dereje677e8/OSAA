using OnlineStudentAcademicAdvisory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace OnlineStudentAcademicAdvisory.Controllers
{
    public class HomeController : Controller
    {
        private OSAAEntities db = new OSAAEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult LogIn()
        {
            return View();
        }
        public ActionResult StudentLogIn(String IdNumber, String Password)
        {

            var student = db.Students.Where(x => x.IdNumber== IdNumber);

            var Batches = db.Batches.FirstOrDefault();

            if (student.Any())
            {
                StudentImage studentImages = new StudentImage();

                var stu = student.FirstOrDefault();
                var studentImage = db.StudentImages.Where(f => f.StudentId == stu.Id);

                if (stu.Password == Encrypt(Password))
                {
                    Session["StudentId"] = stu.Id;
                    Session["IdNumber"] = stu.IdNumber;
                    Session["FirstName"] = stu.FirstName;
                    Session["MiddleName"] = stu.MiddleName;
                    Session["Department"] = Batches.Department.DepartmentName;
                    if(studentImage.Any())
                    {
                        var image = studentImage.FirstOrDefault();
                        //Session["ProfilePictureURL"] = "~/Resources/Images/new222400839.jpg";
                        Session["ProfilePictureURL"] = image.ProfilePictureURL.ToString();

                    }
                    else
                    {
                        //Session["ProfilePictureURL"] = studentImage.;
                        Session["ProfilePictureURL"] = "~/Resources/Image/new222400839.jpg";
                    }

               

                    Session["StudentLogin"] = "Student";
                    ViewBag.Username = Session["Username"];
                    TempData["Success"] = "Welcome to our system!";

                    return RedirectToAction("Index", "Home");// Index implies the first front page for student
                }
                else
                {
                    TempData["Error"] = "Incorrect Password";

                }
            }

           

            else
            {
                TempData["Error"] = "Incorrect Username";

            }


            return RedirectToAction("LogIn", "Home");
        }



        public ActionResult StaffLogIn(String Username, String Password)
        {
            var instructor = db.Instructors.Where(x => x.Username == Username);
            var administrator = db.SystemAdministratorInformations.Where(x => x.Username == Username);
            if (instructor.Any())
            {
                var ins = instructor.FirstOrDefault();

                if (ins.Password == Password)
                {
                    Session["InstructorId"] = ins.Id;
                    Session["Username"] = ins.Username;
                    Session["FirstName"] = ins.FirstName;
                    Session["MiddleName"] = ins.MiddleName;
                    Session["Department"] = ins.Department.DepartmentName;
                    Session["DepartmentId"] = ins.DepartmentId;
                    var role = db.InstractorRoles.Where(f => f.InstractorId == ins.Id);
                    if (role.Any())
                    {
                        var Roles = role.FirstOrDefault();
                        var instructorRole = db.Roles.Where(f => f.Id == Roles.RoleId);
                        if (instructorRole.Any())
                        {
                            InstructorImage instructorImage = new InstructorImage();
                            var Image = db.InstructorImages.Where(x => x.InstructorId == Roles.RoleId).FirstOrDefault();
                            var AssignedRole = instructorRole.FirstOrDefault();
                            if (AssignedRole.RoleName == "Head")
                            {
                                Session["Role"] = "Head";
                                Session["HeadProfilePictureURL"] = Image.ProfilePictureURL;
                            }
                            else
                            {
                                Session["Role"] = "Instructor";
                                //Session["InstructorProfilePictureURL"] = Image.ProfilePictureURL;

                            }


                        }


                    }

                    return RedirectToAction("Index", "Home");// Index implies the first front page for instructor
                }

                else
                {
                    TempData["Error"] = "Incorrect Password";

                }
            }
            if (administrator.Any())
            {
                var administor = administrator.FirstOrDefault();
                var positional = db.SystemAdmins.Where(f => f.SystemAdministratorInformation.Username == administor.Username && f.IsOnPosition == true).FirstOrDefault();
                if (administor.Password == Password)
                {
                    //Session["InstructorId"] = administor.Id;
                    Session["SystemAdminId"] = administor.Id;

                    Session["Username"] = administor.Username;
                    Session["FirstName"] = administor.FirstName;
                    Session["MiddleName"] = administor.MiddleName;
                    var role = db.SystemAdmins.Where(f => f.AdminId == positional.SystemAdministratorInformation.Id);
                    if (role.Any())
                    {
                        var Roles = role.FirstOrDefault();
                        var AdminRole = db.Roles.Where(f => f.Id == Roles.RoleId);
                        SystemAdminImage systemAdminImage = new SystemAdminImage();

                        var Image = db.SystemAdminImages.Where(x => x.SystemAdminId == administor.Id).FirstOrDefault();
                        if (AdminRole.Any())
                        {
                            var AssignedRole = AdminRole.FirstOrDefault();
                            if (AssignedRole.RoleName == "Admin")
                            {
                                Session["Admin"] = "Admin";
                               Session["AdminProfilePictureURL"] = Image.ProfilePictureURL;

                            }



                        }


                    }

                    return RedirectToAction("Index", "Home");// Index implies the first front page for instructor
                }

                else
                {
                    TempData["Error"] = "Incorrect Password";

                }
            }
            else
            {
                TempData["Error"] = "Incorrect Username";

            }
            return RedirectToAction("LogIn", "Home");

        }
        public ActionResult Logout()
        {
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("LogIn", "Home");
        }
        public ActionResult FAQ()
        {
            return View();
        }
        public ActionResult Developers()
        {
            return View();
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

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();

        }
        [HttpPost]
        public ActionResult Changepassword(ChangePassword changePassword)
        {
            if (Session["StudentId"] != null)
            {
                int StudentId = int.Parse(Session["StudentId"].ToString());
                var stu = db.Students.Find(StudentId);
                string hashpassword = Encrypt(changePassword.oldPassword);
                if (stu.Password == hashpassword)
                {
                    stu.Password = Encrypt(changePassword.Password);
                    db.SaveChanges();
                    TempData["Succuss"] = "your password is Successfully changed!";
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    TempData["Error"] = "Wrong Input";
                    return RedirectToAction("Changepassword", "Home");
                }
            }
            else if (Session["InstructorId"] != null)
            {
                int InstructorId = int.Parse(Session["InstructorId"].ToString());
                var ins = db.Instructors.Find(InstructorId);
                string hashpassword = Encrypt(changePassword.oldPassword);
                if (ins.Password == hashpassword)
                {
                    ins.Password = changePassword.Password;
                    db.SaveChanges();
                    TempData["Succuss"] = "your password is Successfully changed!";
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    TempData["Error"] = "Wrong Input";
                    return RedirectToAction("Changepassword", "Home");
                }
            }
            else if (Session["SystemAdminId"] != null)
            {
                int SystemAdminId = int.Parse(Session["SystemAdminId"].ToString());
                var Admin = db.SystemAdministratorInformations.Find(SystemAdminId);
                string hashpassword = Encrypt(changePassword.oldPassword);
                if (Admin.Password == hashpassword)
                {
                    Admin.Password = changePassword.Password;
                    db.SaveChanges();
                    TempData["Succuss"] = "your password is Successfully changed!";
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    TempData["Error"] = "Wrong Input";
                    return RedirectToAction("Changepassword", "Home");
                }
            }

            return View();

        }

    }
}