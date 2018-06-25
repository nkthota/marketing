using Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dashboard.Controllers
{
    [Authorize(Roles = "Marketing_Admin")]
    public class DashboardController : Controller
    {
        private MarketingEntities db = new MarketingEntities();
        // GET: Dashboard
        public ActionResult Index()
        {
            return View(db.Campaigns.ToList());
        }
    }
}