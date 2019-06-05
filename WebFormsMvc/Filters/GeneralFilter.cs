using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using WebFormsMvc.Extensions;
using WebFormsMvc.Models;
using WebFormsMvc.Models.AgGrid;

namespace WebFormsMvc.Filters
{
    public class GeneralFilter
    {
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

        public static List<SampleData> Filter(IQueryable<SampleData> collection, Dictionary<string, ColumnFilter> filters)
        {
            var expressionFilters = new List<ExpressionFilter>();

            foreach (var key in filters.Keys)
            {
                var columnFilter = filters[key];

                var numberQuery = "";

                var keyUpper = key.FirstCharToUpper();

                var first = new DatesFilter.DateWhere();

                if (columnFilter.Condition1 != null)
                {
                    switch (columnFilter.Condition1.FilterType)
                    {
                        case "date":
                            first = DatesFilter.HandleDatesFirst(columnFilter.Condition1, keyUpper, collection);
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
                            return DatesFilter.HandleDatesSecond(columnFilter.Condition2, keyUpper, collection, first, columnFilter.Operator);

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
                            return DatesFilter.HandleDatesSingle(columnFilter, keyUpper, collection);

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

    }
}