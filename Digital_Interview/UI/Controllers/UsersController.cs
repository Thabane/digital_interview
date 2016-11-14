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
    public class UsersController : Controller
    {
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                MethodBank md = new MethodBank();
                int userId = Convert.ToInt32(Session["UserID"]);
                return View(md.GetUserWithDailyLimit());
            }
            return RedirectToAction("Login", "Home", new { area = "" });
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataBank db = new DataBank();
            UserSubcription userSubcription = db.GetUserSubcription(id);
            if (userSubcription == null)
            {
                return HttpNotFound();
            }
            return View(userSubcription);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,SubcriptionID,UserID,DailyLimit")] UserSubcription userSubcription)
        {
            if (ModelState.IsValid)
            {
                DataBank db = new DataBank();
                db.EditUserSubcription(userSubcription);
                return RedirectToAction("Index");
            }
            return View(userSubcription);
        }

        public ActionResult AddVoucher(int userId, int subcriptionId)
        {
            Session["VoucherUserId"] = userId.ToString();
            Session["VoucherSubscriptionId"] = subcriptionId.ToString();
            return View();
        }
        [HttpPost]
        public ActionResult AddVoucher([Bind(Include = "user,subcription,Used,ExpiryDate")] Voucher voucher)
        {
            DataBank db = new DataBank();
            voucher.user = db.GetUser(Convert.ToInt32(Session["VoucherUserId"]));
            voucher.subcription = db.GetSubcription(Convert.ToInt32(Session["VoucherSubscriptionId"]));
            voucher.Date = DateTime.Now;
            if (db.AddVoucher(voucher))
            {
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
