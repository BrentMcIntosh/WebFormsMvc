
using System;

using System.Globalization;

namespace WebFormsMvc.Models
{
    public class DateRange
    {
        public int Id { get; set; }

        public string Range { get; set; }

        public string RangeValue { get; set; }

        public DateTime Start { get; set; }

        public DateTime Finish { get; set; }

        public string SelectedRange(string option)
        {
            return (Range == option) ? "selected='selected'" : "";
        }

        public string SelectedValue(string option)
        {
            return (RangeValue == option) ? "selected='selected'" : "";
        }

        public string Visibility(string option)
        {
            var display = (Range == option) ? "inline" : "none";

            return $"style='display:{display};'";
        }

        private static readonly DateTime NOW = DateTime.UtcNow;
        private static readonly DateTime TOMORROW = NOW.AddDays(1);
        private static readonly int CURRENT_YEAR = NOW.Year;
        private static readonly DateTime JANUARY_FIRST = new DateTime(CURRENT_YEAR, 1, 1);
        private static readonly DateTime APRIL_FIRST = new DateTime(CURRENT_YEAR, 4, 1);
        private static readonly DateTime JULY_FIRST = new DateTime(CURRENT_YEAR, 7, 1);
        private static readonly DateTime OCTOBER_FIRST = new DateTime(CURRENT_YEAR, 10, 1);
        private static readonly DateTime NEXT_JANUARY_FIRST = new DateTime(CURRENT_YEAR + 1, 1, 1);

        public DateRange(int id, string range, string rangeValue)
        {
            Id = id;
            Range = range;
            RangeValue = rangeValue;

            Start = JANUARY_FIRST;
            Finish = TOMORROW;

            if (range == "PreviousTwelveMonths")
            {
                Start = NOW.AddYears(-1);
                Finish = NOW;
            }
            else if (range == "Months")
            {
                var month = DateTime.ParseExact(rangeValue, "MMMM", CultureInfo.CurrentCulture).Month;

                var thisMonth = month;
                var nextMonth = (month + 1);

                var endYear = CURRENT_YEAR;

                if (month == 12)
                {
                    nextMonth = 1;

                    endYear = CURRENT_YEAR + 1;
                }

                Start = new DateTime(CURRENT_YEAR, thisMonth, 1);
                Finish = new DateTime(endYear, nextMonth, 1);
            }
            else if (range == "Quarters")
            {
                switch (rangeValue)
                {
                    case "First":
                        Start = JANUARY_FIRST;
                        Finish = APRIL_FIRST;
                        break;

                    case "Second":
                        Start = APRIL_FIRST;
                        Finish = JULY_FIRST;
                        break;

                    case "Third":
                        Start = JULY_FIRST;
                        Finish = OCTOBER_FIRST;
                        break;

                    case "Fourth":
                        Start = OCTOBER_FIRST;
                        Finish = NEXT_JANUARY_FIRST;
                        break;
                }
            }
            else if (range == "CalendarYears")
            {
                var year = Convert.ToInt32(rangeValue);

                Start = new DateTime(year, 1, 1);
                Finish = new DateTime(year + 1, 1, 1);
            }
        }
    }
}