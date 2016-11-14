using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BusinessLogicLayer;
using DataModelCommon;

namespace UI.Controllers
{
    public class SubcriptionsController : Controller
    {        
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                MethodBank mb = new MethodBank();
                return View(mb.GetSubcriptions());
            }
            return RedirectToAction("Login","Home", new { area = ""});
        }

        public ActionResult AddUserToSubscription(int subcriptionId)
        {
            Session["SubcriptionId"] = subcriptionId.ToString();
            return View();
        }
        [HttpPost]
        public ActionResult AddUserToSubscription(UserSubcription subcription)
        {
            if (Session["UserID"] != null)
            {                
                MethodBank md = new MethodBank();
                int userId = subcription.UserID.ID;
                int subcriptionId = Convert.ToInt32(Session["SubcriptionId"].ToString());
                if (md.AddUserToSubcription(userId, subcriptionId))
                {
                    Session["SubcriptionId"] = null;
                    return RedirectToAction("Index", "Subcriptions", new { area = "" });
                }                
            }
            return RedirectToAction("Login", "Home", new { area = "" });
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Description,CreditLimit")] Subcription subcription)
        {
            if (ModelState.IsValid)
            {
                DataBank db = new DataBank();
                db.CreateSubscription(subcription);
                return RedirectToAction("Index");
            }

            return View(subcription);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataBank db = new DataBank();
            Subcription subcription = db.GetSubcription(id);
            if (subcription == null)
            {
                return HttpNotFound();
            }
            return View(subcription);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Description,CreditLimit")] Subcription subcription)
        {
            if (ModelState.IsValid)
            {
                DataBank db = new DataBank();
                db.EditSubcription(subcription);
                return RedirectToAction("Index");
            }
            return View(subcription);
        }

        public ActionResult EditCreditPool(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataBank db = new DataBank();
            Subcription subcription = db.GetSubcription(id);
            if (subcription == null)
            {
                return HttpNotFound();
            }
            return View(subcription);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCreditPool([Bind(Include = "ID,Name,Description,CreditLimit")] Subcription subcription)
        {
            if (ModelState.IsValid)
            {
                DataBank db = new DataBank();
                db.EditSubcription(subcription);
                return RedirectToAction("Index");
            }
            return View(subcription);
        }
    }
}
