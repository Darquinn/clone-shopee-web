using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FINAL.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult ProfileHome()
        {
            return View();
        }

        public PartialViewResult ProfilePartial()
        {
            return PartialView();
        }
    }
}