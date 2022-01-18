using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataAccesPatterns
{
    public class UnitOfWork : IUnitOfWork
    {
        private DatabaseContext _context;

        public UnitOfWork(DatabaseContext context) 
        {
            _context = context;
        }

        private IProductRepository productRepository;
        public IProductRepository ProductRepository 
        {
            get 
            {
                if (productRepository == null)
                    productRepository = new ProductRepository(_context);

                return productRepository;
            }
        }

        private IOrderRepository orderRepository;
        public IOrderRepository OrderRepository
        {
            get
            {
                if (orderRepository == null)
                    orderRepository = new OrderRepository(_context);

                return orderRepository;
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
