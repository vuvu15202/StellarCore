using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Stellar.Shared.Services
{
    public static class DynamicFilterBuilder
    {
        public static IQueryable<T> ApplyFilter<T>(
            IQueryable<T> query,
            Dictionary<string, object> filter)
        {
            var param = Expression.Parameter(typeof(T), "x");
            Expression? body = null;

            foreach (var kv in filter)
            {
                var prop = Expression.Property(param, kv.Key);
                var value = Expression.Constant(Convert.ChangeType(kv.Value, prop.Type));
                var equal = Expression.Equal(prop, value);

                body = body == null ? equal : Expression.AndAlso(body, equal);
            }

            if (body == null) return query;

            var lambda = Expression.Lambda<Func<T, bool>>(body, param);
            return query.Where(lambda);
        }
    }

}
