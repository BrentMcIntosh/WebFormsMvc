using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebFormsMvc.Filters
{
    public class ConstructAndExpressionTree
    {
        public static Expression<Func<T, bool>> MakeTree<T>(List<ExpressionFilter> filters, string joiner)
        {
            if (filters.Count == 0)
            {
                return null;
            }

            var param = Expression.Parameter(typeof(T), "t") as ParameterExpression;

            Expression exp = null;

            if (filters.Count == 1)
            {
                exp = ExpressionRetriever.GetExpression<T>(param, filters[0]);
            }
            else
            {
                exp = ExpressionRetriever.GetExpression<T>(param, filters[0]);

                for (var i = 1; i < filters.Count; i++)
                {
                    exp = joiner == "AND" ? 
                        Expression.And(exp, ExpressionRetriever.GetExpression<T>(param, filters[i])) : 
                        Expression.Or(exp, ExpressionRetriever.GetExpression<T>(param, filters[i]));
                }
            }

            return Expression.Lambda<Func<T, bool>>(exp, param);
        }
    }
}