using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automatis.Models
{


    public class Car
    {
        public int Id { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }

        // Relation till Customer
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
    }

}
