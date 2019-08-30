using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCAdmin.Models;
using CCAdmin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace CCAdmin.Controllers
{
    public class StudentController : Controller
    {
        public DBContext Context { get; }
        public IConfiguration Config { get; }

        public StudentController(DBContext context, IConfiguration config)
        {
            Context = context;
            Config = config;
        }

        public IActionResult Index()
        {
            if (User.Identity.Name.Length == 0)
            {
                return View("NotAuthorized");
            }
            string username = User.Identity.Name.Split('\\')[User.Identity.Name.Split('\\').Length - 1];
            var CCPermission = Context.ProSolutionPermissions.Where(i => i.UserName == username && i.IsAllowed == true && i.ObjectName == Config["CCProSolutionSecurityObject:ObjectName"] && i.ActionName == Config["CCProSolutionSecurityObject:ActionName"] && i.ObjectType == Config["CCProSolutionSecurityObject:ObjectType"]).FirstOrDefault();
            var harrogateLetterPermission = Context.ProSolutionPermissions.Where(i => i.UserName == username && i.IsAllowed == true && i.ObjectName == Config["HOfferProSolutionSecurityObject:ObjectName"] && i.ActionName == Config["HOfferProSolutionSecurityObject:ActionName"] && i.ObjectType == Config["HOfferProSolutionSecurityObject:ObjectType"]).FirstOrDefault();
            var generalLetterPermission = Context.ProSolutionPermissions.Where(i => i.UserName == username && i.IsAllowed == true && i.ObjectName == Config["GOfferProSolutionSecurityObject:ObjectName"] && i.ActionName == Config["GOfferProSolutionSecurityObject:ActionName"] && i.ObjectType == Config["GOfferProSolutionSecurityObject:ObjectType"]).FirstOrDefault();
            if (!(CCPermission != null || harrogateLetterPermission != null || generalLetterPermission != null))
                return View("NotAuthorized");
            if (!int.TryParse(Request.Query["StudentDetailId"].FirstOrDefault(), out int studentDetailId))
            {
                if (int.TryParse(Request.Query["studentData"].FirstOrDefault(), out int studentId))
                {
                    var acadYear = Request.Query["acadYear"].FirstOrDefault();
                    var student = Context.StudentDetail.Where(i => i.StudentId == studentId && i.AcademicYearId == acadYear).SingleOrDefault();

                    return View("Index", MapStudentView(student, false, CCPermission != null, harrogateLetterPermission != null, generalLetterPermission != null));
                }
                return View("Index", new StudentView());
            }
            return View("Index", MapStudentView(Context.StudentDetail.Where(i => i.StudentDetailId == studentDetailId).SingleOrDefault(), false, CCPermission != null, harrogateLetterPermission != null, generalLetterPermission != null));
        }
         
        public IActionResult SendCC([FromQuery] int studentDetailId)
        {
            Log.Information($"Sending CC email to {studentDetailId}");
            string username = User.Identity.Name.Split('\\')[User.Identity.Name.Split('\\').Length - 1];
            var CCPermission = Context.ProSolutionPermissions.Where(i => i.UserName == username && i.IsAllowed == true && i.ObjectName == Config["CCProSolutionSecurityObject:ObjectName"] && i.ActionName == Config["CCProSolutionSecurityObject:ActionName"] && i.ObjectType == Config["CCProSolutionSecurityObject:ObjectType"]).FirstOrDefault();
            var harrogateLetterPermission = Context.ProSolutionPermissions.Where(i => i.UserName == username && i.IsAllowed == true && i.ObjectName == Config["HOfferProSolutionSecurityObject:ObjectName"] && i.ActionName == Config["HOfferProSolutionSecurityObject:ActionName"] && i.ObjectType == Config["HOfferProSolutionSecurityObject:ObjectType"]).FirstOrDefault();
            var generalLetterPermission = Context.ProSolutionPermissions.Where(i => i.UserName == username && i.IsAllowed == true && i.ObjectName == Config["GOfferProSolutionSecurityObject:ObjectName"] && i.ActionName == Config["GOfferProSolutionSecurityObject:ActionName"] && i.ObjectType == Config["GOfferProSolutionSecurityObject:ObjectType"]).FirstOrDefault();
            StudentDetail student = Context.StudentDetail.Where(i => i.StudentDetailId == studentDetailId).SingleOrDefault();
            if (student == null)
            {
                ModelState.AddModelError("", "No Student specified - close this window and try again");
                Log.Error("No Student specified");
                return View("Index", new StudentView());
            }
            //var str = Request.Query["studentData"].FirstOrDefault();
            var sDetail = Context.StudentDetail.Where(i => i.StudentDetailId == studentDetailId).SingleOrDefault();
            try
            {
                if (sDetail.CriminalConvictionId != 2)
                { 
                    ModelState.AddModelError("", $"Student {sDetail.RefNo} does not have a criminal conviction");
                    return View("Index", MapStudentView(sDetail, true, CCPermission != null, harrogateLetterPermission != null, generalLetterPermission != null));
                }
                var sentEmailControl = Context.CcsentMailControl.Where(i => i.StudentDetailId == sDetail.StudentDetailId).SingleOrDefault();
                if (sentEmailControl == null)
                {
                    sentEmailControl = new CcsentMailControl()
                    {
                        StudentDetailId = sDetail.StudentDetailId,
                        DateSent = DateTime.Now,
                        Receipiant = sDetail.Email,
                        Attempts = 1
                    };
                    Context.Add(sentEmailControl);
                }
                else
                {
                    if (sentEmailControl.Attempts > 0)
                        sentEmailControl.Attempts++;
                    sentEmailControl.DateSent = DateTime.Now;
                    Context.Update(sentEmailControl);
                }
                Context.SaveChanges();

                var appCredential = Context.AppCredentials.Where(i => i.IdentityKey == sDetail.StudentDetailId.ToString() && i.SystemRef == "CC").SingleOrDefault();
                if (appCredential == null)
                {
                    appCredential = new AppCredentials()
                    {
                        ActivationWord = getRandString(50),
                        IdentityKey = sDetail.StudentDetailId.ToString(),
                        Expiry = null,
                        SystemRef = "CC",
                        ProtectionString = getRandString(10)
                    };
                    Context.Add(appCredential);
                }
                else
                {

                }
                Context.SaveChanges();

                var dispQueue = Context.CcdispatchQueue.Where(i => i.StudentDetailId == sDetail.StudentDetailId && i.SystemRef == "CC").SingleOrDefault();
                if (dispQueue == null)
                {
                    dispQueue = new CcdispatchQueue()
                    {
                        StudentDetailId = sDetail.StudentDetailId,
                        AddedBy = User.Identity.Name,
                        DateStamp = DateTime.Now,
                        Status = 0,
                        SystemRef = "CC"
                    };
                    Context.Add(dispQueue);
                }
                else
                {
                    dispQueue.DateStamp = DateTime.Now;
                    dispQueue.AddedBy = User.Identity.Name;
                    dispQueue.Status = 0;
                    Context.Update(dispQueue);
                }
                Context.SaveChanges();
                Log.Information($"Finished SendCC for {sDetail.RefNo} {ModelState.IsValid}");
                if (!ModelState.IsValid)
                {
                    foreach (var error in ModelState)
                    {
                        Log.Error($"{error}");
                    }
                }
                return View("Index", MapStudentView(sDetail, true, CCPermission != null, harrogateLetterPermission != null, generalLetterPermission != null));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error has occurred");
                Log.Error(ex.Message);
                return View("Index", MapStudentView(sDetail, true, CCPermission != null, harrogateLetterPermission != null, generalLetterPermission != null));
            }
        }

        private string getRandString(int length)
        {
            string charStr = "abcdefghijklmnopqrstuvwxyz0123456789";
            Random rnd = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; ++i)
            {
                sb.Append(charStr[rnd.Next() % charStr.Length]);
            }
            return sb.ToString();
        }

        public IActionResult SendOfferLetter([FromQuery] int studentDetailId)
        {
            Log.Information($"Sending offer letter to {studentDetailId}");
            string username = User.Identity.Name.Split('\\')[User.Identity.Name.Split('\\').Length - 1];
            var CCPermission = Context.ProSolutionPermissions.Where(i => i.UserName == username && i.IsAllowed == true && i.ObjectName == Config["CCProSolutionSecurityObject:ObjectName"] && i.ActionName == Config["CCProSolutionSecurityObject:ActionName"] && i.ObjectType == Config["CCProSolutionSecurityObject:ObjectType"]).FirstOrDefault();
            var harrogateLetterPermission = Context.ProSolutionPermissions.Where(i => i.UserName == username && i.IsAllowed == true && i.ObjectName == Config["HOfferProSolutionSecurityObject:ObjectName"] && i.ActionName == Config["HOfferProSolutionSecurityObject:ActionName"] && i.ObjectType == Config["HOfferProSolutionSecurityObject:ObjectType"]).FirstOrDefault();
            var generalLetterPermission = Context.ProSolutionPermissions.Where(i => i.UserName == username && i.IsAllowed == true && i.ObjectName == Config["GOfferProSolutionSecurityObject:ObjectName"] && i.ActionName == Config["GOfferProSolutionSecurityObject:ActionName"] && i.ObjectType == Config["GOfferProSolutionSecurityObject:ObjectType"]).FirstOrDefault();
            StudentDetail student = Context.StudentDetail.Where(i => i.StudentDetailId == studentDetailId).SingleOrDefault();
            if (student == null)
            {
                ModelState.AddModelError("", "No Student specified - close this window and try again");
                Log.Error("No Student specified");
                return View("Index", new StudentView());
            }
            var harrogateApplicant = Context.ApplicantCoursesThisYearAndNext.Where(i => i.StudentDetailId == studentDetailId && i.Code.Substring(0, 1) == "R" && i.Choice == 1 && i.OfferId == 2 && (i.DecisionId == 1 || i.DecisionId == null) && (i.CollegeDecisionId == 1 || i.CollegeDecisionId == null)).FirstOrDefault();
            if (harrogateApplicant == null)
            {
                ModelState.AddModelError("", "This student does not have an accepted Harrogate offer as their first choice");
                Log.Error("This student does not have an accepted Harrogate offer as their first choice");
                return View("Index", MapStudentView(student, true, CCPermission != null, harrogateLetterPermission != null, generalLetterPermission != null));
            }
            try
            {
                student.UserDefined26 = DateTime.Now.ToString("MMM dd yyyy HH:mm");
                Context.StudentDetail.Update(student);
                //var str = Request.Query["studentData"].FirstOrDefault();
                var dispQueue = Context.CcdispatchQueue.Where(i => i.StudentDetailId == studentDetailId && i.SystemRef == "HARROLttr").SingleOrDefault();
                if (dispQueue == null)
                {
                    dispQueue = new CcdispatchQueue()
                    {
                        StudentDetailId = studentDetailId,
                        DateStamp = DateTime.Now,
                        Status = 0,
                        SystemRef = "HARROLttr",
                        AddedBy = User.Identity.Name
                    };
                    Context.Add(dispQueue);
                }
                else
                {
                    dispQueue.DateStamp = DateTime.Now;
                    dispQueue.Status = 0;
                    Context.Update(dispQueue);
                }
                Context.SaveChanges();
                return View("Index", MapStudentView(Context.StudentDetail.Where(i => i.StudentDetailId == studentDetailId).SingleOrDefault(), true, CCPermission != null, harrogateLetterPermission != null, generalLetterPermission != null));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error has occurred");
                Log.Error($"{ex.Message}");
                return View("Index", student);
            }
        }

        public IActionResult SendOtherOfferLetter([FromQuery] int studentDetailId)
        {
            Log.Information($"Sending offer letter to {studentDetailId}");
            string username = User.Identity.Name.Split('\\')[User.Identity.Name.Split('\\').Length - 1];
            var CCPermission = Context.ProSolutionPermissions.Where(i => i.UserName == username && i.IsAllowed == true && i.ObjectName == Config["CCProSolutionSecurityObject:ObjectName"] && i.ActionName == Config["CCProSolutionSecurityObject:ActionName"] && i.ObjectType == Config["CCProSolutionSecurityObject:ObjectType"]).FirstOrDefault();
            var harrogateLetterPermission = Context.ProSolutionPermissions.Where(i => i.UserName == username && i.IsAllowed == true && i.ObjectName == Config["HOfferProSolutionSecurityObject:ObjectName"] && i.ActionName == Config["HOfferProSolutionSecurityObject:ActionName"] && i.ObjectType == Config["HOfferProSolutionSecurityObject:ObjectType"]).FirstOrDefault();
            var generalLetterPermission = Context.ProSolutionPermissions.Where(i => i.UserName == username && i.IsAllowed == true && i.ObjectName == Config["GOfferProSolutionSecurityObject:ObjectName"] && i.ActionName == Config["GOfferProSolutionSecurityObject:ActionName"] && i.ObjectType == Config["GOfferProSolutionSecurityObject:ObjectType"]).FirstOrDefault();
            StudentDetail student = Context.StudentDetail.Where(i => i.StudentDetailId == studentDetailId).SingleOrDefault();
            if (student == null)
            {
                ModelState.AddModelError("", "No Student specified - close this window and try again");
                Log.Error("No Student specified");
                return View("Index", new StudentView());
            }
            var generalApplicant = Context.ApplicantCoursesThisYearAndNext.Where(i => i.StudentDetailId == studentDetailId && i.Choice == 1 && !i.Code.StartsWith("R") && i.OfferId == 2 && (i.DecisionId == 1 || i.DecisionId == null) && (i.CollegeDecisionId == 1 || i.CollegeDecisionId == null)).FirstOrDefault();
            if (generalApplicant == null)
            {
                ModelState.AddModelError("", "This student does not have an accepted Hull/Goole offer as their first choice");
                Log.Error("This student does not have an accepted Hull/Goole offer as their first choice");
                return View("Index", MapStudentView(student, true, CCPermission != null, harrogateLetterPermission != null, generalLetterPermission != null));
            }
            try
            {
                //student.UserDefined26 = DateTime.Now.ToString("MMM dd yyyy HH:mm");
                //Context.StudentDetail.Update(student);
                //var str = Request.Query["studentData"].FirstOrDefault();
                var dispQueue = Context.CcdispatchQueue.Where(i => i.StudentDetailId == studentDetailId && i.SystemRef == "OtherLttr").SingleOrDefault();
                if (dispQueue == null)
                {
                    dispQueue = new CcdispatchQueue()
                    {
                        StudentDetailId = studentDetailId,
                        DateStamp = DateTime.Now,
                        Status = 0,
                        SystemRef = "OtherLttr",
                        AddedBy = User.Identity.Name
                    };
                    Context.Add(dispQueue);
                }
                else
                {
                    dispQueue.DateStamp = DateTime.Now;
                    dispQueue.Status = 0;
                    Context.Update(dispQueue);
                }
                Context.SaveChanges();
                return View("Index", MapStudentView(Context.StudentDetail.Where(i => i.StudentDetailId == studentDetailId).SingleOrDefault(), true, CCPermission != null, harrogateLetterPermission != null, generalLetterPermission != null));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error has occurred");
                Log.Error($"{ex.Message}");
                return View("Index", student);
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public StudentView MapStudentView(StudentDetail student, bool showSuccess, bool CCPermission, bool harrogatePermission, bool generalPermission)
        {
            return new StudentView()
            {
                Name = $"{student.FirstForename} {student.Surname}",
                RefNo = student.RefNo,
                StudentDetailId = student.StudentDetailId,
                Success = ModelState.IsValid && showSuccess,
                ShowCCButton = CCPermission,
                ShowHarrogateLetterButton = harrogatePermission,
                ShowGeneralLetterButton = generalPermission
            };
        }
    }
}