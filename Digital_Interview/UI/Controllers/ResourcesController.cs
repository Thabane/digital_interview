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
    public class ResourcesController : Controller
    {
        private Context db = new Context();

        public ActionResult Index()
        {
            int subId = Convert.ToInt32(Session["SubscriptionID"]);
            var dbResource = db.resource.Where(x => x.subcription.ID == subId);
            List<ResourceModelView> rvm = new List<ResourceModelView>();

            foreach (var res in dbResource)
            {
                rvm.Add(new ResourceModelView() { ID = res.ID, ActivationFee = res.ActivationFee, Name = res.Name, subcriptionId = subId });
            }
            return View(rvm);
        }
        public ActionResult SelectSubscription()
        {
            Session["SubscriptionID"] = null;
            return View();
        }
        [HttpPost]
        public ActionResult SelectSubscription(Subcription subscriptions)
        {
            if (subscriptions != null)
            {
                Session["SubscriptionID"] = subscriptions.ID;
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,ActivationFee,subcriptionId")] ResourceModelView resource)
        {
            if (ModelState.IsValid)
            {
                var res  = db.subcriptions.Find(resource.subcriptionId);
                var finalRes = new Resource() {ActivationFee =  resource.ActivationFee,Name = resource.Name,subcription = res};
                db.resource.Add(finalRes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(resource);
        }

        // GET: Resources/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resource resource = db.resource.Find(id);
            if (resource == null)
            {
                return HttpNotFound();
            }
            return View(resource);
        }

        // POST: Resources/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,ActivationFee")] Resource resource)
        {
            if (ModelState.IsValid)
            {
                db.Entry(resource).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(resource);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult UseResource(int resID, int subcriptionID)
        {
            return RedirectToAction("Index", "ToDoLists", new { subcriptionId = subcriptionID , resId = resID});
        }
    }
}
