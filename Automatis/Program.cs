using Automatis.Data;
using Automatis.Models;
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
                            paus();
                            break;

                        case "2":
                            LäggTillKund(db);
                            Pause();
                            break;
                        case "3":
                            LäggTillBill(db);
                            Pause();
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
                            Pause();
                            break;
                    }
                }




            }


        }

        //lägger in några kunder med bilar första gången programmet körs

        static void InitDemoData(AppDbContext)
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

            
            }
           

        }


    }
}
