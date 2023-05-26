using SleekFlow.Domain.Attributes;
using SleekFlow.Domain.Entities;
using SleekFlow.Domain.Enums;
using SleekFlow.Domain.Filters;
using System.Linq.Expressions;
using System.Reflection;

namespace SleekFlow.Infrastructure
{
    public static class QueryExtension
    {
        public static IQueryable<T> Filter<T>(this IQueryable<T> source, Expression<Func<T, bool>> filterExpression)
        {
            return filterExpression == null ? source : source.Where(filterExpression);
        }

        public static IQueryable<T> HandleSort<T>(this IQueryable<T> source, string column, string direction)
            where T : BaseEntity
        {
            if (string.IsNullOrEmpty(column))
            {
                return source.OrderByDescending(s =>
                    s.EditAt == null ? s.AddAt : (s.EditAt > s.AddAt ? s.EditAt : s.AddAt));
            }
            else
            {
                return source.Sort(column, direction);
            }
        }

        public static IQueryable<T> Sort<T>(this IQueryable<T> source, string column, string direction)
        {
            if (column != null)
            {
                var isOrdered = source.Expression is MethodCallExpression methodCallExpression
                    && (methodCallExpression.Method.Name == "OrderBy"
                        || methodCallExpression.Method.Name == "OrderByDescending"
                        || methodCallExpression.Method.Name == "ThenBy"
                        || methodCallExpression.Method.Name == "ThenByDescending");

                string command = null;
                if (direction == "asc")
                {
                    command = isOrdered ? "ThenBy" : "OrderBy";
                }
                else if (direction == "desc")
                {
                    command = isOrdered ? "ThenByDescending" : "OrderByDescending";
                }

                if (command != null)
                {
                    var type = typeof(T);
                    var property = type.GetProperty(column, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (property != null)
                    {
                        var parameter = Expression.Parameter(type, "p");
                        var propertyAccess = Expression.Property(parameter, property);
                        var orderByExpression = Expression.Lambda(propertyAccess, parameter);
                        var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExpression));
                        return source.Provider.CreateQuery<T>(resultExpression);
                    }
                }
            }

            return source;
        }

        public static IQueryable<T> Page<T>(this IQueryable<T> source, int pageNumber, int itemsPerPage)
        {
            if (pageNumber == 0)
            {
                return source;
            }

            int itemsToSkip = (pageNumber - 1) * itemsPerPage;
            return source.Skip(itemsToSkip).Take(itemsPerPage);
        }

        public static Expression<Func<T, bool>> ToFilterExpression<T>(this BaseFilter filter)
            where T : BaseEntity
        {
            ParameterExpression entityPrm = Expression.Parameter(typeof(T));
            Expression body = Expression.Constant(true);

            PropertyInfo[] filterProperties =
                filter.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Where(prop => prop.GetCustomAttribute<FilterAttribute>() != null || typeof(T).GetProperty(prop.Name) != null)
                    .ToArray();

            if (filterProperties.Any())
            {
                List<Expression> columnFilterExpressions = new();
                List<Expression> wildSearchOnColumnExpressions = new();
                foreach (var filterProperty in filterProperties)
                {
                    var fieldAttribute = filterProperty.GetCustomAttribute<FieldAttribute>();
                    var fieldName = fieldAttribute != null ? fieldAttribute.ExactName : filterProperty.Name;
                    var entityProperty = Expression.Property(entityPrm, fieldName);
                    var filterAttribute = filterProperty.GetCustomAttribute<FilterAttribute>();
                    var filterType = filterAttribute != null ? filterAttribute.FilterType : FilterType.Equal;

                    if (filterProperty.GetValue(filter) is DateTime filterValue)
                    {
                        var leftExpression = Expression.Property(entityProperty, nameof(DateTime.Date));
                        var binaryExpression = filterType switch
                        {
                            FilterType.RangeStart => Expression.GreaterThanOrEqual(leftExpression, Expression.Constant(filterValue.Date)),
                            FilterType.RangeEnd => Expression.LessThan(leftExpression, Expression.Constant(filterValue.Date.AddDays(1))),
                            _ => throw new ArgumentException(nameof(filterAttribute.FilterType)),
                        };

                        columnFilterExpressions.Add(binaryExpression);
                    }
                    else if (filterProperty.GetValue(filter) is Enum enumFilterValue)
                    {
                        var binaryExpression = filterType switch
                        {
                            FilterType.Equal => Expression.Equal(entityProperty, Expression.Constant(enumFilterValue)),
                            _ => throw new ArgumentException(nameof(filterAttribute.FilterType)),
                        };

                        columnFilterExpressions.Add(binaryExpression);
                    }
                }

                if (columnFilterExpressions.Any())
                {
                    Expression columnFilters = columnFilterExpressions
                    .Aggregate(
                        (prev, current) => Expression.AndAlso(prev, current));
                    body = Expression.AndAlso(body, columnFilters);
                }
            }

            return Expression.Lambda<Func<T, bool>>(body, entityPrm);
        }
    }
}
