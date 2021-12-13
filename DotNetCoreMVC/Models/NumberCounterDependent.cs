using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreMVC.Models
{
    public class NumberCounterDependent
    {
        public NumberCounterTransient NumberCounterTransient { get; set; }
        public NumberCounterScoped NumberCounterScoped { get; set; }
        public NumberCounterSingleton NumberCounterSingleton { get; set; }
        public NumberCounterDependent(NumberCounterTransient numberCounterTransient,
                      NumberCounterScoped numberCounterScoped,
                      NumberCounterSingleton numberCounterSingleton
                      )
        {
            NumberCounterTransient = numberCounterTransient;
            NumberCounterScoped = numberCounterScoped;
            NumberCounterSingleton = numberCounterSingleton;

        }
    }
}
