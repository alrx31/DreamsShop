using System.Linq.Expressions;

namespace Domain.Specifications;

public abstract class Specification<T>
{
    public abstract Expression<Func<T, bool>> ToExpression();

    public bool IsSatisfiedBy(T entity)
    {
        var predicate = ToExpression().Compile();
        return predicate(entity);
    }

    public Specification<T> And(Specification<T> other) =>
        new AndSpetification<T>(this, other);

    
    public Specification<T> Or(Specification<T> other) =>
        new OrSpetification<T>(this, other);

    public Specification<T> Not() =>
        new NotSpetification<T>(this);


    public static bool operator true(Specification<T> spec) => false;

    public static bool operator false(Specification<T> spec) => false;

    public static Specification<T> operator &(Specification<T> left, Specification<T> right)
    {
        return left.And(right);
    }

    public static Specification<T> operator |(Specification<T> left, Specification<T> right)
    {
        return left.Or(right);
    }

    public static Specification<T> operator !(Specification<T> spec)
    {
        return spec.Not();
    }
}
