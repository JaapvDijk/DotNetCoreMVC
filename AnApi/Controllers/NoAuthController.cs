using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AnApi.Controllers
{
    /// <summary>
    /// NoAuth controller summary
    /// </summary>
    /// <returns></returns>
    [Route("api/[controller]")]
    [ApiController]
    public class NoAuthController : ControllerBase
    {
        private readonly Order _order = new()
        {
            Products = new()
            {
                new()
                {
                    Name = "ProductNaam",
                    Price = default,
                    Reviews = new()
                    {
                        new() { Rating = 5, Description = "Very nice" },
                        new() { Rating = 4, Description = "Quite nice" },
                        new() { Rating = 3, Description = "Ok I guess" },
                        new() { Rating = 2, Description = "Not so nice" },
                        new() { Rating = 1, Description = "This sucks" },
                    }
                }
            }
        };

        private readonly DatabaseContext _context;

        public NoAuthController(DatabaseContext databaseContext) 
        {
            _context = databaseContext;
        }

        [HttpGet("Get")]
        public string Get()
        {
            return "Everyone can read this";
        }

        [HttpGet("AddManyProduct")]
        public void AddManyProduct(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                _context.Products.Add(new Product() 
                { 
                    Name = $"Product{i}",
                    Price = i % 200
                });
            }
            _context.SaveChanges();
        }

        [HttpGet("DeleteAllProduct")]
        public void DeleteAllProduct()
        {
            var products = _context.Products.ToList();

            if (products != null) _context.RemoveRange(products);
            _context.SaveChanges();
        }

        [HttpGet("AddOrder")]
        public void AddOrder()
        {
            _context.Orders.Add(_order);
            _context.SaveChanges();
        }

        [HttpGet("AddKeyboard")]
        public void AddKeyboard()
        {
            try
            {
                _context.Keyboards.Add(new Keyboard() { NumberOfButtons = 104 });
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                //var path = AppDomain.CurrentDomain.BaseDirectory + "/here.txt";
                using StreamWriter file = new("here.txt");
                file.WriteLineAsync(e.Message);

            }

        }

        [HttpGet("GetAllKeyboard")]
        public List<Keyboard> GetAllKeyboard()
        {
            return _context.Keyboards.ToList();
        }
    }
}
