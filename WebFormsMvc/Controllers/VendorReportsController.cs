
using System;
using System.Diagnostics;
using System.Globalization;
using System.Web.Mvc;
using Util;
using Web.Models;

namespace WebFormsMvc.Controllers
{
    public class VendorReportsController : Controller
    {
        public ActionResult Index(int id, string range = "YearToDate", string rangeValue = "")
        {
            ListAllRanges();

            return View(new DateRange(id, range, rangeValue));
        }


        void ListAllRanges()
        {
            var list = DateRange.GetAllRanges();

            foreach (var item in list)
            {
                var firstDropdown = EnumUtils.GetEnumDescription<RangeChoices>(item.ChosenRange);

                var number = 0;

                var secondDropdown = item.RangeValue;

                if (int.TryParse(item.RangeValue, out number) && number <= 12)
                {
                    secondDropdown = new DateTimeFormatInfo().GetMonthName(number);
                }

                var beginDate = item.Start.ToShortDateString();

                var endDate = item.Finish.AddDays(-1).ToShortDateString();

                // var row = $"<div class=\"row\"><div class=\"cell\" data-title=\"First Dropdown\">{firstDropdown}</div><div class=\"cell\" data-title=\"Second Dropdown\">{secondDropdown}</div>";

                // row += $"<div class=\"cell\" data-title=\"Begin Date\">{beginDate}</div><div class=\"cell\" data-title=\"End Date\">{endDate}</div></div>";

                var row = "<tr>";

                row += $"<td>{firstDropdown}</td>";

                row += $"<td>{secondDropdown}</td>";

                row += $"<td>{beginDate}</td>";

                row += $"<td>{endDate}</td>";

                row += "</tr>";

                Trace.WriteLine(row);



            }
        }
    }
}