using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCurriersSchedulerStudyApp.Domain
{
    /// <summary>
    /// Курьер, способный выполнять заказы на доставку грузов
    /// </summary>
    internal abstract class Curier
    {
        public Company company { get; set; }
        

        /// <summary>
        /// Наименование курьера, например ФИО
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Начальное местоположение
        /// </summary>
        public Location InitialLocation { get; set; }

        /// <summary>
        /// Грузоподъемность (вместимость ранца курьера)
        /// </summary>
        public double CarryingCapacity { get; set; }

        /// <summary>
        /// Скорость
        /// </summary>
        public double Speed { get; set; }

        /// <summary>
        /// Стоимость курьреа
        /// </summary>
        public double CurrierPrice { get; set; }


        /// <summary>
        /// The start of a workshift.
        /// </summary>
        public int WorkshiftStart { get; set; }

        /// <summary>
        /// The end of a workshift.
        /// </summary>
        public int WorkshiftStop { get; set; }



        /// <summary>
        /// -1 - not work
        /// 0 - IDLE
        /// 1 - wrking))0
        /// </summary>
        public int[] TimePlan = new int[100];

        public int[] PossibleTimePlan = new int[100];

        public List<PlanningOption>? PossiblePlan { get; set; } = new List<PlanningOption>();




        public int[] CalculateTimePlan(Order order)
        {
            int[] result = new int[100];
            int[] ordTime = new int[100];



            var ftd = CalculateFullTimeToDeliver(order);
            if (ftd != null)
            {
                ordTime = ftd;
                Console.WriteLine("CURR:::: FTD not null");



                for (int i = 0; i < ordTime.Length; i++)
                {
                    result[i] += ordTime[i];
                    Console.Write(result[i]);
                    if ((PossibleTimePlan[i] + result[i] > 1))
                    {
                        Console.WriteLine("CURR::::WEAKDENIAL");
                        //можно тут разработать метод с решением накладок
                        return null;
                    }

                    if (result[i] > 1 || (TimePlan[i] == -1 && ordTime[i] != 0))
                    {
                        Console.WriteLine("CURR_CTP::::: bad_time");
                        return null;
                    }

                }
            }
            else
            {
                return null;
            }
            


            return result;
        }

        public int CalTime(Location location, Location location1)
        {
            var time = location.GetDistance(location1) / Speed;
            int timee = (int)Math.Round(time);
            return timee;
        }


        public int[] CalculateOrderTime(Order order)
        {
            //CalTime(order.FromLocation,order.ToLocation);
            //order.DeliveryTime

            int[] res = new int[100];
            for (int i = order.DeliveryTime; 
                i > (order.DeliveryTime - CalTime(order.FromLocation, order.ToLocation)); 
                i--)
            {
                if (i < 0)
                {
                    UnPlanOrder(order);
                    return null;
                }
                else
                {
                    if (TimePlan[i] != 0) // МИССИЯ НЕ ВЫПОЛНИМА(допилить условие потом)
                    {
                        UnPlanOrder(order);
                        return null;
                    }
                }
                res[i] = 1;
            }
            return res;
        }

        public int[] CalculateTimeToReach(Order order)
        {
            //CalTime(InitialLocation, order.FromLocation);
            //(order.DeliveryTime - CalTime(order.FromLocation, order.ToLocation)

            int[] res = new int[100];
            for (int i = order.DeliveryTime - CalTime(order.FromLocation, order.ToLocation);
                i > ((order.DeliveryTime - CalTime(order.FromLocation, order.ToLocation)) - CalTime(InitialLocation, order.FromLocation));
                i--)
            {
                if (i < 0)
                {
                    Console.WriteLine("CURR_TTR:::: Cantreach");
                    UnPlanOrder(order);
                    return null;

                }
                else
                {
                    if (TimePlan[i] != 0) // МИССИЯ НЕ ВЫПОЛНИМА(допилить условие потом)
                    {
                        Console.WriteLine("CURR_TTR:::: Cantreach");
                        UnPlanOrder(order);
                        return null;

                    }

                }
                res[i] = 1;
            }
            return res;

        }

        public int[] CalculateFullTimeToDeliver(Order order)
        {
            int[] ints = new int[100];

            var OT = CalculateOrderTime(order);
            var TTD = CalculateTimeToReach(order);

            if (TTD != null && OT != null)
            {

                for (var i = 0; i < TimePlan.Length; i++)
                {
                    ints[i] += OT[i] + TTD[i];
                    
                    
                    if (ints[i] > 1)
                    {
                        Console.WriteLine("ZOPAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                    }
                
                
                }
                return ints;
            }
            else
            {
                Console.WriteLine("CURR:::::::FTTD NULL");

                return null;
            }

        }


        public void LikeAndSubscribe()
        {
            company.EVENT += (sender, args) => { OrderAddedEvent(sender, args); };

            //init timeline
            for (int i = 0; i < TimePlan.Length; i++)
            {
                if (i <= WorkshiftStart || i >= WorkshiftStop)
                {
                    TimePlan[i] = -1;
                }
                else
                {
                    TimePlan[i] = 0;
                }

            }



            Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        }

        private void OrderAddedEvent(object? sender, EventArgs e)
        {
            Console.WriteLine("CURIER------------------------------------------" + this.Name);
            
            PossiblePlan.Clear();
            PossibleTimePlan = new int[100];


            // тут должно быть сложное условие принятия заказа, но пока что оно простое(мы свободны)
            if (ScheduledOrders.Count() == 0) //todo: Distance culling
            {
                Console.WriteLine("CURR::::No SCH orders");

                foreach(var order in company.Orders)
                {

                    Console.WriteLine();
                    Console.WriteLine("ORDER---" + order.GetInfo());

                    if (!(order.IsPlanned) && (order.Weight <= CarryingCapacity) ) 
                    {
                        Console.WriteLine("CURR:::: ord weight&notplaned");
                        if (CalculateTimePlan(order) != null)
                        {
                            Console.WriteLine("CURR::::Adding possible plan(timeplan!=0), ");
                            PossiblePlan.Add(RequestPlanningOptionAction(order));
                            PossibleTimePlan = CalculateTimePlan(order);
                        }
                    }
                }
            }

            //CalculateTimePlan();

        }



        /// <summary>
        /// Проверяет, может ли курьер выполнить Заказ
        /// </summary>
        /// <param name="order">Заказ на перемещение неоторого груза</param>
        /// <returns>Истина, если курьер способен выполнить заказ, ложь - в других случаях</returns>
        public bool CanCarry(Order order)
        {
            Console.WriteLine("CC: {}" + Name + " /// " + (CarryingCapacity >= order.Weight));

            //TODO: Расчёт грузоподъймности с учётом уже принятых грузов
            return CarryingCapacity >= order.Weight;
        }

        public void UnPlanOrder(Order order)
        {
            foreach(var sch in ScheduledOrders)
            {
                if (sch == order)
                {
                    ScheduledOrders.Remove(sch);
                    break;
                }
            }
        }

        /// <summary>
        /// Формирует строковое представление с описанием курьера
        /// </summary>
        /// <returns>Строка, содержащая информацию о курьере</returns>
        public string GetInfo()
        {
            return string.Format("Curier: {0}|" +
                " Speed: {1} |" +
                " WeightMax {2} |" +
                " Now in {3}",
                Name, Speed, CarryingCapacity, InitialLocation.ToString());
        }

        /// <summary>
        /// Действие по размещению заказа в плане курьера
        /// </summary>
        /// <param name="planningOption">Вариант размещения заказа в плане курьера</param>        
        internal void AcceptPlanAction(PlanningOption planningOption)
        {
            ScheduledOrders.AddLast(planningOption.Order);
            if (CalculateTimePlan(planningOption.Order) != null)
            {
                var temp = CalculateTimePlan(planningOption.Order);
                for (int i = 0; i < TimePlan.Length; i++)
                {
                    if (i <= WorkshiftStart || i >= WorkshiftStop)
                    {
                        TimePlan[i] = -1;
                    }
                    else
                    {
                        TimePlan[i] = temp[i];
                    }

                }



                ///TimePlan = CalculateTimePlan(planningOption.Order);
            }
            PossibleTimePlan = new int[100];
            PossiblePlan.Clear();
        }

        /// <summary>
        /// Действие по формированию варианта размещения заказа в плане курьера
        /// </summary>
        /// <param name="order">Заказ, рассматриваемый к размещению в плане</param>
        /// <returns>Вариант размещения, включающий оценки</returns>        
        internal PlanningOption RequestPlanningOptionAction(Order order)
        {
            var planningOption = new PlanningOption();

            var currentCurrierLocation =  ScheduledOrders.LastOrDefault()?.ToLocation ?? InitialLocation;

            var distance = currentCurrierLocation.GetDistance(order.FromLocation) + order.OrderDistance;
            var currierCost = distance * this.CurrierPrice;

            planningOption.Curier = this;
            planningOption.Order = order;
            planningOption.Price = currierCost;

            Console.WriteLine("REVENUE: " + planningOption.Profit);

            return planningOption;
        }

        

        private LinkedList<Order> ScheduledOrders = new LinkedList<Order>();
    }
    /// <summary>
    /// Пеший курьер
    /// </summary>
    internal class FootCurier : Curier
    {
        public FootCurier()
        {
            Speed = Company.DefaultFootCurierSpeed;
            CurrierPrice = Company.PricePerDistance * 0.45;
        }
    }

    /// <summary>
    /// Мобильный курьер
    /// </summary>
    internal class MobileCurier : Curier
    {
        public MobileCurier()
        {
            Speed = Company.DefaultMobileCurierSpeed;
            CurrierPrice = Company.PricePerDistance * 0.55;
        }
    }

}
