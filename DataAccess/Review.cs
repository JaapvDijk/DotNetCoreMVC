using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class Review
    {
        public int Id { get; set; }
        [Range(1,5)]
        public int Rating {get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
    }
}
