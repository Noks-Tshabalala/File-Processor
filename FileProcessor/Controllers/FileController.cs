using System.Collections.Generic;
using System.Web.Mvc;
using FileProcessor.Models.Users;
using System.IO;
using System.Web;
using FileProcessor.Properties;
using Microsoft.AspNet.Identity;
using System.Linq;
using FileProcessor.Models.Results;

namespace FileProcessor.Controllers
{
    public class FileController : Controller
    {
        [HttpGet]
        [Authorize(Roles = Role.Admin + "," + Role.User)]
        public ActionResult Upload()
        {
            
            return View("~/Views/Documents/Unprocessed/Uploads.cshtml");
        }

        [HttpPost]
        [Authorize(Roles = Role.Admin + "," + Role.User)]
        public ActionResult Upload(List<HttpPostedFileBase> files)
        {
            int itemCount = files.Count();
            int successfulItemsCount = 0;
            string fileName = "";
            string userName = User.Identity.GetUserName();
            string uId = User.Identity.GetUserId();
            string errorMsg = "";
            var path1 = "";
            var path2 = "";
            
            List<string> fileList = new List<string>();

            foreach (var item in files)
            {
                if (item != null && item.ContentLength > 0)
                {
                    if (Path.GetExtension(item.FileName).ToLower() == ".csv" || Path.GetExtension(item.FileName).ToLower() == ".txt")
                    {
                        fileName = Path.GetFileName(item.FileName);
                        path1 = Path.Combine(Server.MapPath(Settings.Default.UnprocessedFiles), userName + "_" + fileName);
                        path2 = Path.Combine(Server.MapPath(Settings.Default.ProcessedFiles), userName + "_" + fileName);
                        DirectoryInfo di = new DirectoryInfo(path1);

                        if(!Directory.Exists(path1) && !Directory.Exists(path2))
                        {
                            item.SaveAs(path1);
                            fileList.Add(fileName);
                            successfulItemsCount = successfulItemsCount + 1;
                        }
                        else
                        {
                            errorMsg = errorMsg + " A file with the name " + fileName + " already exists in the database.";
                        }
                    }
                    else
                    {
                        errorMsg = errorMsg + " The file " + fileName + " was not uploaded due to file type restrictions.";
                    }
                }
                else
                {
                    errorMsg = errorMsg + " A file was skipped during validation.";
                }
            }

            if(successfulItemsCount > 0)
            {
                string rpFailed = "";
                ResultsProcessor rp = new ResultsProcessor();
                rpFailed = rp.GetFiles(fileList, uId, userName);
                
                if(rpFailed.Contains("Success"))
                {
                    ViewBag.Message = "Upload succeeded!";
                    ViewBag.UploadSuccess = true;
                    
                }
                else
                {
                    ViewBag.ErrorMessage = "One or more files contain variables that could not be calculated.";
                    ViewBag.UploadSuccess = false;
                }
                
            }
            
            if(itemCount > successfulItemsCount)
            {
                if (errorMsg.Length > 0)
                {
                    ViewBag.ErrorMessage = errorMsg;
                    ViewBag.UploadSuccess = false;
                }
                else
                {
                    ViewBag.ErrorMessage = "One or more files contain variables that could not be calculated.";
                    ViewBag.UploadSuccess = false;
                }
            }
            
            return View("~/Views/Documents/Unprocessed/Uploads.cshtml");
        }

    }
}