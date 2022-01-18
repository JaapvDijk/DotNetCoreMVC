using System.Collections.Generic;

namespace DataAccess.DataAccesPatterns
{
    public interface ILazyProductRepository : IRepository<Product>
    {
        IEnumerable<Product> All();
    }
}