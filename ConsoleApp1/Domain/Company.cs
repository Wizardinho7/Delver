using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;


namespace SimpleCurriersSchedulerStudyApp.Domain
{
    /// <summary>
    /// Курьерская компания, которая выполняет заказы на перевозку грузов своими курьерами
    /// </summary>
    internal class Company
    {
        public delegate void EventHandler(object sender, EventArgs args);
        public event EventHandler EVENT = delegate { };

        

        public const double PricePerDistance = 100;

        public const double DefaultFootCurierSpeed = 4;

        public const double DefaultMobileCurierSpeed = 8;

        public double Profit, ProfitPrev = 0;

        /// <summary>
        /// Magickal model of time, ranging from 0 to 100
        /// </summary>
        public int NowTime = 0;

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
            Console.WriteLine(order.GetInfo());
            EVENT?.Invoke(this, new EventArgs());
        }
        public void AddCurier(Curier curier)
        {
            Curiers.Add(curier);
            Console.WriteLine(curier.GetInfo());
            EVENT?.Invoke(this, new EventArgs());
        }

        public void DeleteOrder(Order order)
        {
            //rewrite this bs in events someday
            foreach (var cur in Curiers)
            {
                cur.UnPlanOrder(order);
            }

            Orders.Remove(order);
            EVENT?.Invoke(this, new EventArgs());
        }
        public void DeleteCurier(Curier curier)
        {
            //unplanning orders
            foreach (var ord in Orders)
            {
                if (ord.CurrentPlan != null)
                {
                    if (ord.CurrentPlan.Curier == curier)
                    {
                        ord.Unplan();
                    }
                }
            }

            Curiers.Remove(curier);
            EVENT?.Invoke(this, new EventArgs());
        }


        /// <summary>
        /// Запускает цикл планирования заказов
        /// </summary>
        public void StartPlaner()
        {
            //PrepareQueue();
            //PlanningCycle();

            Console.WriteLine("=======PLANNING==PASS=========================");
            GetPlans();

            Console.WriteLine("=======ACCEPTION==PASS========================");
            ProfitPrev = Profit;
            Profit = AcceptionPass();

//            if (CalculateFullProfit() < 0)
 //           {
  //              StartPlaner();
   //         }
            Console.WriteLine("DeltaProfit ::: " + CalculateFullProfit());
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

        public double AcceptionPass()
        {
            AcceptedPlans.Clear();

            //PrevOpt = AllPossiblePlans.First<PlanningOption>;

            var PrevOpt = new PlanningOption();

            foreach (var planOption in AllPossiblePlans)
            {
                //if (planOption.Profit >= 0)
                if (PrevOpt.Curier == planOption.Curier)
                {
                    if (planOption.Profit >= PrevOpt.Profit)
                    {
                        planOption.Curier.AcceptPlanAction(planOption);
                        planOption.Order.SetPlan(planOption);
                        AcceptedPlans.Add(planOption);
                    }
                    else
                    {
                        //???????????????????????????????
                        PrevOpt.Curier.AcceptPlanAction(PrevOpt);
                        PrevOpt.Order.SetPlan(PrevOpt);
                        AcceptedPlans.Add(PrevOpt);
                        //???????????????????????????????

                    }

                }
                else
                {
                    planOption.Curier.AcceptPlanAction(planOption);
                    planOption.Order.SetPlan(planOption);
                    AcceptedPlans.Add(planOption);
                }




                PrevOpt = planOption;
            }
            return CalculateStepProfit();
        }


        public double CalculateStepProfit()
        {
            var totalProfit = 0.0;
            foreach (var plan in AcceptedPlans) 
            {
                totalProfit += plan.Profit;
            }
            return totalProfit;
        }

        public double CalculateFullProfit()
        {
            var totalProfit = 0.0;
            foreach (var ord in Orders)
            {
                if (ord.CurrentPlan != null)
                {
                    totalProfit += ord.CurrentPlan.Profit;
                }
            }
            return totalProfit;
        }

        public void PingEvent()
        {
            EVENT?.Invoke(this, new EventArgs());
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
