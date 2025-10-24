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
                            LäggTillBil(db);
                            Paus();
                            break;
                        case "4":
                            TaBort(db);
                            Paus();
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

            var Kund =  VäljKund(db);
            if (Kund == null)
                return;

            int föreBilar = db.Cars.Count(c=> c.CustomerId == Kund.Id);
            Console.WriteLine($"Bilar för {Kund.Name} före: {föreBilar}");

            string märke = LäsText("Angel bilmärke: ");
            string modell = LäsText("Ange bilmodell");
            int år = LäsInt("Ange årsmodell ): ", 1900, DateTime.Now.Year + 1 );

            var nyBil = new Car { Brand = märke, Model = modell, Year = år, CustomerId = Kund.Id };
            db.Cars.Add(nyBil);
            db.SaveChanges();

            int efeterBilar = db.Cars.Count(c => c.CustomerId == Kund.Id);
            Console.WriteLine($"klar! Bilar för {Kund.Name} efter: {efeterBilar}");
        }

        // ta bort kund eller bil
        static void TaBort(AppDbContext db)
        {
            Console.WriteLine("Vill du ta bort (1) kund eller (2) bil");
            var val = Console.ReadLine();

            if (val == "1")
            {
                var kund = VäljKund(db);
                if (kund == null) return;

                int föreKunder = db.Customers.Count();
                db.Customers.Remove(kund);
                db.SaveChanges();
                int efterKunder = db.Customers.Count();

                Console.WriteLine($"Tog bort kund {kund.Name} före {föreKunder}, Efter: {efterKunder}");

            }
            else if (val == "2")
            {
                var bilar = db.Cars.ToList();
                if(bilar.Count == 0)
                {
                    Console.WriteLine("Det finns inga bilar att ta bort.");
                    return;
                }
                Console.WriteLine("Välj bil att ta bort");
                for (int i = 0; i < bilar.Count; i++)
                {
                    var owner = db.Customers.FirstOrDefault(c => c.Id == bilar[i].CustomerId);
                    Console.WriteLine($"{i + 1}. {bilar[i].Brand} {bilar[i].Model} ({bilar[i].Year}) - Ägare: {owner?.Name}");

                }
                int index = LäsInt("Ange bilnummer: ", 1, bilar.Count) - 1;
                var bil = bilar [index];

                int föreBilar = db.Cars.Count();
                db.Cars.Remove(bil);
                db.SaveChanges();
                int efterBilar = db.Cars.Count();

                Console.WriteLine($"Tog bort bil {bil.Brand} {bil.Model}. Före: {föreBilar}, Efter: {efterBilar}");

            }
            else
            {
                Console.WriteLine("Ogiltigt val");
            }
        }

        // läser textinmattning och ser till att det inte är tomt
        static string LäsText(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? "";
                if(!string.IsNullOrWhiteSpace(input))
                    return input.Trim();

                Console.WriteLine("Texten får inte vara tom, försök igen");
            }
        }
        // läser ett heltal och kontrollerar att det är giltigt
        static int LäsInt(string prompt, int min, int max)
        {
            while(true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if(int.TryParse(input, out int tal))
                {
                    if (tal >= min && tal <= max)
                        return tal;
                }

                Console.WriteLine($"Skriv ett tal mellan {min} och {max}.");

            }
        }

        // Väljer kund från listan
        static Customer? VäljKund(AppDbContext db)
        {
            var kunder = db.Customers.ToList(); 
            if (kunder.Count == 0)
            {
                Console.WriteLine("Det finns inga kunder");
                return null;
            }
            Console.WriteLine("Välj kund:");
            for(int i = 0; i < kunder.Count; i++)
                Console.WriteLine($"{i + 1}. {kunder[i].Name}");

            int index = LäsInt("Ange kundnummer:", 1, kunder.Count) - 1;
            return kunder[index];
        }

        static void Paus()
        {
            // jag la till denna metod själv för att kunna pausa programmet 
            // för att hinna se resultaten inna skärmen rensas
            Console.WriteLine("\nTryck på valfri tanget för att fortsätta.");
            Console.ReadKey();
        }
    }
}

