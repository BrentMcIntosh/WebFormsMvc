using System;
using System.Linq.Expressions;
using System.Reflection;
using WebFormsMvc.Extensions;

using static WebFormsMvc.Filters.Comparison;

namespace WebFormsMvc.Filters
{
    public static class ExpressionRetriever
    {
        private static readonly MethodInfo ContainsMethod = typeof(Str).GetMethod("ContainsExt", new Type[] { typeof(string), typeof(StringComparison) });

        private static readonly MethodInfo StartsWithMethod = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string), typeof(StringComparison) });

        private static readonly MethodInfo EndsWithMethod = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string), typeof(StringComparison) });

        public static Expression GetExpression<T>(ParameterExpression param, ExpressionFilter filter)
        {
            var member = Expression.Property(param, filter.PropertyName);

            var constant = Expression.Constant(filter.Value);

            var ignore = Expression.Constant(StringComparison.InvariantCultureIgnoreCase);

            switch (filter.Comparison)
            {
                case Equal:
                    return Expression.Equal(member, constant);

                case GreaterThan:
                    return Expression.GreaterThan(member, constant);

                case GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(member, constant);

                case LessThan:
                    return Expression.LessThan(member, constant);

                case LessThanOrEqual:
                    return Expression.LessThanOrEqual(member, constant);

                case NotEqual:
                    return Expression.NotEqual(member, constant);

                case Contains:
                    return Expression.Call(member, ContainsMethod, constant, ignore);

                case StartsWith:
                    return Expression.Call(member, StartsWithMethod, constant, ignore);

                case EndsWith:
                    return Expression.Call(member, EndsWithMethod, constant, ignore);

                default:
                    return null;
            }
        }
    }
}