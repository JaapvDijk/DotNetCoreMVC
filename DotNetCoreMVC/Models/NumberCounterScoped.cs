using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreMVC.Models
{
    public class NumberCounterScoped : NumberCounter
    {
        public void Display()
        {
            Console.Write($"Total Scoped: {total}");
        }
    }
}
