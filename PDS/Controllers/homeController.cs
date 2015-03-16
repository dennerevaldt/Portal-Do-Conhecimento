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
        /// <summary>
        /// Action da página home do portal.
        /// </summary>
        /// <returns>Home portal.</returns>
        public ActionResult index()
        {
            return View();
        }

    }
}