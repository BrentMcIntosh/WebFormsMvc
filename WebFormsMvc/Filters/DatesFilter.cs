using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using WebFormsMvc.Models;
using WebFormsMvc.Models.AgGrid;

namespace WebFormsMvc.Filters
{
    public class DatesFilter
    {
        public class DateWhere
        {
            public string Predicate { get; set; }

            public DateTime Begin { get; set; }

            public DateTime? End { get; set; }
        }

        public static DateWhere HandleDatesFirst(Condition condition, string columnName, IQueryable<SampleData> collection)
        {
            DateTime.TryParse(condition.DateFrom, out var begin);
            DateTime.TryParse(condition.DateTo, out var end);

            switch (condition.Type)
            {
                case "greaterThan":
                    return new DateWhere { Predicate = $"{columnName} >= @0", Begin = begin, End = null };

                case "notEqual":
                    return new DateWhere { Predicate = $"{columnName} != @0", Begin = begin, End = null };

                case "equals":
                    return new DateWhere { Predicate = $"{columnName} == @0", Begin = begin, End = null };

                case "lessThan":
                    return new DateWhere { Predicate = $"{columnName} <= @0", Begin = begin, End = null };

                case "inRange":
                    return new DateWhere { Predicate = $"{columnName} >= @0 && {columnName} <= @1", Begin = begin, End = end };

                default:
                    return null;
            }
        }

        public static List<SampleData> HandleDatesSecond(Condition condition, string columnName, IQueryable<SampleData> collection, DateWhere first, string joiner)
        {
            DateTime.TryParse(condition.DateFrom, out var begin);
            DateTime.TryParse(condition.DateTo, out var end);

            int position1 = first.End.HasValue ? 2 : 1;
            int position2 = first.End.HasValue ? 3 : 2;

            var second = new DateWhere();

            switch (condition.Type)
            {
                case "greaterThan":
                    second = new DateWhere { Predicate = $"{columnName} >= @{position1}", Begin = begin, End = null };
                    break;

                case "notEqual":
                    second = new DateWhere { Predicate = $"{columnName} != @{position1}", Begin = begin, End = null };
                    break;

                case "equals":
                    second = new DateWhere { Predicate = $"{columnName} == @{position1}", Begin = begin, End = null };
                    break;

                case "lessThan":
                    second = new DateWhere { Predicate = $"{columnName} <= @{position1}", Begin = begin, End = null };
                    break;

                case "inRange":
                    second = new DateWhere { Predicate = $"{columnName} >= @{position1} && {columnName} <= @{position2}", Begin = begin, End = end };
                    break;
            }

            joiner = joiner == "OR" ? " || " : " && ";

            var combined = $"({first.Predicate}) {joiner} ({second.Predicate})";

            var one = first.Begin;

            var two = first.End ?? second.Begin;

            var three = first.End.HasValue ? second.Begin : second.End;

            var four = first.End.HasValue ? second.End : null;

            if (!three.HasValue) return collection.Where(combined, one, two).ToList();

            return four.HasValue ? collection.Where(combined, one, two, three, four).ToList() : collection.Where(combined, one, two, three).ToList();
        }

        public static List<SampleData> HandleDatesSingle(Condition condition, string columnName, IQueryable<SampleData> collection)
        {
            DateTime.TryParse(condition.DateFrom, out var begin);
            DateTime.TryParse(condition.DateTo, out var end);

            switch (condition.Type)
            {
                case "greaterThan":
                    return collection.Where($"{columnName} >= @0", begin).ToList();

                case "notEqual":
                    return collection.Where($"{columnName} != @0", begin).ToList();

                case "equals":
                    return collection.Where($"{columnName} == @0", begin).ToList();

                case "lessThan":
                    return collection.Where($"{columnName} <= @0", begin).ToList();

                case "inRange":
                    return collection.Where($"{columnName} >= @0 && {columnName} <= @1", begin, end).ToList();

                default:
                    return collection.ToList();
            }
        }

    }
}