using System.Reflection;
using System.Threading.Channels;
using Automatis.Data;
using Automatis.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;
namespace Automatis

{
    internal class Program
    {
        // det  här programet är ett kund och biilregister
        // jag skriver om koden och testar den
        static void Main(string[] args)
        {
            // skapa databasen 
            using (var db = new AppDbContext())
            {

                db.Database.EnsureCreated();

                // lägger in startdata om databasen är tom

                InitDemoData(db);

                Console.WriteLine("Testar");
                Console.WriteLine("testar funktioner steg för steg");
                Console.WriteLine("tryck på valfri för att öppna menyn");
                Console.ReadKey();

                bool kör = true;
                while (kör)
                {
                    Console.Clear();
                    Console.WriteLine("----- Automatis system----");
                    Console.WriteLine("1. Visa alla kunder och bilar");
                    Console.WriteLine("2. Lägg till ny  kund med bil");
                    Console.WriteLine("3. Lägg till bil till befintligt kund");
                    Console.WriteLine("4. Ta bort kund eller bil");
                    Console.WriteLine("5. Avsluta");
                    Console.WriteLine("Välj 1-5");
                    var val = Console.ReadLine();

                    Console.Clear();
                    switch (val)
                    {
                        case "1":
                            VisaKunder(db);
                            Paus();
                            break;

                        case "2":
                            LäggTillKund(db);
                            Paus();
                            break;
                        case "3":
                            LäggTillBill(db);
                            Paus();
                            break;
                        case "4":
                            TaBort();
                            Pause();
                            break;
                        case "5":
                            kör = false;
                            Console.WriteLine("programet avslutas");
                            break;
                        default:
                            Console.WriteLine("Fel val, försök igen");
                            Paus();
                            break;
                    }
                }




            }


        }

        //lägger in några kunder med bilar första gången programmet körs

        static void InitDemoData(AppDbContext db)
        {
            if (db.Customers.Any())
                return; //redan data gör inget

            Console.WriteLine("Skapar Demodata");

            var kunder = new List<Customer>
            {
                new Customer
                {
                    Name = "Mesgun",
                    Cars = new List<Car>
                    {
                        new Car { Brand = "BMW", Model = "M8", Year = 2024},
                        new Car { Brand = "Volvo", Model = "XC90", Year = 2022}



                    }


                },
                new Customer
                {
                    Name = "Sara",
                    Cars = new List<Car>
                    {
                        new Car {Brand = "Tesla", Model = "Model 3", Year = 2023}
                    }
                },

                new Customer
                {
                    Name = "Adam",
                    Cars = new List<Car>
                    {
                        new Car {Brand = "Audi", Model = "A6", Year = 2021}
                    }
                }
            };

            db.Customers.AddRange(kunder);
            db.SaveChanges();

        }

        // Test: visar alla kunder med deras bilar
        static void VisaKunder(AppDbContext db)
        {
            Console.WriteLine("visa alla kunder och bilar");
            
            var Kunder = db.Customers.ToList();
            Console.WriteLine($"Antal kunder i databasen: {Kunder.Count}");

            foreach (var K in Kunder)
            {
                Console.WriteLine($"Kund: {K.Name}");
                var bilar = db.Cars.Where(c => c.CustomerId == K.Id).ToList();
                
                if (bilar.Count == 0)
                {
                    Console.WriteLine("(Ingen bil registrerad)");
                }
                else
                {
                    foreach(var bil in bilar)
                        Console.WriteLine($" Bil: {bil.Brand} ({bil.Year})");
                }
            }
        }

        // lägg till en ny kund med en bil
        static void LäggTillKund(AppDbContext db)
        {
            Console.WriteLine("Test: lägg till ny kund med bil");

        }

        // Lägg till en till bil
        static void LäggTillBil(AppDbContext db)
        {
            Console.WriteLine("Test: Lägg till bil till kund");

            var kund =  Väljkund(db);
            if (kund == null)
                return;
        }


        static void Paus()
        {
            Console.WriteLine("\nTryck på valfri tanget för att fortsätta.");
            Console.ReadKey();
        }
    }
}
