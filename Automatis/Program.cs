using Automatis.Data;
using Automatis.Models;
namespace Automatis

{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var db = new AppDbContext())
            {
                // Skapa databasen om den inte finns
                db.Database.EnsureCreated();

                // Lägg till en kund
                var customer = new Customer { Name = "Mesgun" };
                db.Customers.Add(customer);
                db.SaveChanges();

                // Lägg till en bil kopplad till kunden
                var car = new Car
                {
                    Brand = "BMW",
                    Model = "M8",
                    Year = 2024,
                    CustomerId = customer.Id
                };
                db.Cars.Add(car);
                db.SaveChanges();

                // Hämta alla kunder och deras bilar
                var customers = db.Customers.ToList();
                foreach (var c in customers)
                {
                    Console.WriteLine($"Kund: {c.Name}");
                    var cars = db.Cars.Where(car => car.CustomerId == c.Id).ToList();
                    foreach (var carItem in cars)
                    {
                        Console.WriteLine($"  Bil: {carItem.Brand} {carItem.Model} ({carItem.Year})");
                    }
                }
            }
            Console.WriteLine("Klar! Tryck på valfri knapp för att avsluta...");
            Console.ReadKey();

        }
    }
}
