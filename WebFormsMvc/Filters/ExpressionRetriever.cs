using System;
using System.Linq.Expressions;
using System.Reflection;

using static WebFormsMvc.Filters.Comparison;

namespace WebFormsMvc.Filters
{
    public static class ExpressionRetriever
    {
        private static readonly MethodInfo EqualsMethod = typeof(string).GetMethod("Equals", new Type[] { typeof(string), typeof(StringComparison) });

        private static readonly MethodInfo StartsWithMethod = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string), typeof(StringComparison) });

        private static readonly MethodInfo IndexOfMethod = typeof(string).GetMethod("IndexOf", new Type[] { typeof(string), typeof(StringComparison) });

        private static readonly MethodInfo EndsWithMethod = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string), typeof(StringComparison) });

        public static Expression GetExpression<T>(ParameterExpression param, ExpressionFilter filter)
        {
            var member = Expression.Property(param, filter.PropertyName);

            var constant = Expression.Constant(filter.Value);

            var ignore = Expression.Constant(StringComparison.InvariantCultureIgnoreCase);

            switch (filter.Comparison)
            {
                case Equal:
                    return Expression.Call(member, EqualsMethod, constant, ignore);

                case NotEqual:
                    var equals = Expression.Call(member, EqualsMethod, constant, ignore);

                    return Expression.NotEqual(equals, Expression.Constant(true));

                case Contains:
                    var indexOf = Expression.Call(member, IndexOfMethod, constant, ignore);

                    return Expression.NotEqual(indexOf, Expression.Constant(-1));

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