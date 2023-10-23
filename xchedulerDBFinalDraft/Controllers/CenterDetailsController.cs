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
    public class CenterDetailsController : Controller
    {
        private Entities db = new Entities();

        // GET: CenterDetails
        [Authorize]
        public ActionResult Index()
        {
            return View(db.CenterDetails.ToList());
        }

        // GET: CenterDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CenterDetail centerDetail = db.CenterDetails.Find(id);
            if (centerDetail == null)
            {
                return HttpNotFound();
            }
            return View(centerDetail);
        }

        // GET: CenterDetails/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: CenterDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Create([Bind(Include = "CenterId,CenterLocation,Suburb,Latitude,Longitude")] CenterDetail centerDetail)
        {
            if (ModelState.IsValid)
            {
                db.CenterDetails.Add(centerDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(centerDetail);
        }

        // GET: CenterDetails/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CenterDetail centerDetail = db.CenterDetails.Find(id);
            if (centerDetail == null)
            {
                return HttpNotFound();
            }
            return View(centerDetail);
        }

        // POST: CenterDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Edit([Bind(Include = "CenterId,CenterLocation,Suburb,Latitude,Longitude")] CenterDetail centerDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(centerDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(centerDetail);
        }

        // GET: CenterDetails/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CenterDetail centerDetail = db.CenterDetails.Find(id);
            if (centerDetail == null)
            {
                return HttpNotFound();
            }
            return View(centerDetail);
        }

        // POST: CenterDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CenterDetail centerDetail = db.CenterDetails.Find(id);
            db.CenterDetails.Remove(centerDetail);
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
