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
    public class ActivityItemsController : Controller
    {
        private ActivityDBEntities db = new ActivityDBEntities();

        public ActionResult Index(string id)
        {
            ViewBag.Student_N_Number = id;
            var activities = db.ActivityItems.Where(a => a.Student_N_Number == id).OrderBy(a => a.Term);

            return PartialView("_Index", activities.ToList());
        }

        [ChildActionOnly]
        public ActionResult List(string id)
        {
            ViewBag.Student_N_Number = id;
            var activities = db.ActivityItems.Where(a => a.Student_N_Number == id);

            return PartialView("_List", activities.ToList());
        }

        public ActionResult Create(string student_n_number)
        {
            ActivityItem activityitem = new ActivityItem();
            activityitem.Student_N_Number = student_n_number;

            return PartialView("_Create", activityitem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ActivityID,Student_N_Number,Term,Date,Hours,Description")] ActivityItem activityitem)
        {
            if (ModelState.IsValid)
            {
                db.ActivityItems.Add(activityitem);
                db.SaveChanges();

                string url = Url.Action("Index", "ActivityItems", new { id = activityitem.Student_N_Number });
                return Json(new { success = true, url = url });
            }

            return PartialView("_Create", activityitem);
        }

        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityItem activityitem = db.ActivityItems.Find(id);
            if (activityitem == null)
            {
                return HttpNotFound();
            }

            return PartialView("_Edit", activityitem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ActivityID,Student_N_Number,Term,Date,Hours,Description")] ActivityItem activityitem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activityitem).State = EntityState.Modified;
                db.SaveChanges();

                string url = Url.Action("Index", "ActivityItems", new { id = activityitem.Student_N_Number });
                return Json(new { success = true, url = url });
            }


            return PartialView("_Edit", activityitem);
        }

        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityItem activityitem = db.ActivityItems.Find(id);
            if (activityitem == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Delete", activityitem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ActivityItem activityitem = db.ActivityItems.Find(id);
            string N_Number = activityitem.Student_N_Number;
            db.ActivityItems.Remove(activityitem);
            db.SaveChanges();

            string url = Url.Action("Index", "ActivityItems", new { id = N_Number });
            return Json(new { success = true, url = url });

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

