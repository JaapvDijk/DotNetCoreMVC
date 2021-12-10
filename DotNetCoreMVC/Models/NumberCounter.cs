using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreMVC.Models
{
    public class NumberCounter : INumberCounter
    {
        public int total { get; set; } = 0;
    }
}
