using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dashboard.Models;

namespace Dashboard.Controllers
{
    [Authorize(Roles = "Marketing_Admin")]
    public class EmailSystemsController : Controller
    {
        private MarketingEntities db = new MarketingEntities();

        // GET: EmailSystems
        public ActionResult Index()
        {
            return View(db.EmailSystems.ToList());
        }

        // GET: EmailSystems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmailSystem emailSystem = db.EmailSystems.Find(id);
            if (emailSystem == null)
            {
                return HttpNotFound();
            }
            return View(emailSystem);
        }

        // GET: EmailSystems/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmailSystems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] EmailSystem emailSystem)
        {
            if (ModelState.IsValid)
            {
                db.EmailSystems.Add(emailSystem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(emailSystem);
        }

        // GET: EmailSystems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmailSystem emailSystem = db.EmailSystems.Find(id);
            if (emailSystem == null)
            {
                return HttpNotFound();
            }
            return View(emailSystem);
        }

        // POST: EmailSystems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] EmailSystem emailSystem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(emailSystem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(emailSystem);
        }

        // GET: EmailSystems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmailSystem emailSystem = db.EmailSystems.Find(id);
            if (emailSystem == null)
            {
                return HttpNotFound();
            }
            return View(emailSystem);
        }

        // POST: EmailSystems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmailSystem emailSystem = db.EmailSystems.Find(id);
            db.EmailSystems.Remove(emailSystem);
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
