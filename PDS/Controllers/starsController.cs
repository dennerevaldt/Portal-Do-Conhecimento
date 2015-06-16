using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PDS.Controllers
{
    [Authorize]
    public class starsController : Controller
    {
        public ActionResult index(int id)
        {
            return View();
        }
    }
}