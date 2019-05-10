using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NHibernate.Linq;
using WebFormsMvc.Extensions;
using WebFormsMvc.Filters;
using WebFormsMvc.Models;
using WebFormsMvc.Models.AgGrid;

namespace WebFormsMvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(AgGridModel.GetSample());
        }

        public static IQueryable<SampleData> Sort(IQueryable<SampleData> collection, string sortBy, bool reverse = false)
        {
            return collection.OrderBy(sortBy + (reverse ? " descending" : ""));
        }

        public static ExpressionFilter GetFilter(Condition condition, string columnName)
        {
            condition.Type = condition.Type.Replace("equals", "equal");

            if (Enum.Parse(typeof(Comparison), condition.Type.FirstCharToUpper()) is Comparison compare)
            {
                return new ExpressionFilter
                {
                    PropertyName = columnName,
                    Comparison = compare,
                    Value = condition.Filter
                };
            }

            return null;
        }

        public static string HandleNumbers(Condition condition, string columnName)
        {
            decimal.TryParse(condition.Filter, out var begin);

            switch (condition.Type)
            {
                case "greaterThan":
                    return $"({columnName} >= {begin})";

                case "notEqual":
                    return $"({columnName} != {begin})";

                case "equals":
                    return $"({columnName} == {begin})";

                case "lessThan":
                    return $"({columnName} <= {begin})";

                case "inRange":
                    return $"(({columnName} >= {begin}) && ({columnName} <= {condition.FilterTo}))";

                default:
                    return "";
            }
        }

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

        public static List<SampleData> Filter(IQueryable<SampleData> collection, Dictionary<string, ColumnFilter> filters)
        {
            var expressionFilters = new List<ExpressionFilter>();

            foreach (var key in filters.Keys)
            {
                var columnFilter = filters[key];

                var numberQuery = "";

                var keyUpper = key.FirstCharToUpper();

                var first = new DateWhere();

                if (columnFilter.Condition1 != null)
                {
                    switch (columnFilter.Condition1.FilterType)
                    {
                        case "date":
                            first = HandleDatesFirst(columnFilter.Condition1, keyUpper, collection);
                            break;

                        case "number":
                            numberQuery = HandleNumbers(columnFilter.Condition1, keyUpper);
                            break;

                        default:
                            expressionFilters.Add(GetFilter(columnFilter.Condition1, keyUpper));
                            break;
                    }
                }

                if (columnFilter.Condition2 != null)
                {
                    switch (columnFilter.Condition2.FilterType)
                    {
                        case "date":
                            return HandleDatesSecond(columnFilter.Condition2, keyUpper, collection, first, columnFilter.Operator);

                        case "number":

                            var joiner = columnFilter.Operator == "AND" ? "&&" : "||";

                            numberQuery = $"{numberQuery} {joiner} {HandleNumbers(columnFilter.Condition2, keyUpper)}";

                            break;

                        default:
                            expressionFilters.Add(GetFilter(columnFilter.Condition2, keyUpper));
                            break;
                    }
                }

                if (columnFilter.Condition1 == null)
                {
                    switch (columnFilter.FilterType)
                    {
                        case "date":
                            return HandleDatesSingle(columnFilter.Condition1, keyUpper, collection);

                        case "number":
                            numberQuery = HandleNumbers(columnFilter, keyUpper);
                            break;

                        default:
                            expressionFilters.Add(GetFilter(columnFilter, keyUpper));
                            break;
                    }
                }

                if (!string.IsNullOrWhiteSpace(numberQuery))
                {
                    return collection.Where(numberQuery).ToList();
                }

                var expressionTree = ConstructAndExpressionTree.MakeTree<SampleData>(expressionFilters, columnFilter.Operator);

                var anonymousFunc = expressionTree.Compile();

                return new List<SampleData>(collection.AsEnumerable().Where(predicate: anonymousFunc));
            }

            return null;
        }

        public string GetRows(GetRowsRequest request)
        {
            var data = new List<Dictionary<string, object>>();

            var count = 7;

            using (var session = NHibernateSession.OpenSession())
            {
                var items = session.Query<SampleData>().Take(count).ToList();

                if (request?.SortModel != null && request.SortModel.Count > 0)
                {
                    var sort = request.SortModel[0];

                    var columnName = sort.ColId.FirstCharToUpper();

                    var direction = sort.Sort;

                    items = new List<SampleData>(Sort(items.AsQueryable(), columnName, direction == "desc"));
                }

                if (request?.FilterModel != null && request.FilterModel.Count > 0)
                {
                    items = Filter(items.AsQueryable(), request.FilterModel);
                }

                foreach (var item in items)
                {
                    data.Add(new Dictionary<string, object>
                    {
                        ["id"] = item.Id,
                        ["name"] = item.Name,
                        ["born"] = item.Born,
                        ["cost"] = item.Cost,
                        ["good"] = item.Good
                    });
                }
            }

            var response = new GetRowsResponse
            {
                Data = data,

                LastRow = count
            };

            return JsonConvert.SerializeObject(response, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }
    }
}