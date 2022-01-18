using DataAccess.DataAccesPatterns;
using System;
using System.Collections.Generic;

namespace DataAccess
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public decimal Price { get; set; }

        public IValueHolder<byte[]> ProductPictureHolder { get; set; }
        public byte[] Picture 
        { 
            get 
            {
                return ProductPictureHolder.GetValue(Name);       
            }
            set 
            { 
                Picture = value; 
            }
        }

        public IValueHolder<byte[]> ProductPictureHolder2 { get; set; }
        public virtual byte[] Picture2 { get; set; }
        public List<Review> Reviews { get; set; }
        public List<Order> Orders { get; set; }
    }
}
