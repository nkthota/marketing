using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dashboard.Models;

namespace Dashboard.Controllers
{
    public class RoadMapController : Controller
    {
        private MarketingEntities _db = new MarketingEntities();

        private List<CalenderYear> CalenderYear = new List<CalenderYear>();

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [Authorize(Roles = "Marketing_Admin,Marketing_Trade")]
        public ActionResult Index(int? id, int? type)
        {
            // Initial setup for calender layout
            CalenderYear.Clear();   
            var months = new List<int>();

            var currentTrade = "All Trades";
            var currentScheduleType = "All Campaign Types";

            for (var i = 0; i < 24; i++) CalenderYear.Add(new CalenderYear());

            if (id == null)
            {
                if (type == null)
                {
                    foreach (var camp in _db.Campaigns)
                    {
                        months.Clear();
                        foreach (var month in _db.CampaignMonths.Where(p => p.CampaignID == camp.ID).OrderBy(p => p.Month)) months.Add(month.Month);
                        GetCurrentRow(CalenderYear, camp.ID, months);
                    }
                }
                else
                {
                    var campaignFilter = _db.vCampaignTypeMonths.Where(p => (p.CampaignTypeID == type)).GroupBy(p => p.CampaignID).Select(p => p.FirstOrDefault());

                    //foreach (var camp in _db.Campaigns)
                    //{
                    //    months.Clear();
                    //    var campgainFilter = from t in _db.CampaignMonths
                    //                         join x in _db.Campaigns on t.CampaignID equals x.ID
                    //                         where x.CampaignTypeID == type
                    //                         select t;
                    //    foreach (var month in campgainFilter.Where(p => p.CampaignID == camp.ID).OrderBy(p => p.Month)) months.Add(month.Month);
                    //    GetCurrentRow(CalenderYear, camp.ID, months);
                    //}

                    foreach (var camp in campaignFilter)
                    {
                        months.Clear();
                        var campMonths = _db.CampaignMonths.Where(p => p.CampaignID == camp.CampaignID).OrderBy(p => p.Month);
                        foreach (var m in campMonths)
                        {
                            months.Add(m.Month);
                        }
                        GetCurrentRow(CalenderYear, camp.CampaignID, months);
                    }

                    currentScheduleType =
                        _db.CampaignTypes.FirstOrDefault(p => p.ID == type)?.Type;

                }

            }
            else if (id == 0)
            {
                //var campaignFilter = from t in _db.Campaigns
                //                     join y in _db.CampaignMonths on t.ID equals y.CampaignID
                //                     where t.CampaignTypeID == type && y.Month == 1
                //                     select y;


                var campaignFilter = _db.vCampaignTypeMonths.Where(p => (p.CampaignTypeID == type)).GroupBy(p => p.CampaignID).Select(p => p.FirstOrDefault());

                //foreach (var camp in campaignFilter)
                //{
                //    months.Clear();
                //    foreach (var month in campaignFilter.Where(p => p.CampaignID == camp.ID).OrderBy(p => p.Month))
                //    {
                //        months.Add(month.Month);
                //    }

                //    GetCurrentRow(CalenderYear, camp.ID, months);
                //}

                foreach(var camp in campaignFilter)
                {
                    months.Clear();
                    var campMonths = _db.CampaignMonths.Where(p => p.CampaignID == camp.CampaignID).OrderBy(p => p.Month);
                    foreach(var m in campMonths)
                    {
                        months.Add(m.Month);
                    }
                    GetCurrentRow(CalenderYear, camp.CampaignID, months);
                }

                currentScheduleType =
                    _db.CampaignTypes.FirstOrDefault(p => p.ID == type)?.Type;
            }
            else
            {

                if (type == null)
                {
                    foreach (var camp in _db.CampaignTrades.Where(p => p.TradeID == id))
                    {
                        months.Clear();
                        foreach (var month in _db.CampaignMonths.Where(p => p.CampaignID == camp.CampaignID).OrderBy(p => p.Month)) months.Add(month.Month);
                        GetCurrentRow(CalenderYear, camp.CampaignID, months);
                    }

                    currentTrade = _db.Trades.FirstOrDefault(p => p.ID == id)?.Name;
                }
                else
                {

                    //var campaignFilter = from t in _db.Campaigns
                    //                     join x in _db.CampaignTrades on t.ID equals x.CampaignID
                    //                     join y in _db.CampaignMonths on t.ID equals y.CampaignID
                    //                     where t.CampaignTypeID == type && x.TradeID == id
                    //                     select y;

                    //foreach (var camp in campaignFilter)
                    //{
                    //    months.Clear();
                    //    foreach (var month in campaignFilter.Where(p => p.CampaignID == camp.ID).OrderBy(p => p.Month)) months.Add(month.Month);
                    //    GetCurrentRow(CalenderYear, camp.ID, months);
                    //}

                    var campaignFilter = _db.vCampaignTradeMonths.Where(p => (p.CampaignTypeID == type && p.TradeID == id)).GroupBy(p => p.ID).Select(p => p.FirstOrDefault());

                    foreach (var camp in campaignFilter)
                    {
                        months.Clear();
                        var campMonths = _db.CampaignMonths.Where(p => p.CampaignID == camp.ID).OrderBy(p => p.Month);
                        foreach (var m in campMonths)
                        {
                            months.Add(m.Month);
                        }
                        GetCurrentRow(CalenderYear, camp.ID, months);
                    }

                    currentScheduleType =
                        _db.CampaignTypes.FirstOrDefault(p => p.ID == type)?.Type;

                    currentTrade = _db.Trades.FirstOrDefault(p => p.ID == id)?.Name;

                }


            }

            var tradeData = from t in _db.CampaignTrades
                            join x in _db.CampaignOwners on t.TradeID equals x.TradeID
                            join y in _db.Trades on t.TradeID equals y.ID
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


            ViewBag.CalenderYear = CalenderYear;
            ViewBag.Trades = _db.Trades.ToList();
            ViewBag.Owners = tradeData.ToList();
            ViewBag.CampaignTypes = _db.CampaignTypes.ToList();
            ViewBag.ScheduleFilters = new ScheduleFilter()
            {
                TradeName = currentTrade,
                CampaignType = currentScheduleType
            };

            var campaigns = _db.Campaigns.Include(c => c.CampaignType).Include(c => c.CampaignLead).Include(c => c.EmailSystem).Include(c => c.Status);
            return View(campaigns.ToList());
        }

        private void GetCurrentRow(List<CalenderYear> calenderYear, int campaignId, List<int> months)
        {

            foreach (var i in months)
                switch (i)
                {
                    case 1:
                        foreach (var cy in calenderYear)
                            if (cy.Jan == 0)
                            {
                                cy.Jan = campaignId;
                                break;
                            }

                        break;
                    case 2:
                        foreach (var cy in calenderYear)
                            if (cy.Feb == 0)
                            {
                                cy.Feb = campaignId;
                                break;
                            }

                        break;
                    case 3:
                        foreach (var cy in calenderYear)
                            if (cy.Mar == 0)
                            {
                                cy.Mar = campaignId;
                                break;
                            }

                        break;
                    case 4:
                        foreach (var cy in calenderYear)
                            if (cy.Apr == 0)
                            {
                                cy.Apr = campaignId;
                                break;
                            }

                        break;
                    case 5:
                        foreach (var cy in calenderYear)
                            if (cy.May == 0)
                            {
                                cy.May = campaignId;
                                break;
                            }

                        break;
                    case 6:
                        foreach (var cy in calenderYear)
                            if (cy.Jun == 0)
                            {
                                cy.Jun = campaignId;
                                break;
                            }

                        break;
                    case 7:
                        foreach (var cy in calenderYear)
                            if (cy.Jul == 0)
                            {
                                cy.Jul = campaignId;
                                break;
                            }

                        break;
                    case 8:
                        foreach (var cy in calenderYear)
                            if (cy.Aug == 0)
                            {
                                cy.Aug = campaignId;
                                break;
                            }

                        break;
                    case 9:
                        foreach (var cy in calenderYear)
                            if (cy.Sep == 0)
                            {
                                cy.Sep = campaignId;
                                break;
                            }

                        break;
                    case 10:
                        foreach (var cy in calenderYear)
                            if (cy.Oct == 0)
                            {
                                cy.Oct = campaignId;
                                break;
                            }

                        break;
                    case 11:
                        foreach (var cy in calenderYear)
                            if (cy.Nov == 0)
                            {
                                cy.Nov = campaignId;
                                break;
                            }

                        break;
                    case 12:
                        foreach (var cy in calenderYear)
                            if (cy.Dec == 0)
                            {
                                cy.Dec = campaignId;
                                break;
                            }

                        break;
                }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
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

    public class TradeDetails
    {
        public int CampaignId { get; set; }
        public string TradeName { get; set; }
        public string TradeDescription { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public string Color { get; set; }
    }

    public class ScheduleFilter
    {
        public string TradeName { get; set; }
        public string CampaignType { get; set; }
    }
}