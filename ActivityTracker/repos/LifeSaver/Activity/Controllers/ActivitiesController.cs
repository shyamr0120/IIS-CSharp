using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Tracking.Models;

namespace Tracking.Controllers
{
    public class ActivitiesController : Controller
    {
        private ActivityDBEntities db = new ActivityDBEntities();

        // GET: Activities
        public ActionResult Index(String id)
        {
            ViewBag.Student_N_Number = id;
            var activities = db.Activities.Where(a => a.Student_N_Number == id);

            return PartialView("_Index", activities.ToList());
            //var activities = db.Activities.Include(a => a.Faculty).Include(a => a.Semester).Include(a => a.Student).Include(a => a.User);
            //return View(activities.ToList());
        }

        [ChildActionOnly]
        public ActionResult List(String id)
        {
            ViewBag.Student_N_Number = id;
            var activities = db.Activities.Where(a => a.Student_N_Number == id);

            return PartialView("_List", activities.ToList());
        }


        // GET: Activities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // GET: Activities/Create
        public ActionResult Create(int ActivityID)
        {

            Activity activity = new Activity();
            activity.ActivityID = ActivityID;

            return PartialView("_Create", activity);

        }

        // POST: Activities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ActivityID,Student_N_Number,Term,Date,Hours,Description,AuthorizedBy,EnteredBy,Timestamp")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                db.Activities.Add(activity);
                db.SaveChanges();

                string url = Url.Action("Index", "Activities", new { id = activity.ActivityID });
                return Json(new { success = true, url = url });
            }

            return PartialView("_Create", activity);

        }

        // GET: Activities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorizedBy = new SelectList(db.Faculties, "N_Number", "FirstName", activity.AuthorizedBy);
            ViewBag.Term = new SelectList(db.Semesters, "Term", "Term", activity.Term);
            ViewBag.Student_N_Number = new SelectList(db.Students, "N_Number", "FirstName", activity.Student_N_Number);
            ViewBag.EnteredBy = new SelectList(db.Users, "N_Number", "FirstName", activity.EnteredBy);
            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ActivityID,Student_N_Number,Term,Date,Hours,Description,AuthorizedBy,EnteredBy,Timestamp")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorizedBy = new SelectList(db.Faculties, "N_Number", "FirstName", activity.AuthorizedBy);
            ViewBag.Term = new SelectList(db.Semesters, "Term", "Term", activity.Term);
            ViewBag.Student_N_Number = new SelectList(db.Students, "N_Number", "FirstName", activity.Student_N_Number);
            ViewBag.EnteredBy = new SelectList(db.Users, "N_Number", "FirstName", activity.EnteredBy);
            return View(activity);
        }

        // GET: Activities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Activity activity = db.Activities.Find(id);
            db.Activities.Remove(activity);
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
