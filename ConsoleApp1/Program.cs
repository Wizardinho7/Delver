using System;
using System.Security.Cryptography.X509Certificates;
using SimpleCurriersSchedulerStudyApp.Domain;

namespace SimpleCurriersSchedulerStudyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var companyMain = Company.CompanyInstance;



            var currier1 = new
                FootCurier
            {
                company = companyMain,
                Name = "Vasya",
                InitialLocation = Location.Create(5, 5),
                CarryingCapacity = 5
            };

        
            var currier2 = new
                FootCurier
            {
                company = companyMain,
                Name = "Petri",
                InitialLocation = Location.Create(3, 3),
                CarryingCapacity = 6
            };

            var currier3 = new
                MobileCurier
            {
                company = companyMain,

                Name = "Nikolay",
                InitialLocation = Location.Create(1, 1),
                CarryingCapacity = 8
            };

            var currier4 = new
                MobileCurier
            {

                company = companyMain,
                Name = "Nina",
                InitialLocation = Location.Create(3, 3),
                CarryingCapacity = 2
            };

            var order1 = new Order
            {
                FromLocation = Location.Create(3, 3),
                ToLocation = Location.Create(2, 2),
                Weight = 5
            };

            var order2 = new Order
            {
                FromLocation = Location.Create(5, 5),
                ToLocation = Location.Create(5, 1),
                Weight = 10
            };

            var order3 = new Order
            {
                FromLocation = Location.Create(1, 5),
                ToLocation = Location.Create(1, 1),
                Weight = 3
            };

            var order4 = new Order
            {
                FromLocation = Location.Create(2, 2),
                ToLocation = Location.Create(3, 3),
                Weight = 3
            };

            var order5 = new Order
            {
                FromLocation = Location.Create(1, 5),
                ToLocation = Location.Create(5, 1),
                Weight = 10
            };
        
            companyMain.Curiers.Add(currier1);
            companyMain.Curiers.Add(currier2);
            companyMain.Curiers.Add(currier3);
            companyMain.Curiers.Add(currier4);

            foreach (var curier in companyMain.Curiers)
            {
                curier.LikeAndSubscribe();
            }

            companyMain.AddOrder(order1);
            companyMain.AddOrder(order2);
            companyMain.AddOrder(order3);
            companyMain.AddOrder(order4);
            companyMain.AddOrder(order5);
            

            /*
            company.PrintOrders();
            Console.WriteLine();
            company.PrintCuriers();
            Console.WriteLine();
            */

            companyMain.StartPlaner();

            //BullshitProcessing?.Invoke();

            foreach(var order in companyMain.Orders)
            {
                if (order.CurrentPlan != null)
                {
                    Console.WriteLine(order.CurrentPlan.GetInfo());
                }
            }



            Console.ReadKey();
        }
    }
}