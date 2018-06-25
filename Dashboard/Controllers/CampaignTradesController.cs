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
    public class CampaignTradesController : Controller
    {
        private MarketingEntities db = new MarketingEntities();

        // GET: CampaignTrades
        public ActionResult Index()
        {
            var campaignTrades = db.CampaignTrades.Include(c => c.Campaign).Include(c => c.Trade);
            return View(campaignTrades.ToList());
        }

        // GET: CampaignTrades/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CampaignTrade campaignTrade = db.CampaignTrades.Find(id);
            if (campaignTrade == null)
            {
                return HttpNotFound();
            }
            return View(campaignTrade);
        }

        // GET: CampaignTrades/Create
        public ActionResult Create()
        {
            ViewBag.CampaignID = new SelectList(db.Campaigns, "ID", "Name");
            ViewBag.TradeID = new SelectList(db.Trades, "ID", "Name");
            return View();
        }

        // POST: CampaignTrades/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CampaignID,TradeID")] CampaignTrade campaignTrade)
        {
            if (ModelState.IsValid)
            {
                db.CampaignTrades.Add(campaignTrade);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CampaignID = new SelectList(db.Campaigns, "ID", "Name", campaignTrade.CampaignID);
            ViewBag.TradeID = new SelectList(db.Trades, "ID", "Name", campaignTrade.TradeID);
            return View(campaignTrade);
        }

        // GET: CampaignTrades/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CampaignTrade campaignTrade = db.CampaignTrades.Find(id);
            if (campaignTrade == null)
            {
                return HttpNotFound();
            }
            ViewBag.CampaignID = new SelectList(db.Campaigns, "ID", "Name", campaignTrade.CampaignID);
            ViewBag.TradeID = new SelectList(db.Trades, "ID", "Name", campaignTrade.TradeID);
            return View(campaignTrade);
        }

        // POST: CampaignTrades/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CampaignID,TradeID")] CampaignTrade campaignTrade)
        {
            if (ModelState.IsValid)
            {
                db.Entry(campaignTrade).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CampaignID = new SelectList(db.Campaigns, "ID", "Name", campaignTrade.CampaignID);
            ViewBag.TradeID = new SelectList(db.Trades, "ID", "Name", campaignTrade.TradeID);
            return View(campaignTrade);
        }

        // GET: CampaignTrades/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CampaignTrade campaignTrade = db.CampaignTrades.Find(id);
            if (campaignTrade == null)
            {
                return HttpNotFound();
            }
            return View(campaignTrade);
        }

        // POST: CampaignTrades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CampaignTrade campaignTrade = db.CampaignTrades.Find(id);
            db.CampaignTrades.Remove(campaignTrade);
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
