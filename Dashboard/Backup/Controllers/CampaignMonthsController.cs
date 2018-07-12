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
    public class CampaignMonthsController : Controller
    {
        private MarketingEntities db = new MarketingEntities();

        // GET: CampaignMonths
        public ActionResult Index()
        {
            var campaignMonths = db.CampaignMonths.Include(c => c.Campaign);
            return View(campaignMonths.ToList());
        }

        // GET: CampaignMonths/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CampaignMonth campaignMonth = db.CampaignMonths.Find(id);
            if (campaignMonth == null)
            {
                return HttpNotFound();
            }
            return View(campaignMonth);
        }

        // GET: CampaignMonths/Create
        public ActionResult Create()
        {
            ViewBag.CampaignID = new SelectList(db.Campaigns, "ID", "Name");
            return View();
        }

        // POST: CampaignMonths/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CampaignID,CampaignMonth1")] CampaignMonth campaignMonth)
        {
            if (ModelState.IsValid)
            {
                db.CampaignMonths.Add(campaignMonth);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CampaignID = new SelectList(db.Campaigns, "ID", "Name", campaignMonth.CampaignID);
            return View(campaignMonth);
        }

        // GET: CampaignMonths/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CampaignMonth campaignMonth = db.CampaignMonths.Find(id);
            if (campaignMonth == null)
            {
                return HttpNotFound();
            }
            ViewBag.CampaignID = new SelectList(db.Campaigns, "ID", "Name", campaignMonth.CampaignID);
            return View(campaignMonth);
        }

        // POST: CampaignMonths/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CampaignID,CampaignMonth1")] CampaignMonth campaignMonth)
        {
            if (ModelState.IsValid)
            {
                db.Entry(campaignMonth).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CampaignID = new SelectList(db.Campaigns, "ID", "Name", campaignMonth.CampaignID);
            return View(campaignMonth);
        }

        // GET: CampaignMonths/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CampaignMonth campaignMonth = db.CampaignMonths.Find(id);
            if (campaignMonth == null)
            {
                return HttpNotFound();
            }
            return View(campaignMonth);
        }

        // POST: CampaignMonths/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CampaignMonth campaignMonth = db.CampaignMonths.Find(id);
            db.CampaignMonths.Remove(campaignMonth);
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
