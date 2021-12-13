using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreMVC.Models
{
    public class NumberCounterSingleton : NumberCounter
    {
        public void Display()
        {
            Console.Write($"Total Singleton: {total}");
        }
    }
}
