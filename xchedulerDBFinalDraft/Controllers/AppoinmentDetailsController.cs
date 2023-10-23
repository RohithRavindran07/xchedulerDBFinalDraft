using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using xchedulerDBFinalDraft.Models;

namespace xchedulerDBFinalDraft.Controllers
{
    public class AppoinmentDetailsController : Controller
    {
        private Entities db = new Entities();

        // GET: AppoinmentDetails
        [Authorize(Roles = "doctor,admin")]
        public ActionResult Index()
        {
            var appoinmentDetails = db.AppoinmentDetails.Include(a => a.AspNetUser).Include(a => a.CenterDetail).Include(a => a.AspNetUser1);
            return View(appoinmentDetails.ToList());
        }
        [Authorize(Roles = "patient")]
        public ActionResult PatientIndex()
        {
            string currentUserId = User.Identity.GetUserId();
            var patientAppointments = db.AppoinmentDetails
                .Where(a => a.PatientID == currentUserId)
                .Include(a => a.AspNetUser)
                .Include(a => a.CenterDetail)
                .Include(a => a.AspNetUser1)
                .ToList();

            return View("Index", patientAppointments);
        }


        // GET: AppoinmentDetails/Details/5

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppoinmentDetail appoinmentDetail = db.AppoinmentDetails.Find(id);
            if (appoinmentDetail == null)
            {
                return HttpNotFound();
            }
            return View(appoinmentDetail);
        }

        private bool IsDuplicateAppointment(AppoinmentDetail appointment)
        {
            var existingAppointment = db.AppoinmentDetails
                .FirstOrDefault(a => a.AppoinmentDate == appointment.AppoinmentDate &&
                                     a.AppoinmentTime == appointment.AppoinmentTime &&
                                     a.CenterID == appointment.CenterID &&
                                     a.ReportStaffID == appointment.ReportStaffID);

            return existingAppointment != null;
        }


        // GET: AppoinmentDetails/PatientCreate
        [Authorize(Roles = "patient")]
        public ActionResult PatientCreate()
        {
            var currentUserId = User.Identity.GetUserId();
            var currentUser = db.AspNetUsers.Find(currentUserId);

            if (currentUser == null)
            {
                return HttpNotFound();
            }

            ViewBag.CenterID = new SelectList(db.CenterDetails, "CenterId", "CenterLocation");

            var patientList = new List<SelectListItem>
    {
        new SelectListItem { Value = currentUserId, Text = currentUser.Email }
    };

            ViewBag.PatientID = new SelectList(patientList, "Value", "Text");
            ViewBag.ReportStaffID = new SelectList(db.AspNetUsers, "Id", "Email");

            return View("Create"); // Redirect to the existing "Create" view
        }

        // POST: AppoinmentDetails/PatientCreate
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "patient")]
        public ActionResult PatientCreate([Bind(Include = "AppoinmentDate,AppoinmentTime,AppoinmentEndTime,AppoinmentStatus,PatientID,ReportStaffID,CenterID")] AppoinmentDetail appoinmentDetail)
        {
            if (ModelState.IsValid)
            {
                if (IsDuplicateAppointment(appoinmentDetail))
                {
                    ModelState.AddModelError("", "An appointment with the same properties already exists.");
                    // Add a custom error message and return to the Create view
                    ViewBag.ReportStaffID = new SelectList(db.AspNetUsers, "Id", "Email");
                    ViewBag.CenterID = new SelectList(db.CenterDetails, "CenterId", "CenterLocation");
                    ViewBag.PatientID = new SelectList(db.AspNetUsers, "Id", "Email");
                    return View("Create", appoinmentDetail);
                }

                db.AppoinmentDetails.Add(appoinmentDetail);
                appoinmentDetail.PatientID = User.Identity.GetUserId(); // Set the patient ID to the current user's ID
                db.SaveChanges();
                return RedirectToAction("PatientIndex"); // Redirect to the patient's appointments list
            }

            ViewBag.ReportStaffID = new SelectList(db.AspNetUsers, "Id", "Email", appoinmentDetail.ReportStaffID);
            ViewBag.CenterID = new SelectList(db.CenterDetails, "CenterId", "CenterLocation", appoinmentDetail.CenterID);
            ViewBag.PatientID = new SelectList(db.AspNetUsers, "Id", "Email", appoinmentDetail.PatientID);

            return View("Create", appoinmentDetail); // Return to the "Create" view with validation errors
        }

        // GET: AppoinmentDetails/Create
        [Authorize(Roles = "patient, doctor")]
        public ActionResult Create()
        {
            ViewBag.ReportStaffID = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.CenterID = new SelectList(db.CenterDetails, "CenterId", "CenterLocation");
            ViewBag.PatientID = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: AppoinmentDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "patient, doctor")]
        public ActionResult Create([Bind(Include = "AppoinmentId,AppoinmentDate,AppoinmentTime,AppoinmentEndTime,AppoinmentStatus,PatientID,ReportStaffID,CenterID")] AppoinmentDetail appoinmentDetail)
        {
            if (ModelState.IsValid)
            {
                if (IsDuplicateAppointment(appoinmentDetail))
                {
                    ModelState.AddModelError("", "This Slot is already Booked Please Book Another Slot.");
                    // Add a custom error message and return to the Create view
                    ViewBag.ReportStaffID = new SelectList(db.AspNetUsers, "Id", "Email");
                    ViewBag.CenterID = new SelectList(db.CenterDetails, "CenterId", "CenterLocation");
                    ViewBag.PatientID = new SelectList(db.AspNetUsers, "Id", "Email");
                    return View("Create", appoinmentDetail);
                }

                db.AppoinmentDetails.Add(appoinmentDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ReportStaffID = new SelectList(db.AspNetUsers, "Id", "Email", appoinmentDetail.ReportStaffID);
            ViewBag.CenterID = new SelectList(db.CenterDetails, "CenterId", "CenterLocation", appoinmentDetail.CenterID);
            ViewBag.PatientID = new SelectList(db.AspNetUsers, "Id", "Email", appoinmentDetail.PatientID);
            return View(appoinmentDetail);
        }



        // POST: AppoinmentDetails/PatientCreate
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.


        // GET: AppoinmentDetails/Edit/5
        [Authorize(Roles = "patient, doctor")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppoinmentDetail appoinmentDetail = db.AppoinmentDetails.Find(id);
            if (appoinmentDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.ReportStaffID = new SelectList(db.AspNetUsers, "Id", "Email", appoinmentDetail.ReportStaffID);
            ViewBag.CenterID = new SelectList(db.CenterDetails, "CenterId", "CenterLocation", appoinmentDetail.CenterID);
            ViewBag.PatientID = new SelectList(db.AspNetUsers, "Id", "Email", appoinmentDetail.PatientID);
            return View(appoinmentDetail);
        }


        // POST: AppoinmentDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "patient, doctor")]
        public ActionResult Edit([Bind(Include = "AppoinmentId,AppoinmentDate,AppoinmentTime,AppoinmentEndTime,AppoinmentStatus,PatientID,ReportStaffID,CenterID")] AppoinmentDetail appoinmentDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appoinmentDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ReportStaffID = new SelectList(db.AspNetUsers, "Id", "Email", appoinmentDetail.ReportStaffID);
            ViewBag.CenterID = new SelectList(db.CenterDetails, "CenterId", "CenterLocation", appoinmentDetail.CenterID);
            ViewBag.PatientID = new SelectList(db.AspNetUsers, "Id", "Email", appoinmentDetail.PatientID);
            return View(appoinmentDetail);
        }

        // GET: AppoinmentDetails/Delete/5
        [Authorize(Roles = "patient, doctor")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppoinmentDetail appoinmentDetail = db.AppoinmentDetails.Find(id);
            if (appoinmentDetail == null)
            {
                return HttpNotFound();
            }
            return View(appoinmentDetail);
        }

        // POST: AppoinmentDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AppoinmentDetail appoinmentDetail = db.AppoinmentDetails.Find(id);
            db.AppoinmentDetails.Remove(appoinmentDetail);
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
