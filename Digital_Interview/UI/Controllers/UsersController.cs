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
        private Context db = new Context();

        // GET: Users
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
            UserSubcription userSubcription = db.userSubcriptions.Find(id);
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
                db.Entry(userSubcription).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userSubcription);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult AddVoucher(int userId,int subcriptionId)
        {
            Session["VoucherUserId"] = userId.ToString();
            Session["VoucherSubscriptionId"] = subcriptionId.ToString();
            return View();
        }
        [HttpPost]
        public ActionResult AddVoucher([Bind(Include = "user,subcription,Used,ExpiryDate")] Voucher voucher)
        {
            voucher.user = db.users.Find(Convert.ToInt32(Session["VoucherUserId"]));
            voucher.subcription = db.subcriptions.Find(Convert.ToInt32(Session["VoucherSubscriptionId"]));
            voucher.Date = DateTime.Now;

            try
            {
                db.couchers.Add(voucher);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }

            
        }
    }
}
