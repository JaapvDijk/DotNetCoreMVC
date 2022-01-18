using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataAccesPatterns
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
        IOrderRepository OrderRepository { get; }
    }
}
