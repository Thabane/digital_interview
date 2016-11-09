using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogicLayer;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            MethodBank client =  new MethodBank();
            var users =  client.GetUsers();
            return View();
        }
    }
}
