using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreMVC.Models
{
    public class NumberCounterTransient : NumberCounter
    {
        public void Display()
        {
            Console.Write($"Total Transient: {total}");
        }
    }
}
