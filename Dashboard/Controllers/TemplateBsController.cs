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
    public class TemplateBsController : Controller
    {
        private MarketingEntities db = new MarketingEntities();

        // GET: TemplateBs
        [Authorize(Roles = "Marketing_Admin")]
        public ActionResult Index()
        {
            var templateBs = db.TemplateBs.Include(t => t.Campaign);
            return View(templateBs.ToList());
        }

        // GET: TemplateBs/Details/5
        [Authorize(Roles = "Marketing_Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TemplateB templateB = db.TemplateBs.Find(id);
            if (templateB == null)
            {
                return HttpNotFound();
            }
            return View(templateB);
        }

        // GET: TemplateBs/Create
        [Authorize(Roles = "Marketing_Admin")]
        public ActionResult Create()
        {
            ViewBag.CampaignID = new SelectList(db.Campaigns, "ID", "Name");
            return View();
        }

        // POST: TemplateBs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Marketing_Admin")]
        public ActionResult Create([Bind(Include = "ID,CampaignID,HeadLine,SubHeadLine,KeyBannerImage,IntroductionMessage,CTAText,CTALink,SecondaryCaption,Column1Image,Column1Title,Column1Message,Column1CTAText,Column1CTALink")] TemplateB templateB)
        {
            if (ModelState.IsValid)
            {
                db.TemplateBs.Add(templateB);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CampaignID = new SelectList(db.Campaigns, "ID", "Name", templateB.CampaignID);
            return View(templateB);
        }

        // GET: TemplateBs/Edit/5
        [Authorize(Roles = "Marketing_Admin,Marketing_Trade")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TemplateB templateB = db.TemplateBs.Find(id);
            if (templateB == null)
            {
                return HttpNotFound();
            }
            ViewBag.CampaignID = new SelectList(db.Campaigns, "ID", "Name", templateB.CampaignID);
            return View(templateB);
        }

        // POST: TemplateBs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Marketing_Admin,Marketing_Trade")]
        public ActionResult Edit([Bind(Include = "ID,CampaignID,HeadLine,SubHeadLine,KeyBannerImage,IntroductionMessage,CTAText,CTALink,SecondaryCaption,Column1Image,Column1Title,Column1Message,Column1CTAText,Column1CTALink")] TemplateB templateB)
        {
            if (ModelState.IsValid)
            {
                db.Entry(templateB).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", new { @id = templateB.ID });
            }
            ViewBag.CampaignID = new SelectList(db.Campaigns, "ID", "Name", templateB.CampaignID);
            return View(templateB);
        }

        // GET: TemplateBs/Delete/5
        [Authorize(Roles = "Marketing_Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TemplateB templateB = db.TemplateBs.Find(id);
            if (templateB == null)
            {
                return HttpNotFound();
            }
            return View(templateB);
        }

        // POST: TemplateBs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Marketing_Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            TemplateB templateB = db.TemplateBs.Find(id);
            db.TemplateBs.Remove(templateB);
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
