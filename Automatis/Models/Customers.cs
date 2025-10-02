using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automatis.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // En kund kan ha flera bilar
        public List<Car> Cars { get; set; } = new List<Car>();
    }
}
