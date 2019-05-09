using System;
using System.Collections.Generic;
using System.IO;
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

        public static List<SampleData> HandleDates(Condition condition, string columnName, IQueryable<SampleData> collection)
        {
            DateTime.TryParse(condition.DateFrom, out var begin);
            DateTime.TryParse(condition.DateTo, out var end);

            switch (condition.Type)
            {
                case "greaterThan":
                    return collection.Where(columnName + ">= @0", begin).ToList();

                case "notEqual":
                    return collection.Where(columnName + "!= @0", begin).ToList();

                case "equals":
                    return collection.Where(columnName + "== @0", begin).ToList();

                case "lessThan":
                    return collection.Where(columnName + "<= @0", begin).ToList();

                case "inRange":
                    return collection.Where(columnName + ">= @0 && " + columnName + " <= @1", begin, end).ToList();

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

                if (columnFilter.Condition1 != null)
                {
                    switch (columnFilter.Condition1.FilterType)
                    {
                        case "date":
                            return HandleDates(columnFilter.Condition1, keyUpper, collection);

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
                            return HandleDates(columnFilter.Condition2, keyUpper, collection);

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
                            return HandleDates(columnFilter, keyUpper, collection);

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