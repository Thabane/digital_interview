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
        public ActionResult Index()
        {
            Session["ResId"] = Request.QueryString["resId"];
            Session["SubcriptionId"] = Request.QueryString["subcriptionId"];
            DataBank db = new DataBank();
            return View(db.GetToDoList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataBank db = new DataBank();
            ToDoList toDoList = db.GetToDolistItem(id);
            if (toDoList == null)
            {
                return HttpNotFound();
            }
            toDoList.Resource = db.GetResource(Convert.ToInt32(Session["ResId"]));
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
            DataBank db = new DataBank();
            toDoList.Done = false;
            toDoList.CreatedDate = DateTime.Now;
            toDoList.Resource = db.GetResource(Convert.ToInt32(Session["ResId"]));
            toDoList.Resource.subcription = db.GetSubcription(Convert.ToInt32(Session["SubcriptionId"]));
            if(db.CreateToDoListItem(toDoList))
            {
                return RedirectToAction("Index");
            }
            return View(toDoList);

        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataBank db = new DataBank();
            ToDoList toDoList = db.GetToDolistItem(id);
            if (toDoList == null)
            {
                return HttpNotFound();
            }
            toDoList.Resource = db.GetResource(Convert.ToInt32(Session["ResId"]));
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
            DataBank db = new DataBank();
            UpdatedToDo.ID = toDoList.ID;
            UpdatedToDo.Title = toDoList.Title;
            UpdatedToDo.Content = toDoList.Content;
            UpdatedToDo.CreatedDate = toDoList.CreatedDate;
            UpdatedToDo.Done = toDoList.Done;
            UpdatedToDo.DueDate = toDoList.DueDate;
            UpdatedToDo.Prority = toDoList.Prority;
            UpdatedToDo.Resource = db.GetResource(toDoList.Resource);

            if (ModelState.IsValid)
            {
                db.EditToDo(UpdatedToDo);
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
            DataBank db = new DataBank();
            ToDoList toDoList = db.GetToDolistItem(id);
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
            DataBank db = new DataBank();
            ToDoList toDoList = db.GetToDolistItem(id);
            db.RemoveToDo(toDoList);
            return RedirectToAction("Index");
        }
    }
}
