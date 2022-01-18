using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataAccesPatterns
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(DatabaseContext context) : base(context) 
        {

        }

        public string ProductSpecificMethod()
        {
            return "Called from non generic ProductRepository";
        }
    }
}
