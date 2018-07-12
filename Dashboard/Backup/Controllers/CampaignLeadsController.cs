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
    public class CampaignLeadsController : Controller
    {
        private MarketingEntities db = new MarketingEntities();

        // GET: CampaignLeads
        public ActionResult Index()
        {
            return View(db.CampaignLeads.ToList());
        }

        // GET: CampaignLeads/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CampaignLead campaignLead = db.CampaignLeads.Find(id);
            if (campaignLead == null)
            {
                return HttpNotFound();
            }
            return View(campaignLead);
        }

        // GET: CampaignLeads/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CampaignLeads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,Email,Phone")] CampaignLead campaignLead)
        {
            if (ModelState.IsValid)
            {
                db.CampaignLeads.Add(campaignLead);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(campaignLead);
        }

        // GET: CampaignLeads/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CampaignLead campaignLead = db.CampaignLeads.Find(id);
            if (campaignLead == null)
            {
                return HttpNotFound();
            }
            return View(campaignLead);
        }

        // POST: CampaignLeads/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,Email,Phone")] CampaignLead campaignLead)
        {
            if (ModelState.IsValid)
            {
                db.Entry(campaignLead).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(campaignLead);
        }

        // GET: CampaignLeads/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CampaignLead campaignLead = db.CampaignLeads.Find(id);
            if (campaignLead == null)
            {
                return HttpNotFound();
            }
            return View(campaignLead);
        }

        // POST: CampaignLeads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CampaignLead campaignLead = db.CampaignLeads.Find(id);
            db.CampaignLeads.Remove(campaignLead);
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
