using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dashboard.Models;
using Microsoft.Ajax.Utilities;

namespace Dashboard.Controllers
{
    public class CampaignsController : Controller
    {
        private MarketingEntities db = new MarketingEntities();

        public List<CalenderYear> calenderYear = new List<CalenderYear>();

        // GET: Campaigns
        public ActionResult Index()
        {
            var campaigns = db.Campaigns.Include(c => c.CampaignType).Include(c => c.CampaignOwner).Include(c => c.CampaignLead).Include(c => c.EmailSystem).Include(c => c.Status);
            return View(campaigns.ToList());
        }

        public ActionResult Schedule(int? id)
        {
            // Initial setup for calender layout
            calenderYear.Clear();
            var months = new List<int>();

            for (var i = 0; i < 24; i++) calenderYear.Add(new CalenderYear());

            if (id == null)
            {
                foreach (var camp in db.Campaigns)
                {
                    months.Clear();
                    foreach (var month in db.CampaignMonths.Where(p => p.CampaignID == camp.ID).OrderBy(p => p.CampaignMonth1)) months.Add(month.CampaignMonth1);
                    GetCurrentRow(calenderYear, camp.ID, months);
                }
            }
            else
            {
                foreach (var camp in db.CampaignTrades.Where(p => p.TradeID == id).AsEnumerable().DistinctBy(p => p.CampaignID))
                {
                    months.Clear();
                    foreach (var month in db.CampaignMonths.Where(p => p.CampaignID == camp.ID).OrderBy(p => p.CampaignMonth1)) months.Add(month.CampaignMonth1);
                    GetCurrentRow(calenderYear, camp.ID, months);
                }
            }

            
            ViewBag.CalenderYear = calenderYear;

            ViewBag.Trades = db.Trades.ToList();

            var campaigns = db.Campaigns.Include(c => c.CampaignType).Include(c => c.CampaignOwner).Include(c => c.CampaignLead).Include(c => c.EmailSystem).Include(c => c.Status);
            return View(campaigns.ToList());
        }

        private void GetCurrentRow(List<CalenderYear> calenderYear, int campaignID, List<int> months)
        {

            foreach (var i in months)
                switch (i)
                {
                    case 1:
                        foreach (var cy in calenderYear)
                            if (cy.Jan == 0)
                            {
                                cy.Jan = campaignID;
                                break;
                            }

                        break;
                    case 2:
                        foreach (var cy in calenderYear)
                            if (cy.Feb == 0)
                            {
                                cy.Feb = campaignID;
                                break;
                            }

                        break;
                    case 3:
                        foreach (var cy in calenderYear)
                            if (cy.Mar == 0)
                            {
                                cy.Mar = campaignID;
                                break;
                            }

                        break;
                    case 4:
                        foreach (var cy in calenderYear)
                            if (cy.Apr == 0)
                            {
                                cy.Apr = campaignID;
                                break;
                            }

                        break;
                    case 5:
                        foreach (var cy in calenderYear)
                            if (cy.May == 0)
                            {
                                cy.May = campaignID;
                                break;
                            }

                        break;
                    case 6:
                        foreach (var cy in calenderYear)
                            if (cy.Jun == 0)
                            {
                                cy.Jun = campaignID;
                                break;
                            }

                        break;
                    case 7:
                        foreach (var cy in calenderYear)
                            if (cy.Jul == 0)
                            {
                                cy.Jul = campaignID;
                                break;
                            }

                        break;
                    case 8:
                        foreach (var cy in calenderYear)
                            if (cy.Aug == 0)
                            {
                                cy.Aug = campaignID;
                                break;
                            }

                        break;
                    case 9:
                        foreach (var cy in calenderYear)
                            if (cy.Sep == 0)
                            {
                                cy.Sep = campaignID;
                                break;
                            }

                        break;
                    case 10:
                        foreach (var cy in calenderYear)
                            if (cy.Oct == 0)
                            {
                                cy.Oct = campaignID;
                                break;
                            }

                        break;
                    case 11:
                        foreach (var cy in calenderYear)
                            if (cy.Nov == 0)
                            {
                                cy.Nov = campaignID;
                                break;
                            }

                        break;
                    case 12:
                        foreach (var cy in calenderYear)
                            if (cy.Dec == 0)
                            {
                                cy.Dec = campaignID;
                                break;
                            }

                        break;
                }
        }

        // GET: Campaigns/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Campaign campaign = db.Campaigns.Find(id);
            if (campaign == null)
            {
                return HttpNotFound();
            }
            return View(campaign);
        }

        // GET: Campaigns/Create
        public ActionResult Create()
        {
            ViewBag.CampaignTypeID = new SelectList(db.CampaignTypes, "ID", "Type");
            ViewBag.CampaignOwnerID = new SelectList(db.CampaignOwners, "ID", "FirstName");
            ViewBag.CampaignLeadID = new SelectList(db.CampaignLeads, "ID", "FirstName");
            ViewBag.EmailSystemID = new SelectList(db.EmailSystems, "ID", "Name");
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Name");
            return View();
        }

        // POST: Campaigns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Objective,CampaignTypeID,CampaignOwnerID,CampaignLeadID,StatusID,EmailSystemID")] Campaign campaign)
        {
            if (ModelState.IsValid)
            {
                db.Campaigns.Add(campaign);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CampaignTypeID = new SelectList(db.CampaignTypes, "ID", "Type", campaign.CampaignTypeID);
            ViewBag.CampaignOwnerID = new SelectList(db.CampaignOwners, "ID", "FirstName", campaign.CampaignOwnerID);
            ViewBag.CampaignLeadID = new SelectList(db.CampaignLeads, "ID", "FirstName", campaign.CampaignLeadID);
            ViewBag.EmailSystemID = new SelectList(db.EmailSystems, "ID", "Name", campaign.EmailSystemID);
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Name", campaign.StatusID);
            return View(campaign);
        }

        // GET: Campaigns/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Campaign campaign = db.Campaigns.Find(id);
            if (campaign == null)
            {
                return HttpNotFound();
            }
            ViewBag.CampaignTypeID = new SelectList(db.CampaignTypes, "ID", "Type", campaign.CampaignTypeID);
            ViewBag.CampaignOwnerID = new SelectList(db.CampaignOwners, "ID", "FirstName", campaign.CampaignOwnerID);
            ViewBag.CampaignLeadID = new SelectList(db.CampaignLeads, "ID", "FirstName", campaign.CampaignLeadID);
            ViewBag.EmailSystemID = new SelectList(db.EmailSystems, "ID", "Name", campaign.EmailSystemID);
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Name", campaign.StatusID);
            return View(campaign);
        }

        // POST: Campaigns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Objective,CampaignTypeID,CampaignOwnerID,CampaignLeadID,StatusID,EmailSystemID")] Campaign campaign)
        {
            if (ModelState.IsValid)
            {
                db.Entry(campaign).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CampaignTypeID = new SelectList(db.CampaignTypes, "ID", "Type", campaign.CampaignTypeID);
            ViewBag.CampaignOwnerID = new SelectList(db.CampaignOwners, "ID", "FirstName", campaign.CampaignOwnerID);
            ViewBag.CampaignLeadID = new SelectList(db.CampaignLeads, "ID", "FirstName", campaign.CampaignLeadID);
            ViewBag.EmailSystemID = new SelectList(db.EmailSystems, "ID", "Name", campaign.EmailSystemID);
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Name", campaign.StatusID);
            return View(campaign);
        }

        // GET: Campaigns/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Campaign campaign = db.Campaigns.Find(id);
            if (campaign == null)
            {
                return HttpNotFound();
            }
            return View(campaign);
        }

        // POST: Campaigns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Campaign campaign = db.Campaigns.Find(id);
            db.Campaigns.Remove(campaign);
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
    public class CalenderYear
    {
        public int Jan { get; set; } = 0;
        public int Feb { get; set; } = 0;
        public int Mar { get; set; } = 0;
        public int Apr { get; set; } = 0;
        public int May { get; set; } = 0;
        public int Jun { get; set; } = 0;
        public int Jul { get; set; } = 0;
        public int Aug { get; set; } = 0;
        public int Sep { get; set; } = 0;
        public int Oct { get; set; } = 0;
        public int Nov { get; set; } = 0;
        public int Dec { get; set; } = 0;
    }
}
