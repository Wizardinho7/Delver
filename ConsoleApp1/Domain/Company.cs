using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SimpleCurriersSchedulerStudyApp.Domain
{
    /// <summary>
    /// Курьерская компания, которая выполняет заказы на перевозку грузов своими курьерами
    /// </summary>
    internal class Company
    {
        public delegate void EventHandler(object sender, EventArgs args);
        public event EventHandler OrderAdded = delegate { };



        public const double PricePerDistance = 100;

        public const double DefaultFootCurierSpeed = 4;

        public const double DefaultMobileCurierSpeed = 8;


        /// <summary>
        /// Курьеры
        /// </summary>
        public HashSet<Curier> Curiers { get; set; } = new HashSet<Curier>();

        /// <summary>
        /// Очередь поступающих заказов
        /// </summary>
        public Queue<Order> OrdersQueue { get; set; } = new Queue<Order>();

        /// <summary>
        /// Заказы компании
        /// </summary>
        public HashSet<Order> Orders { get; set; } = new HashSet<Order>();

        public List<PlanningOption> AllPossiblePlans { get; set; } = new List<PlanningOption>();
       
        public List<PlanningOption> AcceptedPlans { get; set; } = new List<PlanningOption>();

        /// <summary>
        /// Получает множество доступных на текущий момент курьеров
        /// </summary>
        /// <returns>Множество доступных курьеров</returns>
        public HashSet<Curier> GetAvailibleCurriers()
        {
            return Curiers;
        }

        /// <summary>
        /// Отображает заказы в консоли
        /// </summary>
        //TODO: Вынести метод - не относится к предметной логике
        public void PrintOrders()
        {
            foreach (var order in Orders)
            {
                Console.WriteLine(order.GetInfo());
            }
        }

        /// <summary>
        /// Отображает курьров в консоли
        /// </summary>
        //TODO: Вынести метод - не относится к предметной логике
        public void PrintCuriers()
        {
            foreach (var curier in Curiers)
            {
                Console.WriteLine(curier.GetInfo());
            }
        }

        ///adding Orders to a list and generating event
        public void AddOrder(Order order)
        {
            Orders.Add(order);
            OrderAdded?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Запускает цикл планирования заказов
        /// </summary>
        public void StartPlaner()
        {
            //PrepareQueue();
            //PlanningCycle();

            GetPlans();
            AcceptionPass();
            Console.WriteLine(CalculateProfit());
        }

        //todo: make it more efficient end less garbage
        public int GetPlans()
        {
            AllPossiblePlans.Clear();
            foreach (var curier in Curiers)
            {
                foreach (var plan in curier.PossiblePlan)
                {
                    AllPossiblePlans.Add(plan);
                }
            }
            return 0; //got new plans ))

            //return 1; //no new plans
            //return 2; // what
        }

        public int AcceptionPass()
        {
            AcceptedPlans.Clear();
            foreach (var planOption in AllPossiblePlans)
            {
                if (planOption.Profit >= 0)
                {
                    planOption.Curier.AcceptPlanAction(planOption);
                    planOption.Order.SetPlan(planOption);
                    AcceptedPlans.Add(planOption);
                }
            }
            return 0; //should be profit but i dont care rn
        }


        public double CalculateProfit()
        {
            var totalProfit = 0.0;
            foreach (var plan in AcceptedPlans) 
            {
                totalProfit += plan.Profit;
            }
            return totalProfit;
        }


        /*
        /// <summary>
        /// Подготовка очереди заказов к планирования
        /// </summary>
        private void PrepareQueue()
        {
            var sortedOrders = Orders.OrderByDescending(x => x.OrderPrice);

            foreach (var order in sortedOrders)
            {
                OrdersQueue.Enqueue(order);
            }
        }
        */

        /*
        /// <summary>
        /// Реализация цикла планирования заказов
        /// </summary>
        private void PlanningCycle()
        {
            var totalProfit = 0.0;

 

            while (OrdersQueue.Count > 0)
            {
                var orderForPlanning = OrdersQueue.Dequeue();

                Console.WriteLine($"Planning order: {orderForPlanning.GetInfo()}");
                Console.WriteLine();

                var result = orderForPlanning.PlanOrderAction();

                if (result)
                {
                    totalProfit += orderForPlanning.CurrentPlan.Profit;

                    Console.WriteLine($"Order had been planned: " +
                        $"{orderForPlanning.CurrentPlan.Curier.Name}" +
                        $" With profit: {Math.Round(orderForPlanning.CurrentPlan.Profit, 2)}");
                }
                else
                {
                    Console.WriteLine($"order was NOT planned");
                }
            }

            Console.WriteLine($"Summary profit: {Math.Round(totalProfit,2)}");
        }

        */

        /// <summary>
        /// Экземпляр компании (внутренее поле)
        /// </summary>
        private static Company _instance;

        /// <summary>
        /// Экземпляр компании
        /// </summary>
        public static Company CompanyInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Company();
                }

                return _instance;
            }
        }
    }    
}
