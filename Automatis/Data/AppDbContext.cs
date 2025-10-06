using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automatis.Models;
using Microsoft.EntityFrameworkCore;

namespace Automatis.Data
{


    public class AppDbContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Databasanslutning (LocalDB)
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;Database=Automatis;Trusted_Connection=True;");
        }
    }

}