using System.Linq.Expressions;
using Store.Domain.Entities;

namespace Store.Domain.Queries;

public static class ProductQueries
{
    public static Expression<Func<Product, bool>> GetActiveProduct()
    {
        return x => x.Ative;
    }

    public static Expression<Func<Product, bool>> GetInactiveProduct()
    {
        return x => x.Ative == false;
    }
}