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
            DataBank db =  new DataBank();
            var dbResource = db.GetResourceById(subId);
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
                DataBank db = new DataBank();
                var res  = db.GetSubcription(resource.subcriptionId);
                var finalRes = new Resource() {ActivationFee =  resource.ActivationFee,Name = resource.Name,subcription = res};                
                db.CreateResource(finalRes);
                return RedirectToAction("Index");
            }
            return View(resource);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataBank db = new DataBank();
            Resource resource = db.GetResource(id);
            if (resource == null)
            {
                return HttpNotFound();
            }
            return View(resource);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,ActivationFee")] Resource resource)
        {
            if (ModelState.IsValid)
            {
                DataBank db = new DataBank();
                db.EditResource(resource);
                return RedirectToAction("Index");
            }
            return View(resource);
        }

        public ActionResult UseResource(int resID, int subcriptionID)
        {
            return RedirectToAction("Index", "ToDoLists", new { subcriptionId = subcriptionID , resId = resID});
        }
    }
}
