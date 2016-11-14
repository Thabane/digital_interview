using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogicLayer;
using DataModelCommon;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                MethodBank mb = new MethodBank();
                mb.RegisterUser(user);
                ModelState.Clear();
                ViewBag.Message = user.FirstName + " " + user.LastName + " Successfully Added.";            
            }
            return View();
        }
        public ActionResult Login()
        {
            Session["UserID"] = null;
            Session["FirstName"] = null;
            Session["LastName"] = null;
            return View();
        }
        [HttpPost]
        public ActionResult Login(User user)
        {
            MethodBank mb = new MethodBank();
            User sysUser = mb.Login(user);

            if (sysUser != null)
            {
                Session["UserID"] = sysUser.ID.ToString();
                Session["FirstName"] = sysUser.FirstName;
                Session["LastName"] = sysUser.LastName;
                return RedirectToAction("Index","Index", new { userId = Session["UserID"] });
            }
            ModelState.AddModelError("","Login Credentials are Incorrect");
            return View();
        }


    }
}
