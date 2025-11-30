using System.Linq.Expressions;

namespace Domain.Specifications;

public abstract class Spetification<T>
{
    public abstract Expression<Func<T, bool>> ToExpression();

    public bool IsSatisfiedBy(T entity)
    {
        var predicate = ToExpression().Compile();
        return predicate(entity);
    }

    public Spetification<T> And(Spetification<T> other) =>
        new AndSpetification<T>(this, other);

    
    public Spetification<T> Or(Spetification<T> other) =>
        new OrSpetification<T>(this, other);

    public Spetification<T> Not() =>
        new NotSpetification<T>(this);


    public static bool operator true(Spetification<T> spec) => true;

    public static bool operator false(Spetification<T> spec) => false;

    public static Spetification<T> operator &(Spetification<T> left, Spetification<T> right)
    {
        return left.And(right);
    }

    public static Spetification<T> operator |(Spetification<T> left, Spetification<T> right)
    {
        return left.Or(right);
    }

    public static Spetification<T> operator !(Spetification<T> spec)
    {
        return spec.Not();
    }
}
