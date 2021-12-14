using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreMVC.Models
{
    public class NumberCounterDependent
    {
        public NumberCounterTransient NumberCounterTransient { get; }
        public NumberCounterScoped NumberCounterScoped { get; }
        public NumberCounterSingleton NumberCounterSingleton { get; }
        public IServiceProvider Provider { get; }

        public NumberCounterDependent(NumberCounterTransient numberCounterTransient,
                      NumberCounterScoped numberCounterScoped,
                      NumberCounterSingleton numberCounterSingleton,
                      IServiceProvider provider)
        {
            NumberCounterTransient = numberCounterTransient;
            NumberCounterScoped = numberCounterScoped;
            NumberCounterSingleton = numberCounterSingleton;
            Provider = provider;
        }

        //public void go() {
        //    for (int i = 0; i < 10; i++) 
        //    {
        //        if (i % 3 == 0)
        //            Provider.GetRequiredService<NumberCounterScoped>();
        //    }

        //}
    }
}
