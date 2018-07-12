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
    public class CampaignOwnersController : Controller
    {
        private MarketingEntities db = new MarketingEntities();

        // GET: CampaignOwners
        public ActionResult Index()
        {
            return View(db.CampaignOwners.ToList());
        }

        // GET: CampaignOwners/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CampaignOwner campaignOwner = db.CampaignOwners.Find(id);
            if (campaignOwner == null)
            {
                return HttpNotFound();
            }
            return View(campaignOwner);
        }

        // GET: CampaignOwners/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CampaignOwners/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,Email,Phone")] CampaignOwner campaignOwner)
        {
            if (ModelState.IsValid)
            {
                db.CampaignOwners.Add(campaignOwner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(campaignOwner);
        }

        // GET: CampaignOwners/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CampaignOwner campaignOwner = db.CampaignOwners.Find(id);
            if (campaignOwner == null)
            {
                return HttpNotFound();
            }
            return View(campaignOwner);
        }

        // POST: CampaignOwners/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,Email,Phone")] CampaignOwner campaignOwner)
        {
            if (ModelState.IsValid)
            {
                db.Entry(campaignOwner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(campaignOwner);
        }

        // GET: CampaignOwners/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CampaignOwner campaignOwner = db.CampaignOwners.Find(id);
            if (campaignOwner == null)
            {
                return HttpNotFound();
            }
            return View(campaignOwner);
        }

        // POST: CampaignOwners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CampaignOwner campaignOwner = db.CampaignOwners.Find(id);
            db.CampaignOwners.Remove(campaignOwner);
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
