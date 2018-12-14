using System.Collections.Generic;
using System.Linq;
using FileProcessor.Models;
using System.Web.Mvc;
using FileProcessor.Models.Users;
using FileProcessor.ViewModels;
using Microsoft.AspNet.Identity;
using System.Net;
using FileProcessor.Models.Results;

namespace FileProcessor.Controllers
{
    public class ProcessedController : Controller
    {
        // GET: ProcessedData
        [Authorize(Roles = Role.Admin + "," + Role.User)]
        public ActionResult Files()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ApplicationDbContext dr = new ApplicationDbContext();
            ApplicationDbContext dr2 = new ApplicationDbContext();
            ApplicationDbContext dr3 = new ApplicationDbContext();
            ApplicationDbContext dr4 = new ApplicationDbContext();
            List<VMFileDetail> r = new List<VMFileDetail>();
            string uId = HttpContext.User.Identity.GetUserId();

            if (HttpContext.User.IsInRole(Role.User))
            {
                foreach (var o in db.FileDetails.Where(rec => rec.UserId == uId))
                {
                    var x = new VMFileDetail();
                    x.Id = o.Id;
                    x.UserId = o.UserId;
                    x.UserName = dr.Users.Where(user => user.Id == x.UserId).FirstOrDefault().FullName;
                    x.FileName = o.FileName;
                    x.DateProcessed = o.DateProcessed;
                    x.Total = dr2.CalculationDetails.Where(c => c.FileId == o.Id).Count();
                    x.Erred = dr3.CalculationDetails.Where(c => c.FileId == o.Id && c.ErrorMessage == "Calculation Error").Count();
                    x.Duplicates = dr4.CalculationDetails.Where(d => d.FileId == o.Id && d.ErrorMessage == "Duplicate Record").Count();
                    r.Add(x);
                }

            }
            else if (HttpContext.User.IsInRole(Role.Admin))
            {

                foreach (var p in db.FileDetails)
                {
                    var y = new VMFileDetail();
                    y.Id = p.Id;
                    y.UserId = p.UserId;
                    y.UserName = dr.Users.Where(user => user.Id == y.UserId).FirstOrDefault().FullName;
                    y.FileName = p.FileName;
                    y.DateProcessed = p.DateProcessed;
                    y.Total = dr2.CalculationDetails.Where(c => c.FileId == p.Id).Count();
                    y.Erred = dr3.CalculationDetails.Where(c => c.FileId == p.Id && c.ErrorMessage == "Calculation Error").Count();
                    y.Duplicates = dr4.CalculationDetails.Where(d => d.FileId == p.Id && d.ErrorMessage == "Duplicate Record").Count();
                    r.Add(y);
                }

            }
            return View("~/Views/Documents/Processed/Files.cshtml", r);
        }
        

        [Authorize(Roles = Role.Admin + "," + Role.User)]
        public ActionResult Data(int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ApplicationDbContext dr = new ApplicationDbContext();
            List<VMCalculationDetail> d = new List<VMCalculationDetail>();
            string uId = HttpContext.User.Identity.GetUserId();

           
            foreach (var fd in db.CalculationDetails.Where(f => f.FileId == id))
            {
                var y = new VMFormula();
                var x = new VMCalculationDetail();
                x.Id = fd.Id;
                x.Formula = fd.Formula;
                x.FormulaDescription = y.GetDescription(fd.Formula);
                x.A = fd.A;
                x.B = fd.B;
                x.C = fd.C;
                x.CalculatedResult = fd.CalculatedResult;
                x.ErrorMessage = fd.ErrorMessage;
                d.Add(x);
            }

            return View("~/Views/Documents/Processed/Data.cshtml", d);

        }


        // GET: ProcessedData/Delete/5
        [Authorize(Roles = Role.Admin + "," + Role.User)]
        public ActionResult Delete(int? id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ApplicationDbContext dr = new ApplicationDbContext();
            FileDetail fileToDelete = db.FileDetails.Find(id);
            VMFileDetail fd = new VMFileDetail();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                if (fileToDelete == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    
                    fd.Id = fileToDelete.Id;
                    fd.UserId = fileToDelete.UserId;
                    fd.UserName = dr.Users.Where(user => user.Id == fd.UserId).FirstOrDefault().FullName;
                    fd.FileName = fileToDelete.FileName;
                    fd.DateProcessed = fileToDelete.DateProcessed;
                }

            }

            return View("~/Views/Documents/Processed/Delete.cshtml", fd);
        }
        
        [HttpPost]
        [Authorize(Roles = Role.Admin + "," + Role.User)]
        public ActionResult Delete(int id)
        {
            try
            {
                ApplicationDbContext db = new ApplicationDbContext();
                FileDetail fileToDelete = db.FileDetails.Find(id);
                db.FileDetails.Remove(fileToDelete);
                db.SaveChanges();

                return RedirectToAction("Files");
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
    }
}