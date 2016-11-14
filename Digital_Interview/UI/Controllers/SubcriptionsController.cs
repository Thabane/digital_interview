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
        private Context db = new Context();

        // GET: Subcriptions
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                MethodBank mb = new MethodBank();
                return View(db.subcriptions.ToList());
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
                db.subcriptions.Add(subcription);
                db.SaveChanges();
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
            Subcription subcription = db.subcriptions.Find(id);
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
                db.Entry(subcription).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(subcription);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult EditCreditPool(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subcription subcription = db.subcriptions.Find(id);
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
                db.Entry(subcription).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(subcription);
        }
    }
}
