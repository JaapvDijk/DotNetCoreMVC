using System;
using System.Collections.Generic;

namespace DataAccess
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Review Reviews { get; set; }

        public List<Order> Orders { get; set; }
    }
}
