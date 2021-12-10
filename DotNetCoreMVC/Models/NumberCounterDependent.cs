using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreMVC.Models
{
    public class NumberCounterDependent
    {
        NumberCounterTransient _numberCounterTransient;
        NumberCounterScoped _numberCounterScoped;
        NumberCounterSingleton _numberCounterSingleton;
        public NumberCounterDependent(NumberCounterTransient numberCounterTransient,
                      NumberCounterScoped numberCounterScoped,
                      NumberCounterSingleton numberCounterSingleton)
        {
            _numberCounterTransient = numberCounterTransient;
            _numberCounterScoped = numberCounterScoped;
            _numberCounterSingleton = numberCounterSingleton;
        }
    }
}
