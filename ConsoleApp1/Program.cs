using System;
using System.Security.Cryptography.X509Certificates;
using SimpleCurriersSchedulerStudyApp.Domain;


namespace SimpleCurriersSchedulerStudyApp
{
    internal class Program
    {
        //public List<Curier> currr = new List<Curier>();
        //public List<Order> orders = new List<Order>();


        static Curier GenerateRandomCURR(Company companyy)
        {
            Random rand = new Random();
            Curier CURCUR =  new FootCurier
            {
                company = companyy,
                Name = rand.Next(100).ToString(),
                InitialLocation = Location.Create(rand.Next(200), rand.Next(200)),
                CarryingCapacity = rand.Next(3, 15),
                WorkshiftStart = rand.Next(0, 50),
                WorkshiftStop = rand.Next(50, 100)
            };
            return CURCUR;
        }

        static Order GenerateRandomOrder()
        {
            Random random = new Random();
            Order order = new Order 
            {
                DeliveryTime = random.Next(0,100),
                FromLocation = Location.Create(random.Next(200), random.Next(200)),
                ToLocation = Location.Create(random.Next(200), random.Next(200)),
                Weight = random.Next(10)
            };
            return order;
        }

        static void Main(string[] args)
        {
            var companyMain = Company.CompanyInstance;

            Random random = new Random();

            ///generating curiers for absolute fun time
            for(var i = 0; i < 5; i++)
            {
                companyMain.AddCurier(GenerateRandomCURR(companyMain));
            }

            foreach (var curier in companyMain.Curiers)
            {
                curier.LikeAndSubscribe();
            }


            ///generating orders for an absolutely fantastic time
            for (var i = 0; i < 200; i++)
            {
                companyMain.AddOrder(GenerateRandomOrder());
            }


           // companyMain.PrintOrders();
            Console.WriteLine();
           // companyMain.PrintCuriers();
            Console.WriteLine();
           

            companyMain.StartPlaner();

            //BullshitProcessing?.Invoke();

            while (true)
            {

                companyMain.StartPlaner();
                
                foreach (var order in companyMain.Orders)
                {
                    if (order.CurrentPlan != null)
                    {
                        Console.WriteLine(order.CurrentPlan.GetInfo());
                        Console.WriteLine();
                    }
                }

                Console.WriteLine("1 - YEET random order");
                Console.WriteLine("2 - YEET random curier");
                Console.WriteLine("3 - ADD random order");
                Console.WriteLine("4 - ADD random curier");



                var str = Console.ReadLine();

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();


                switch (str){
                    case "1":
                        companyMain.DeleteOrder(companyMain.Orders.ElementAt(random.Next(companyMain.Orders.Count())));
                        break;

                    case "2":
                        companyMain.DeleteCurier(companyMain.Curiers.ElementAt(random.Next(companyMain.Curiers.Count())));
                        break;

                    case "3":
                        companyMain.AddOrder(GenerateRandomOrder());
                        break;

                    case "4":
                        var currrrr = GenerateRandomCURR(companyMain);
                        companyMain.AddCurier(currrrr);
                        currrrr.LikeAndSubscribe();
                        break;

                    default: 
                        Console.WriteLine("what the hell did you type in? Invoking event");
                        companyMain.PingEvent();
                        break;
                }

                


            }




            Console.ReadKey();
        }
    }
}



/*
 * 
 * 
 * 
 * 

            var currier1 = new
                FootCurier
            {
                company = companyMain,
                Name = "Vasya",
                InitialLocation = Location.Create(5, 5),
                CarryingCapacity = 5,
                WorkshiftStart = 10,
                WorkshiftStop = 90
            };

        
            var currier2 = new
                FootCurier
            {
                company = companyMain,
                Name = "Petri",
                InitialLocation = Location.Create(3, 3),
                CarryingCapacity = 6,
                WorkshiftStart = 15,
                WorkshiftStop = 90
            };

            var currier3 = new
                MobileCurier
            {
                company = companyMain,
                Name = "Nikolay",
                InitialLocation = Location.Create(1, 1),
                CarryingCapacity = 8,
                WorkshiftStart = 5,
                WorkshiftStop = 95
            };

            var currier4 = new
                MobileCurier
            {

                company = companyMain,
                Name = "Nina",
                InitialLocation = Location.Create(3, 3),
                CarryingCapacity = 2,
                WorkshiftStart = 10,
                WorkshiftStop = 90
            };
           

            var order1 = new Order
            {
                FromLocation = Location.Create(3, 3),
                ToLocation = Location.Create(2, 2),
                Weight = 5,
                DeliveryTime = 60
            };


            var order2 = new Order
            {
                FromLocation = Location.Create(5, 5),
                ToLocation = Location.Create(5, 1),
                Weight = 10,
                DeliveryTime = 80
            };

            var order3 = new Order
            {
                FromLocation = Location.Create(1, 5),
                ToLocation = Location.Create(1, 1),
                Weight = 3,
                DeliveryTime = 20
            };

            var order4 = new Order
            {
                FromLocation = Location.Create(2, 2),
                ToLocation = Location.Create(3, 3),
                Weight = 3,
                DeliveryTime = 55
            };

            var order5 = new Order
            {
                FromLocation = Location.Create(1, 5),
                ToLocation = Location.Create(5, 1),
                Weight = 10,
                DeliveryTime = 30
            };
        
            companyMain.AddCurier(currier1);
            companyMain.AddCurier(currier2);
            companyMain.AddCurier(currier3);
            companyMain.AddCurier(currier4);



*/