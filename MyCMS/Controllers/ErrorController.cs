using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyCMS.Models;
using System.Data.Entity;

namespace MyCMS.Controllers
{
    [Authorize]
    public class ErrorController : Controller
    {
        private MyCMSDBContent db = new MyCMSDBContent();

        public ActionResult Index()
        {
            return View("Error.cshtml");
        }

       
    }
}
