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
    public class ReportsController : Controller
    {
        private ActivityDBEntities db = new ActivityDBEntities();
        DateTime sqlFormattedDate;

        // GET: Reports
        [HttpGet]
        public ActionResult Index()
        {
            var activityItems = db.ActivityItems.Include(a => a.Semester).Include(a => a.Student);
            return View(activityItems.OrderBy(a => a.TermStartDate).ThenBy(a => a.Student.LastName).ThenBy(a => a.Student.FirstName).ToList());
        }

        // POST: Reports
        [HttpPost]
        public ActionResult Index(string action)
        {
            string number = Request.Form["Number"].ToString();
            ViewBag.Number = number;
            string lastName = Request.Form["LastName"].ToString();
            ViewBag.lastName = lastName;
            string firstName = Request.Form["FirstName"].ToString();
            ViewBag.firstName = firstName;
            string term = Request.Form["Term"].ToString();
            ViewBag.term = term;
            string major = Request.Form["Major"].ToString();
            ViewBag.major = major;
            string date = Request.Form["Date"].ToString();
            ViewBag.date = date;

            if (action == "Clear")
            {
                number = "";
                ViewBag.Number = number;
                lastName = "";
                ViewBag.lastName = lastName;
                firstName = "";
                ViewBag.firstName = firstName;
                term = "";
                ViewBag.term = term;
                major = "";
                ViewBag.major = major;
                date = "";
                ViewBag.date = date;
            }

            if (date != null && date != "")
            {
                try
                {
                    //var sqlFormattedDate = Convert.ToDateTime(filterString).ToShortDateString();
                    sqlFormattedDate = (DateTime.ParseExact(date, "MM/dd/yyyy", null)).Date;
                    //Console.WriteLine(sqlFormattedDate);

                    //activityItems = (from p in db.ActivityItems.Include(a => a.Semester).Include(a => a.Student)
                    //               where (p.Date == sqlFormattedDate)
                    //             orderby (p.Term)
                    //           select p
                    //      );
                }
                catch (Exception ex)
                {
                    ViewBag.Exception = ex.Message;
                    ViewBag.Message = "Invalid Date format. Please enter mm/dd/yyyy format";
                }
                finally
                {
                }
            }

            var activityItems = (db.ActivityItems.Include(a => a.Semester).Include(a => a.Student).OrderBy(a => a.TermStartDate).ThenBy(a => a.Student.LastName).ThenBy(a => a.Student.FirstName)).AsQueryable();
            if (number != null && number != "") activityItems = activityItems.Where(s => s.Student_N_Number == number);
            if (lastName != null && lastName != "") activityItems = activityItems.Where(s => s.Student.LastName == lastName);
            if (firstName != null && firstName != "") activityItems = activityItems.Where(s => s.Student.FirstName == firstName);
            if (term != null && term != "") activityItems = activityItems.Where(s => s.Term == term);
            if (major != null && major != "") activityItems = activityItems.Where(s => s.Student.Major == major);
            if (date != null && date != "") activityItems = activityItems.Where(s => s.Date == sqlFormattedDate);

            return View(activityItems.ToList());

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
