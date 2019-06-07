using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Web.Mvc;

namespace Web.Models
{
    public enum RangeChoices
    {
        [Description("Year To Date")]
        YearToDate,
        [Description("Previous Twelve Months")]
        PreviousTwelveMonths,
        Months,
        Quarters,
        [Description("Calendar Years")]
        CalendarYears
    }

    /// <remarks>
    /// I am using the start month to get the time span 
    /// </remarks>
    public enum QuarterChoices
    {
        [Description("Please Choose a Quarter")]
        PleaseChoose = 0,
        First = 1,
        Second = 4,
        Third = 7,
        Fourth = 10
    }

    public class DateRange
    {
        public int Id { get; set; }

        public RangeChoices ChosenRange { get; set; }

        public QuarterChoices ChosenQuarter { get; set; }

        public IEnumerable<SelectListItem> Years { get; set; }

        public IEnumerable<SelectListItem> Months { get; set; }

        public string RangeValue { get; set; }

        public DateTime Start { get; set; }

        public DateTime Finish { get; set; }

        public string Information { get; set; }

        private static readonly DateTime NOW = DateTime.UtcNow;
        private static readonly int CURRENT_YEAR = NOW.Year;

        public DateRange(int id, string range, string rangeValue)
        {
            Id = id;

            RangeValue = rangeValue;

            var rangeValueAsNumber = 0;

            int.TryParse(rangeValue, out rangeValueAsNumber);

            ChosenQuarter = QuarterChoices.PleaseChoose;

            ChosenRange = (RangeChoices)Enum.Parse(typeof(RangeChoices), range);

            GetRange(rangeValue, rangeValueAsNumber);

            Months = GetMonths(rangeValueAsNumber);

            Years = GetYears(rangeValueAsNumber);

            Information = $"You will see information for things that occurred on or after {Start.ToLongDateString()} and before {Finish.ToLongDateString()}";
        }

        public void GetRange(string rangeValue, int rangeValueAsNumber)
        {
            switch (ChosenRange)
            {
                case RangeChoices.PreviousTwelveMonths:
                    Start = NOW.AddYears(-1);
                    Finish = NOW;
                    break;

                case RangeChoices.Months:
                    Start = new DateTime(CURRENT_YEAR, rangeValueAsNumber, 1);
                    Finish = Start.AddMonths(1);
                    break;

                case RangeChoices.Quarters:
                    Start = GetQuarterStart(rangeValue);
                    Finish = Start.AddMonths(3);
                    break;

                case RangeChoices.CalendarYears:
                    Start = new DateTime(rangeValueAsNumber, 1, 1);
                    Finish = Start.AddYears(1);
                    break;

                default:
                    Start = new DateTime(CURRENT_YEAR, 1, 1);
                    Finish = NOW.AddDays(1);
                    break;
            }
        }

        private DateTime GetQuarterStart(string rangeValue)
        {
            ChosenQuarter = (QuarterChoices)Enum.Parse(typeof(QuarterChoices), rangeValue);

            return new DateTime(CURRENT_YEAR, (int)ChosenQuarter, 1);
        }

        private IEnumerable<SelectListItem> GetYears(int chosen)
        {
            var years = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Please Choose a Year"
                }
            };

            for (var year = CURRENT_YEAR; year > CURRENT_YEAR - 4; year--)
            {
                years.Add(new SelectListItem
                {
                    Selected = chosen == year,
                    Text = $"{year}"
                });
            }

            return years;
        }

        private IEnumerable<SelectListItem> GetMonths(int chosen)
        {
            var months = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Please Choose a Month"
                }
            };

            var count = 1;

            foreach (var month in DateTimeFormatInfo.CurrentInfo.MonthNames)
            {
                if (string.IsNullOrWhiteSpace(month)) continue;

                months.Add(new SelectListItem
                {
                    Selected = chosen == count,
                    Text = month,
                    Value = $"{count}"
                });

                count++;
            }

            return months;
        }

        public string GetStyle(RangeChoices dropdown)
        {
            return ChosenRange == dropdown ? "display: inline" : "display: none";
        }

        public static List<DateRange> GetAllRanges()
        {
            var list = new List<DateRange>();

            foreach (var range in Enum.GetNames(typeof(RangeChoices)))
            {
                if (range == "YearToDate" || range == "PreviousTwelveMonths")
                {
                    list.Add(new DateRange(0, range, ""));
                }
                else if (range == "Months")
                {
                    for (var month = 1; month <= 12; month++)
                    {
                        list.Add(new DateRange(0, range, $"{month}"));
                    }
                }
                else if (range == "Quarters")
                {
                    foreach (var quarter in Enum.GetNames(typeof(QuarterChoices)))
                    {
                        if (quarter.StartsWith("Please")) continue;

                        list.Add(new DateRange(0, range, quarter));
                    }
                }
                else if (range == "CalendarYears")
                {
                    for (var year = CURRENT_YEAR; year > CURRENT_YEAR - 4; year--)
                    {
                        list.Add(new DateRange(0, range, $"{year}"));
                    }
                }
            }

            return list;
        }
    }
}