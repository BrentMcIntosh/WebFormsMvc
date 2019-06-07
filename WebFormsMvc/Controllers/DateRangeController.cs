
using System;

using System.Web.Mvc;

using Web.Models;

namespace WebFormsMvc.Controllers
{
    public class DateRangeController : Controller
    {
        public ActionResult Index(int id, string range = "YearToDate", string rangeValue = "")
        {
            return View(new DateRange(id, range, rangeValue));
        }
    }
}