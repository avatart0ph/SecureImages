using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SecureImages.Models;
using SecureImages.Helpers;
using System.Configuration;
using System.IO;
using System.Web.Security;

namespace SecureImages.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            FormsAuthentication.SetAuthCookie("denise@mvcisawesome.com", false);
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Index(ImageEncodeViewModel file)
        {

            try
            {
                if (file.FileItem.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileItem.FileName);
                    //var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                    //file.SaveAs(path);

                    IRijndael encryptor = new ValueObfuscate();
                    string encodedUsername = HttpUtility.UrlEncode(encryptor.Encrypt(file.UserName, file.SaltPerUser));
                    string encodedFileName = HttpUtility.UrlEncode(encryptor.Encrypt(fileName, file.SaltPerUser));

                    string fullFilePathParentDirectory = ConfigurationManager.AppSettings["UploadPath"].ToString();

                    string[] allowdFile = { ".png", ".jpg", ".gif" };
                    //Here we are allowing only excel file so verifying selected file pdf or not
                    string fileExt = Path.GetExtension(fileName);
                    //Check whether selected file is valid extension or not
                    bool isValidFile = allowdFile.Contains(fileExt);
                    if (!isValidFile)
                    {
                        ModelState.AddModelError("ImageEncodeViewModel.FileName", "Please upload only png images");
                    }
                    else
                    {
                        // Get size of uploaded file, here restricting size of file
                        int fileSize = file.FileItem.ContentLength;
                        if (fileSize <= 10485760)//1048576 byte = 10MB
                        {
                            string directoryPath = fullFilePathParentDirectory + "/" + encodedUsername;
                            if (!Directory.Exists(Server.MapPath(directoryPath)))
                            {
                                Directory.CreateDirectory(Server.MapPath(directoryPath));
                            }
                            string filePath = Server.MapPath(directoryPath) + "/" + encodedFileName + fileExt;
                            
                            //Save selected file into server location
                            file.FileItem.SaveAs(filePath);

                        }
                        else
                        {
                            ModelState.AddModelError("ImageEncodeViewModel.FileName", "Attachment file size should not be greater then 10 MB!");
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("ImageEncodeViewModel.FileName", "Error occurred while uploading a file: " + ex.Message);
            }
            return View();

        }

        [HttpPost]
        public ActionResult TestUpload(HttpPostedFileBase file)
        {

            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                file.SaveAs(path);
            }

            return RedirectToAction("Index");
        }
    }
}