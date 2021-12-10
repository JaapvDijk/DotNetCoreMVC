using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreMVC.Models
{
    public class CountersViewModel
    {
        public int Transient { get; set; }
        public int Scoped { get; set; }
        public int  Singleton { get; set; }
    }
}
