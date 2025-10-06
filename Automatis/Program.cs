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
                db.Database.EnsureCreated();

                if (!db.Customers.Any())
                {
                    var customers = new List<Customer>
                    {
                        new Customer
                        {
                            Name = "Mesgun",
                            Cars = new List<Car>
                            {
                                new Car { Brand = "BMW", Model = "M8", Year = 2024 },
                                new Car { Brand = "Volvo", Model = "XC90", Year = 2022 }
                            }
                        },
                        new Customer
                        {
                            Name = "Sara",
                            Cars = new List<Car>
                            {
                                new Car { Brand = "Tesla", Model = "Model 3", Year = 2023 }
                            }
                        },
                        new Customer
                        {
                            Name = "Adam",
                            Cars = new List<Car>
                            {
                                new Car { Brand = "Audi", Model = "A6", Year = 2021 }
                            }
                        }
                    };

                    db.Customers.AddRange(customers);
                    db.SaveChanges();
                }

                bool running = true;
                while (running)
                {
                    Console.Clear();
                    Console.WriteLine("🚗 AUTOMATIS SYSTEM 🚗");
                    Console.WriteLine("---------------------------");
                    Console.WriteLine("1. Visa alla kunder och bilar");
                    Console.WriteLine("2. Lägg till ny kund");
                    Console.WriteLine("3. Lägg till bil till befintlig kund");
                    Console.WriteLine("4. Ta bort kund eller bil");
                    Console.WriteLine("5. Avsluta");
                    Console.WriteLine("---------------------------");
                    Console.Write("Välj ett alternativ (1-5): ");
                    string choice = Console.ReadLine();

                    Console.Clear();

                    switch (choice)
                    {
                        case "1":
                            VisaAllaKunder(db);
                            break;
                        case "2":
                            LäggTillNyKund(db);
                            break;
                        case "3":
                            LäggTillBilTillKund(db);
                            break;
                        case "4":
                            TaBortKundEllerBil(db);
                            break;
                        case "5":
                            running = false;
                            Console.WriteLine("Programmet avslutas...");
                            break;
                        default:
                            Console.WriteLine("Ogiltigt val, försök igen!");
                            break;
                    }

                    if (running)
                    {
                        Console.WriteLine("\nTryck på valfri knapp för att fortsätta...");
                        Console.ReadKey();
                    }
                }
            }
        }

        // ----- METODER -----

        static void VisaAllaKunder(AppDbContext db)
        {
            var allCustomers = db.Customers.ToList();
            Console.WriteLine("Alla kunder och deras bilar:\n");

            foreach (var c in allCustomers)
            {
                Console.WriteLine($"Kund: {c.Name}");
                var cars = db.Cars.Where(car => car.CustomerId == c.Id).ToList();
                foreach (var carItem in cars)
                {
                    Console.WriteLine($"  Bil: {carItem.Brand} {carItem.Model} ({carItem.Year})");
                }
            }
        }

        static void LäggTillNyKund(AppDbContext db)
        {
            Console.Write("Ange kundens namn: ");
            string name = Console.ReadLine();

            var newCustomer = new Customer { Name = name };
            db.Customers.Add(newCustomer);
            db.SaveChanges();

            Console.Write("Ange bilmärke: ");
            string brand = Console.ReadLine();

            Console.Write("Ange bilmodell: ");
            string model = Console.ReadLine();

            Console.Write("Ange årsmodell (t.ex. 2023): ");
            int year = int.Parse(Console.ReadLine());

            var newCar = new Car
            {
                Brand = brand,
                Model = model,
                Year = year,
                CustomerId = newCustomer.Id
            };

            db.Cars.Add(newCar);
            db.SaveChanges();

            Console.WriteLine("\n✅ Ny kund och bil har lagts till!");
        }

        static void LäggTillBilTillKund(AppDbContext db)
        {
            var customers = db.Customers.ToList();

            Console.WriteLine("Välj en kund att lägga till bil till:\n");
            for (int i = 0; i < customers.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {customers[i].Name}");
            }

            Console.Write("\nAnge kundnummer: ");
            int index = int.Parse(Console.ReadLine()) - 1;

            if (index < 0 || index >= customers.Count)
            {
                Console.WriteLine("Felaktigt val!");
                return;
            }

            var customer = customers[index];

            Console.Write("Ange bilmärke: ");
            string brand = Console.ReadLine();

            Console.Write("Ange bilmodell: ");
            string model = Console.ReadLine();

            Console.Write("Ange årsmodell (t.ex. 2023): ");
            int year = int.Parse(Console.ReadLine());

            var newCar = new Car
            {
                Brand = brand,
                Model = model,
                Year = year,
                CustomerId = customer.Id
            };

            db.Cars.Add(newCar);
            db.SaveChanges();

            Console.WriteLine($"\n✅ En ny bil har lagts till {customer.Name}!");
        }

        static void TaBortKundEllerBil(AppDbContext db)
        {
            Console.WriteLine("Vill du ta bort en (1) kund eller (2) bil?");
            string val = Console.ReadLine();

            if (val == "1")
            {
                var customers = db.Customers.ToList();
                Console.WriteLine("Välj kund att ta bort:\n");
                for (int i = 0; i < customers.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {customers[i].Name}");
                }

                Console.Write("\nAnge kundnummer: ");
                int index = int.Parse(Console.ReadLine()) - 1;

                if (index < 0 || index >= customers.Count)
                {
                    Console.WriteLine("Felaktigt val!");
                    return;
                }

                var customer = customers[index];
                db.Customers.Remove(customer);
                db.SaveChanges();

                Console.WriteLine($"\n🗑️ Kunden '{customer.Name}' har tagits bort (inklusive bilar).");
            }
            else if (val == "2")
            {
                var cars = db.Cars.ToList();
                Console.WriteLine("Välj bil att ta bort:\n");
                for (int i = 0; i < cars.Count; i++)
                {
                    var owner = db.Customers.FirstOrDefault(c => c.Id == cars[i].CustomerId);
                    Console.WriteLine($"{i + 1}. {cars[i].Brand} {cars[i].Model} ({cars[i].Year}) - Ägare: {owner?.Name}");
                }

                Console.Write("\nAnge bilnummer: ");
                int index = int.Parse(Console.ReadLine()) - 1;

                if (index < 0 || index >= cars.Count)
                {
                    Console.WriteLine("Felaktigt val!");
                    return;
                }

                var car = cars[index];
                db.Cars.Remove(car);
                db.SaveChanges();

                Console.WriteLine($"\n🗑️ Bilen {car.Brand} {car.Model} har tagits bort.");
            }
            else
            {
                Console.WriteLine("Ogiltigt val!");
            }
        }
    }
}
