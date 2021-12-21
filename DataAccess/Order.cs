using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class Order
    {
        public int Id { get; set; }
        public List<Product> Products {get; set; }
    }
}
