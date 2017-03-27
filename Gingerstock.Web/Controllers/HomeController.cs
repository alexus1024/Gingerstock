using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gingerstock2.Store;

namespace Gingerstock.Web.Controllers
{
    public class HomeController : Controller
    {


        public HomeController()
        {

        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
