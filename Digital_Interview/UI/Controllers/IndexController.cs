using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogicLayer;
using DataModelCommon;

namespace UI.Controllers
{
    public class IndexController : Controller
    {
        // GET: Index
        public ActionResult Index(int userId)
        {
            if (Session["UserID"] != null)
            {
                MethodBank mbBank = new MethodBank();
                Dashboard dashboard = mbBank.GetDashboard(Convert.ToInt32(Session["UserID"]));
                return View(dashboard);
            }
            return View();
        }
    }
}