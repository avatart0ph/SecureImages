using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecureImages.Models
{
    public class ImageEncodeViewModel
    {
        public HttpPostedFileBase FileItem { get; set; }
        public string UserName { get; set; }
        public string SaltPerUser { get; set; }
    }
}