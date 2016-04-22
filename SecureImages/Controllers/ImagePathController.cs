using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace SecureImages.Controllers
{
    public class ImagePathController : Controller
    {
        // GET: ImagePath
        public ActionResult Index(string query)
        {
            if (query == "1")
            {
                return Content("data:image/png;base64," + EncodeFile(Server.MapPath(@"/images/example.png")));
            }
            return Content("");
        }

        private string EncodeFile(string fileName)
        {
            return Convert.ToBase64String(System.IO.File.ReadAllBytes(fileName));
        }
    }
}