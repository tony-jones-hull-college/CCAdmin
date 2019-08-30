using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CCAdmin.Models;
using System.Text;

namespace CCAdmin.Controllers
{
    public class HomeController : Controller
    {
        public DBContext ImmediateDB { get; }

        public HomeController(DBContext dBContext)
        {
            ImmediateDB = dBContext;
        }

        public IActionResult Index(string studentid)
        {
            return View(studentid);
        }

        public IActionResult SendCCee(string p1)
        {
            try
            {
                var f = Request.Query["studentId"];
                var sDetail = ImmediateDB.StudentDetail.Where(i => i.RefNo == "").SingleOrDefault();

                var sentEmailControl = ImmediateDB.CcsentMailControl.Where(i => i.StudentDetailId == sDetail.StudentDetailId).SingleOrDefault();
                if (sentEmailControl == null)
                {
                    sentEmailControl = new CcsentMailControl()
                    {
                        StudentDetailId = sDetail.StudentDetailId,
                        DateSent = DateTime.Now,
                        Receipiant = sDetail.Email,
                        Attempts = 1
                    };
                    ImmediateDB.Add(sentEmailControl);
                }
                else
                {
                    sentEmailControl.Attempts++;
                    sentEmailControl.DateSent = DateTime.Now;
                    ImmediateDB.Update(sentEmailControl);
                }
                ImmediateDB.SaveChanges();

                var appCredential = ImmediateDB.AppCredentials.Where(i => i.IdentityKey == sDetail.StudentDetailId.ToString() && i.SystemRef == "CC").SingleOrDefault();
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
                    ImmediateDB.Add(appCredential);
                }
                else
                {

                }
                ImmediateDB.SaveChanges();

                var dispQueue = ImmediateDB.CcdispatchQueue.Where(i => i.StudentDetailId == sDetail.StudentDetailId).SingleOrDefault();
                if (dispQueue == null)
                {
                    dispQueue = new CcdispatchQueue()
                    {
                        StudentDetailId = sDetail.StudentDetailId,
                        AddedBy = "",
                        DateStamp = DateTime.Now,
                        Status = 0
                    };
                    ImmediateDB.Add(dispQueue);
                }
                else
                {
                    dispQueue.DateStamp = DateTime.Now;
                    dispQueue.Status = 0;
                    ImmediateDB.Update(dispQueue);
                }
                ImmediateDB.SaveChanges();
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
