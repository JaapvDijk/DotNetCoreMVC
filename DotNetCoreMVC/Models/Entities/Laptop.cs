using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreMVC.Models
{
    public interface IDevice
    {
        string Name { get; set; }
    }

    public interface ILaptop : IDevice
    {
        string? CPU { get; set; }
        int? Storage { get; set; }
    }

    public class Laptop : ILaptop
    {
        public string Name { get; set; }
        public string? CPU { get; set; }
        public int? Storage { get; set; }
    }

    public class TestViewModel
    {
        public IEnumerable<Laptop> LaptopList { get; set; }
        [BindProperty(SupportsGet = true)]
        public string searchString { get; set; }
        #nullable enable
        public string? AuthorizedMessageFromApi { get; set; }
        public int? ProductTotal { get; set; }
        #nullable disable
        public TestViewModel GetByName(string name)
        {
            LaptopList = LaptopList.Where(x => x.Name.ToLower().Contains(name.ToLower()));
            return this;
        }
    }
}
