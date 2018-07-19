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
    
    public class CampaignsController : Controller
    {
        private MarketingEntities db = new MarketingEntities();

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET: Campaigns
        [Authorize(Roles = "Marketing_Admin")]
        public ActionResult Index()
        {
            var campaigns = db.Campaigns.Include(c => c.CampaignType).Include(c => c.CampaignLead).Include(c => c.CampaignPlan).Include(c => c.EmailSystem).Include(c => c.Status);
            return View(campaigns.ToList());
        }

        // GET: Campaigns/Details/5
        [Authorize(Roles = "Marketing_Admin,Marketing_Trade")]
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

            var tradeData = from t in db.CampaignTrades
                join x in db.CampaignOwners on t.TradeID equals x.TradeID
                join y in db.Trades on t.TradeID equals y.ID
                select new TradeDetails()
                {
                    CampaignId = t.CampaignID,
                    TradeName = y.Name,
                    TradeDescription = y.Description,
                    Name = x.Name,
                    Email = x.Email,
                    Phone = x.Phone,
                    Role = x.Role,
                    Color = y.Color
                };

            ViewBag.Trades = db.Trades.ToList();
            ViewBag.Owners = tradeData.ToList();
            ViewBag.CampaignTypes = db.CampaignTypes.ToList();

            // Campaign Markets
            ViewBag.Markets = GetMarkets(id);

            // Campaign Accounts
            ViewBag.Accounts = GetAccounts(id);

            // campaign Products
            ViewBag.Products = db.Campaigns.FirstOrDefault(p => p.ID == id)
                ?.CampaignProducts;

            // campaign for canada also
            if (string.IsNullOrEmpty(campaign.CampaignMarket))
            {
                ViewBag.IsCanada = 1;
            }
            else
            {
                if(campaign.CampaignMarket.IndexOf("2") != -1)
                {
                    ViewBag.IsCanada = 0;
                }
                else
                {
                    ViewBag.IsCanada = 1;
                }
            }
            

            return View(campaign);
        }

        // GET: Campaigns/Create
        [Authorize(Roles = "Marketing_Admin")]
        public ActionResult Create()
        {
            ViewBag.CampaignTypeID = new SelectList(db.CampaignTypes, "ID", "Type");
            ViewBag.CampaignLeadID = new SelectList(db.CampaignLeads, "ID", "FirstName");
            ViewBag.CampaignPlanID = new SelectList(db.CampaignPlans, "ID", "PlanName");
            ViewBag.EmailSystemID = new SelectList(db.EmailSystems, "ID", "Name");
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Name");
            ViewBag.AccountSizeMultiple = new SelectList(db.AccountSizes, "ID", "AccSize");
            ViewBag.CampaignMarketMultiple = new SelectList(db.CampaignMarkets, "ID", "Market");
            return View();
        }

        // POST: Campaigns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Marketing_Admin")]
        public ActionResult Create([Bind(Include = "ID,Name,Objective,CampaignTypeID,CampaignLeadID,StatusID,EmailSystemID,CampaignPlanID,StartDate,EndDate,DateSubmitted,CampaignMarket,CampaignProducts,SuccessMetric,AccountSize,ContactPriority,Notes,CampaignProjectManager,Email,Email_Deliveries,Website_Sessions,Transactions,Email_Recipients,Open_Rate,Click_Rate,Bounce_Rate,Conversion_Rate,LandingPage,LandingUrl")] Campaign campaign)
        {
            if (ModelState.IsValid)
            {
                db.Campaigns.Add(campaign);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CampaignTypeID = new SelectList(db.CampaignTypes, "ID", "Type", campaign.CampaignTypeID);
            ViewBag.CampaignLeadID = new SelectList(db.CampaignLeads, "ID", "FirstName", campaign.CampaignLeadID);
            ViewBag.CampaignPlanID = new SelectList(db.CampaignPlans, "ID", "PlanName", campaign.CampaignPlanID);
            ViewBag.EmailSystemID = new SelectList(db.EmailSystems, "ID", "Name", campaign.EmailSystemID);
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Name", campaign.StatusID);
            ViewBag.AccountSizeMultiple = new SelectList(db.AccountSizes, "ID", "AccSize");
            ViewBag.CampaignMarketMultiple = new SelectList(db.CampaignMarkets, "ID", "Market");
            return View(campaign);           
        }

        // GET: Campaigns/Edit/5
        //[Authorize(Roles = "Marketing_Admin,Marketing_Trade")]
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
            ViewBag.CampaignLeadID = new SelectList(db.CampaignLeads, "ID", "FirstName", campaign.CampaignLeadID);
            ViewBag.CampaignPlanID = new SelectList(db.CampaignPlans, "ID", "PlanName", campaign.CampaignPlanID);
            ViewBag.EmailSystemID = new SelectList(db.EmailSystems, "ID", "Name", campaign.EmailSystemID);
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Name", campaign.StatusID);
            ViewBag.AccountSizeMultiple = new SelectList(db.AccountSizes, "ID", "AccSize");
            ViewBag.CampaignMarketMultiple = new SelectList(db.CampaignMarkets, "ID", "Market");
            ViewBag.Trades = db.Trades.ToList();

            var tradeData = from t in db.CampaignTrades
                            join x in db.CampaignOwners on t.TradeID equals x.TradeID
                            join y in db.Trades on t.TradeID equals y.ID
                            select new TradeDetails()
                            {
                                CampaignId = t.CampaignID,
                                TradeName = y.Name,
                                TradeDescription = y.Description,
                                Name = x.Name,
                                Email = x.Email,
                                Phone = x.Phone,
                                Role = x.Role,
                                Color = y.Color
                            };
            ViewBag.Owners = tradeData.ToList();

            List<CampaignTemplates> campaignTemplates = new List<CampaignTemplates>();

            var templateAs = db.TemplateAs.Where(p => p.CampaignID == id).ToList();
            foreach(var tempA in templateAs)
            {
                campaignTemplates.Add(new CampaignTemplates {
                    TemplateType = "Template A",
                    TemplateUrl = $"/TemplateAs/Edit/{tempA.ID}"
                });
            }

            var templateBs = db.TemplateBs.Where(p => p.CampaignID == id).ToList();
            foreach (var tempB in templateBs)
            {
                campaignTemplates.Add(new CampaignTemplates
                {
                    TemplateType = "Template B",
                    TemplateUrl = $"/TemplateBs/Edit/{tempB.ID}"
                });
            }

            var templateCs = db.TemplateCs.Where(p => p.CampaignID == id).ToList();
            foreach (var tempC in templateCs)
            {
                campaignTemplates.Add(new CampaignTemplates
                {
                    TemplateType = "Template C",
                    TemplateUrl = $"/TemplateCs/Edit/{tempC.ID}"
                });
            }
            ViewBag.CurrentTemplates = campaignTemplates;

            //if (User.IsInRole("Marketing_Admin"))
            //    return View("Edit", campaign);
            //return View("ViewCampaign", campaign);
            return View("Edit", campaign);

        }

        // POST: Campaigns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Marketing_Admin")]
        public ActionResult Edit([Bind(Include = "ID,Name,Objective,CampaignTypeID,CampaignLeadID,StatusID,EmailSystemID,CampaignPlanID,StartDate,EndDate,DateSubmitted,CampaignMarket,CampaignProducts,SuccessMetric,AccountSize,ContactPriority,Notes,CampaignProjectManager,Email,Email_Deliveries,Website_Sessions,Transactions,Email_Recipients,Open_Rate,Click_Rate,Bounce_Rate,Conversion_Rate,LandingPage,LandingUrl,USGraphWeeks,USGraphWeekRevenue,USGraphWeekUnits,CAGraphWeeks,CAGraphWeekRevenue,CAGraphWeekUnits,Email_Status,TemplateType,JobFunctions")] Campaign campaign)
        {
            if (ModelState.IsValid)
            {
                db.Entry(campaign).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Edit");
                ViewBag.CampaignTypeID = new SelectList(db.CampaignTypes, "ID", "Type", campaign.CampaignTypeID);
                ViewBag.CampaignLeadID = new SelectList(db.CampaignLeads, "ID", "FirstName", campaign.CampaignLeadID);
                ViewBag.CampaignPlanID = new SelectList(db.CampaignPlans, "ID", "PlanName", campaign.CampaignPlanID);
                ViewBag.EmailSystemID = new SelectList(db.EmailSystems, "ID", "Name", campaign.EmailSystemID);
                ViewBag.StatusID = new SelectList(db.Status, "ID", "Name", campaign.StatusID);
                ViewBag.AccountSizeMultiple = new SelectList(db.AccountSizes, "ID", "AccSize");
                ViewBag.CampaignMarketMultiple = new SelectList(db.CampaignMarkets, "ID", "Market");
                ViewBag.Trades = db.Trades.ToList();
                var tradeData = from t in db.CampaignTrades
                                join x in db.CampaignOwners on t.TradeID equals x.TradeID
                                join y in db.Trades on t.TradeID equals y.ID
                                select new TradeDetails()
                                {
                                    CampaignId = t.CampaignID,
                                    TradeName = y.Name,
                                    TradeDescription = y.Description,
                                    Name = x.Name,
                                    Email = x.Email,
                                    Phone = x.Phone,
                                    Role = x.Role,
                                    Color = y.Color
                                };
                ViewBag.Owners = tradeData.ToList();
                return View(campaign);
            }
            ViewBag.CampaignTypeID = new SelectList(db.CampaignTypes, "ID", "Type", campaign.CampaignTypeID);
            ViewBag.CampaignLeadID = new SelectList(db.CampaignLeads, "ID", "FirstName", campaign.CampaignLeadID);
            ViewBag.CampaignPlanID = new SelectList(db.CampaignPlans, "ID", "PlanName", campaign.CampaignPlanID);
            ViewBag.EmailSystemID = new SelectList(db.EmailSystems, "ID", "Name", campaign.EmailSystemID);
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Name", campaign.StatusID);
            ViewBag.AccountSizeMultiple = new SelectList(db.AccountSizes, "ID", "AccSize");
            ViewBag.CampaignMarketMultiple = new SelectList(db.CampaignMarkets, "ID", "Market");
            return View(campaign);
        }

        // GET: Campaigns/Delete/5
        [Authorize(Roles = "Marketing_Admin")]
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
        [Authorize(Roles = "Marketing_Admin")]
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
        private string GetMarkets(int? id)
        {
            List<string> marketMatches = new List<string>();
            var campMarkets = db.Campaigns.FirstOrDefault(p => p.ID == id)
                ?.CampaignMarket;

            if (campMarkets != null)
            {
                if (campMarkets.ToString().Contains(","))
                {
                    var markInts = campMarkets.Split(',').Select(int.Parse);
                    var mMatches = from t in db.CampaignMarkets
                                   where markInts.Contains(t.ID)
                                   select new
                                   {
                                       t.Market
                                   };
                    foreach (var match in mMatches)
                    {
                        marketMatches.Add(match.Market);
                    }

                }
                else
                {
                    var markInt = Convert.ToInt32(campMarkets);
                    var mMatches = from t in db.CampaignMarkets
                                   where t.ID == markInt
                                   select new
                                   {
                                       t.Market
                                   };

                    foreach (var match in mMatches)
                    {
                        marketMatches.Add(match.Market);
                    }
                }
            }

            return string.Join(", ", marketMatches);
        }

        private string GetAccounts(int? id)
        {
            List<string> marketMatches = new List<string>();
            var campMarkets = db.Campaigns.FirstOrDefault(p => p.ID == id)
                ?.AccountSize;

            if (campMarkets != null)
            {
                if (campMarkets.ToString().Contains(","))
                {
                    var markInts = campMarkets.Split(',').Select(int.Parse);
                    var mMatches = from t in db.AccountSizes
                                   where markInts.Contains(t.ID)
                                   select new
                                   {
                                       t.AccSize
                                   };
                    foreach (var match in mMatches)
                    {
                        marketMatches.Add(match.AccSize);
                    }

                }
                else
                {
                    var markInt = Convert.ToInt32(campMarkets);
                    var mMatches = from t in db.AccountSizes
                                   where t.ID == markInt
                                   select new
                                   {
                                       t.AccSize
                                   };

                    foreach (var match in mMatches)
                    {
                        marketMatches.Add(match.AccSize);
                    }
                }
            }

            return string.Join(", ", marketMatches);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            //Log the error!!
            log.Error(filterContext.Exception);

            //Redirect or return a view, but not both.
            filterContext.Result = RedirectToAction("Index", "ErrorHandler");
            
        }

        public JsonResult GetUsGraph(int? id)
        {
            string json = string.Empty;
            try {
                if(db.Campaigns.Find(id).USGraphWeekRevenue!= null && db.Campaigns.Find(id).USGraphWeekUnits != null)
                {
                    string[] revenue = db.Campaigns.Find(id).USGraphWeekRevenue.Split('|');
                    int[] counts = Array.ConvertAll(db.Campaigns.Find(id).USGraphWeekUnits.Split(','), s => int.Parse(s));
                    string[] plotBands = revenue[6].Split(',');
                    json = "[{\"name\":\"Time\",\"data\":[" + db.Campaigns.Find(id).USGraphWeeks +
                        "]},{\"name\":\"Total\",\"data\":[" + revenue[0] +
                        "]},{\"name\":\"Portfolio\",\"data\":[" + revenue[1] +
                        "]},{\"name\":\"Online\",\"data\":[" + revenue[2] +
                        "]},{\"name\":\"AM\",\"data\":[" + revenue[3] +
                        "]},{\"name\":\"HS\",\"data\":[" + revenue[4] +
                        "]},{\"name\":\"CS\",\"data\":[" + revenue[5] +
                        "]},{\"name\":\"CS\",\"data\":[" + counts.Sum() +
                        "]},{\"name\":\"PB1\",\"data\":" + plotBands[0] +
                        "},{\"name\":\"PB2\",\"data\":" + plotBands[1] +
                        "},{\"name\":\"Items\",\"data\":[" + db.Campaigns.Find(id).USGraphWeekUnits + "]}]";
                }
                return new JsonResult { Data = json, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch(Exception e)
            {
                return new JsonResult { Data = json, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            
        }

        public JsonResult GetCaGraph(int? id)
        {
            string json = string.Empty;
            try
            {
                if (db.Campaigns.Find(id).CAGraphWeekRevenue != null && db.Campaigns.Find(id).CAGraphWeekUnits != null)
                {
                    string[] revenue = db.Campaigns.Find(id).CAGraphWeekRevenue.Split('|');
                    int[] counts = Array.ConvertAll(db.Campaigns.Find(id).CAGraphWeekUnits.Split(','), s => int.Parse(s));
                    string[] plotBands = revenue[6].Split(',');
                    json = "[{\"name\":\"Time\",\"data\":[" + db.Campaigns.Find(id).CAGraphWeeks +
                        "]},{\"name\":\"Total\",\"data\":[" + revenue[0] +
                        "]},{\"name\":\"Portfolio\",\"data\":[" + revenue[1] +
                        "]},{\"name\":\"Online\",\"data\":[" + revenue[2] +
                        "]},{\"name\":\"AM\",\"data\":[" + revenue[3] +
                        "]},{\"name\":\"HS\",\"data\":[" + revenue[4] +
                        "]},{\"name\":\"CS\",\"data\":[" + revenue[5] +
                        "]},{\"name\":\"CS\",\"data\":[" + counts.Sum() +
                        "]},{\"name\":\"PB1\",\"data\":" + plotBands[0] +
                        "},{\"name\":\"PB2\",\"data\":" + plotBands[1] +
                        "},{\"name\":\"Items\",\"data\":[" + db.Campaigns.Find(id).CAGraphWeekUnits + "]}]";
                }
                return new JsonResult { Data = json, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception e)
            {
                return new JsonResult { Data = json, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }

        public int GetTemplateAUrl(int? id)
        {
            int templateId = 0;
            if (db.TemplateAs.Where(p => p.CampaignID == id).Count() != 0)
            {
                templateId = db.TemplateAs.Where(p => p.CampaignID == id).FirstOrDefault().ID;
            }
            return templateId;
        }

            
        public int CreateTemplateA(int? id)
        {
            var record = db.TemplateAs.Add(new TemplateA
            {
                CampaignID = id
            });
            db.SaveChanges();
            return record.ID;
        }

        public int GetTemplateBUrl(int? id)
        {
            int templateId = 0;
            if (db.TemplateBs.Where(p => p.CampaignID == id).Count() != 0)
            {
                templateId = db.TemplateBs.Where(p => p.CampaignID == id).FirstOrDefault().ID;
            }
            return templateId;
        }


        public int CreateTemplateB(int? id)
        {
            var record = db.TemplateBs.Add(new TemplateB
            {
                CampaignID = id
            });
            db.SaveChanges();
            return record.ID;
        }

        public int GetTemplateCUrl(int? id)
        {
            int templateId = 0;
            if (db.TemplateCs.Where(p => p.CampaignID == id).Count() != 0)
            {
                templateId = db.TemplateCs.Where(p => p.CampaignID == id).FirstOrDefault().ID;
            }
            return templateId;
        }


        public int CreateTemplateC(int? id)
        {
            var record = db.TemplateCs.Add(new TemplateC
            {
                CampaignID = id
            });
            db.SaveChanges();
            return record.ID;
        }
    }

    public class CampaignTemplates
    {
        public string TemplateType { get; set; }
        public string TemplateUrl { get; set; }

        public static implicit operator List<object>(CampaignTemplates v)
        {
            throw new NotImplementedException();
        }
    }
}
