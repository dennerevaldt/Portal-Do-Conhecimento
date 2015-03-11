using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PDS.Controllers
{
    [Authorize]
    public class homeController : Controller
    {
        // GET: home page
        public ActionResult index()
        {
            return View();
        }

    }
}