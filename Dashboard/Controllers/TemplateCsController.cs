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
    public class TemplateCsController : Controller
    {
        private MarketingEntities db = new MarketingEntities();

        // GET: TemplateCs
        [Authorize(Roles = "Marketing_Admin")]
        public ActionResult Index()
        {
            var templateCs = db.TemplateCs.Include(t => t.Campaign);
            return View(templateCs.ToList());
        }

        // GET: TemplateCs/Details/5
        [Authorize(Roles = "Marketing_Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TemplateC templateC = db.TemplateCs.Find(id);
            if (templateC == null)
            {
                return HttpNotFound();
            }
            return View(templateC);
        }

        // GET: TemplateCs/Create
        [Authorize(Roles = "Marketing_Admin")]
        public ActionResult Create()
        {
            ViewBag.CampaignID = new SelectList(db.Campaigns, "ID", "Name");
            return View();
        }

        // POST: TemplateCs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CampaignID,HeadLine,SubHeadLine,KeyBannerImage,IntroductionMessage,CTAText,CTALink,SecondaryCaption,Column1Image,Column1Title,Column1Message,Column1CTAText,Column1CTALink,Column2Image,Column2Title,Column2Message,Column2CTAText,Column2CTALink")] TemplateC templateC)
        {
            if (ModelState.IsValid)
            {
                db.TemplateCs.Add(templateC);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CampaignID = new SelectList(db.Campaigns, "ID", "Name", templateC.CampaignID);
            return View(templateC);
        }

        // GET: TemplateCs/Edit/5
        [Authorize(Roles = "Marketing_Admin,Marketing_Trade")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TemplateC templateC = db.TemplateCs.Find(id);
            if (templateC == null)
            {
                return HttpNotFound();
            }
            ViewBag.CampaignID = new SelectList(db.Campaigns, "ID", "Name", templateC.CampaignID);
            return View(templateC);
        }

        // POST: TemplateCs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Marketing_Admin,Marketing_Trade")]
        public ActionResult Edit([Bind(Include = "ID,CampaignID,HeadLine,SubHeadLine,KeyBannerImage,IntroductionMessage,CTAText,CTALink,SecondaryCaption,Column1Image,Column1Title,Column1Message,Column1CTAText,Column1CTALink,Column2Image,Column2Title,Column2Message,Column2CTAText,Column2CTALink")] TemplateC templateC)
        {
            if (ModelState.IsValid)
            {
                db.Entry(templateC).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", new { @id = templateC.ID });
            }
            ViewBag.CampaignID = new SelectList(db.Campaigns, "ID", "Name", templateC.CampaignID);
            return View(templateC);
        }

        // GET: TemplateCs/Delete/5
        [Authorize(Roles = "Marketing_Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TemplateC templateC = db.TemplateCs.Find(id);
            if (templateC == null)
            {
                return HttpNotFound();
            }
            return View(templateC);
        }

        // POST: TemplateCs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Marketing_Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            TemplateC templateC = db.TemplateCs.Find(id);
            db.TemplateCs.Remove(templateC);
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
