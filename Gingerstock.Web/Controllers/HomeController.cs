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
        private readonly IDbService _srv;

        public HomeController(IDbService srv)
        {
            _srv = srv;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
