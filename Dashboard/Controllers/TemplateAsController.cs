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
    public class TemplateAsController : Controller
    {
        private MarketingEntities db = new MarketingEntities();

        // GET: TemplateAs
        [Authorize(Roles = "Marketing_Admin")]
        public ActionResult Index()
        {
            var templateAs = db.TemplateAs.Include(t => t.Campaign);
            return View(templateAs.ToList());
        }

        // GET: TemplateAs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TemplateA templateA = db.TemplateAs.Find(id);
            if (templateA == null)
            {
                return HttpNotFound();
            }
            return View(templateA);
        }

        // GET: TemplateAs/Create
        [Authorize(Roles = "Marketing_Admin")]
        public ActionResult Create()
        {
            ViewBag.CampaignID = new SelectList(db.Campaigns, "ID", "Name");
            return View();
        }

        // POST: TemplateAs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Marketing_Admin")]
        public ActionResult Create([Bind(Include = "ID,CampaignID,HeadLine,SubHeadLine,KeyBannerImage,IntroductionMessage,CTAText,CTALink,SecondaryCaption,Column1Image,Column1Title,Column1Message,Column1CTAText,Column1CTALink,Column2Image,Column2Title,Column2Message,Column2CTAText,Column2CTALink,Column3Image,Column3Title,Column3Message,Column3CTAText,Column3CTALink,HeadLine1,SubHeadLine1,KeyBannerImage1,IntroductionMessage1,CTAText1,CTALink1,SecondaryCaption1,Column1Image1,Column1Title1,Column1Message1,Column1CTAText1,Column1CTALink1,Column2Image1,Column2Title1,Column2Message1,Column2CTAText1,Column2CTALink1,Column3Image1,Column3Title1,Column3Message1,Column3CTAText1,Column3CTALink1")] TemplateA templateA)
        {
            if (ModelState.IsValid)
            {
                db.TemplateAs.Add(templateA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CampaignID = new SelectList(db.Campaigns, "ID", "Name", templateA.CampaignID);
            return View(templateA);
        }

        // GET: TemplateAs/Edit/5
        [Authorize(Roles = "Marketing_Admin,Marketing_Trade")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TemplateA templateA = db.TemplateAs.Find(id);
            if (templateA == null)
            {
                return HttpNotFound();
            }
            ViewBag.CampaignID = new SelectList(db.Campaigns, "ID", "Name", templateA.CampaignID);
            return View(templateA);
        }

        // POST: TemplateAs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Marketing_Admin,Marketing_Trade")]
        public ActionResult Edit([Bind(Include = "ID,CampaignID,HeadLine,SubHeadLine,KeyBannerImage,IntroductionMessage,CTAText,CTALink,SecondaryCaption,Column1Image,Column1Title,Column1Message,Column1CTAText,Column1CTALink,Column2Image,Column2Title,Column2Message,Column2CTAText,Column2CTALink,Column3Image,Column3Title,Column3Message,Column3CTAText,Column3CTALink,HeadLine1,SubHeadLine1,KeyBannerImage1,IntroductionMessage1,CTAText1,CTALink1,SecondaryCaption1,Column1Image1,Column1Title1,Column1Message1,Column1CTAText1,Column1CTALink1,Column2Image1,Column2Title1,Column2Message1,Column2CTAText1,Column2CTALink1,Column3Image1,Column3Title1,Column3Message1,Column3CTAText1,Column3CTALink1")] TemplateA templateA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(templateA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", new { @id = templateA.ID });
            }
            ViewBag.CampaignID = new SelectList(db.Campaigns, "ID", "Name", templateA.CampaignID);
            return View(templateA);
        }

        // GET: TemplateAs/Delete/5
        [Authorize(Roles = "Marketing_Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TemplateA templateA = db.TemplateAs.Find(id);
            if (templateA == null)
            {
                return HttpNotFound();
            }
            return View(templateA);
        }

        // POST: TemplateAs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Marketing_Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            TemplateA templateA = db.TemplateAs.Find(id);
            db.TemplateAs.Remove(templateA);
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
