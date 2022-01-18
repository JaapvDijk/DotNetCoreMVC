using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataAccesPatterns
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(DatabaseContext context) : base(context)
        {

        }

        public string OrderSpecificMethod()
        {
            return "Called from non generic ProductRepository";
        }
    }
}
