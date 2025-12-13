using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Domain.Specifications;

public class ValueSpecification<T, K>(Expression<Func<T, K>> keySelector, K[] values) : Specification<T>
{
    private static MethodInfo ContainsMethodInfo =>
        typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public)
            .Single(m => m.Name == nameof(Enumerable.Contains) && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(K));

    public override Expression<Func<T, bool>> ToExpression()
    {
        if (values is null || values.Length == 0)
        {
            return _ => false;
        }

        var memberExpression = keySelector.Body is UnaryExpression unary && unary.Operand is MemberExpression member
            ? member
            : keySelector.Body as MemberExpression
                ?? throw new InvalidOperationException("Key selector must point to a member.");

        var call = Expression.Call(
            ContainsMethodInfo,
            Expression.Constant(values),
            memberExpression);

        return Expression.Lambda<Func<T, bool>>(call, keySelector.Parameters);
    }
}
