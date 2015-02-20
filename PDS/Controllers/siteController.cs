using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PDS.Controllers
{
    public class siteController : Controller
    {
        // GET: site
        public ActionResult home()
        {
            return View();
        }
    }
}