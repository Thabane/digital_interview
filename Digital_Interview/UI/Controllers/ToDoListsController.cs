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
    public class ToDoListsController : Controller
    {
        private Context db = new Context();

        public ActionResult Index()
        {
            Session["ResId"] = Request.QueryString["resId"];
            Session["SubcriptionId"] = Request.QueryString["subcriptionId"];
            return View(db.toDoList.ToList().OrderBy(x => x.Prority));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDoList toDoList = db.toDoList.Find(id);
            if (toDoList == null)
            {
                return HttpNotFound();
            }
            toDoList.Resource = db.resource.Find(Convert.ToInt32(Session["ResId"]));
            ToDoListViewModel viewModel = new ToDoListViewModel();
            viewModel.ID = toDoList.ID;
            viewModel.Title = toDoList.Title;
            viewModel.Content = toDoList.Content;
            viewModel.CreatedDate = toDoList.CreatedDate;
            viewModel.Done = toDoList.Done;
            viewModel.DueDate = toDoList.DueDate;
            viewModel.Prority = toDoList.Prority;
            return View(viewModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Content,Prority,Done,CreatedDate,DueDate")] ToDoList toDoList)
        {
            toDoList.Done = false;
            toDoList.CreatedDate = DateTime.Now;
            toDoList.Resource = db.resource.Find(Convert.ToInt32(Session["ResId"]));
            toDoList.Resource.subcription = db.subcriptions.Find(Convert.ToInt32(Session["SubcriptionId"]));

            try
            {
                db.toDoList.Add(toDoList);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(toDoList);
            }
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDoList toDoList = db.toDoList.Find(id);
            if (toDoList == null)
            {
                return HttpNotFound();
            }
            toDoList.Resource = db.resource.Find(Convert.ToInt32(Session["ResId"]));
            ToDoListViewModel viewModel = new ToDoListViewModel();
            viewModel.ID = toDoList.ID;
            viewModel.Title = toDoList.Title;
            viewModel.Content = toDoList.Content;
            viewModel.CreatedDate = toDoList.CreatedDate;
            viewModel.Done = toDoList.Done;
            viewModel.DueDate = toDoList.DueDate;
            viewModel.Prority = toDoList.Prority;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Content,Prority,Done,CreatedDate,DueDate")] ToDoListViewModel toDoList)
        {
            ToDoList UpdatedToDo = new ToDoList();

            UpdatedToDo.ID = toDoList.ID;
            UpdatedToDo.Title = toDoList.Title;
            UpdatedToDo.Content = toDoList.Content;
            UpdatedToDo.CreatedDate = toDoList.CreatedDate;
            UpdatedToDo.Done = toDoList.Done;
            UpdatedToDo.DueDate = toDoList.DueDate;
            UpdatedToDo.Prority = toDoList.Prority;
            UpdatedToDo.Resource = db.resource.Find(toDoList.Resource);

            if (ModelState.IsValid)
            {
                db.Entry(UpdatedToDo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(toDoList);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDoList toDoList = db.toDoList.Find(id);
            if (toDoList == null)
            {
                return HttpNotFound();
            }
            return View(toDoList);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ToDoList toDoList = db.toDoList.Find(id);
            db.toDoList.Remove(toDoList);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
