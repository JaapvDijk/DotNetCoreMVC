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

    public class NumberCounterScoped : NumberCounter
    {
        public void Display()
        {
            Console.Write($"Total Scoped: {total}");
        }
    }

    public class NumberCounterTransient : NumberCounter
    {
        public void Display()
        {
            Console.Write($"Total Transient: {total}");
        }
    }
}
