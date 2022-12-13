using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCurriersSchedulerStudyApp.Domain
{
    /// <summary>
    /// Базовый заказ на перемещение груза, который может быть выполнен курьером
    /// </summary>
    internal class Order
    {
        /// <summary>
        /// Отправная точка заказа
        /// </summary>
        public Location FromLocation { get; set; }

        /// <summary>
        /// Пункт назначения заказа
        /// </summary>
        public Location ToLocation { get; set; }

        /// <summary>
        /// Вес посылки
        /// </summary>
        public double Weight { get; set; }

      

        /// <summary>
        /// Расстояние, на которое необходимо переместить посылку
        /// </summary>
        public double OrderDistance
        {
            get { return FromLocation.GetDistance(ToLocation); }
        }

        /// <summary>
        /// Базовая стоимость заказа
        /// </summary>
        public double OrderPrice
        {
            get
            {
                return GetOrderPrice();
            }
        }

        public int DeliveryTime { get; set; }


        /// <summary>
        /// Текущий вариант исполнения Заказа
        /// </summary>
        public PlanningOption CurrentPlan { get; private set; }

        /// <summary>
        /// Определяет, что заказ запланирован
        /// </summary>
        public bool IsPlanned { get { return CurrentPlan != null; } }

        /// <summary>
        /// Выполняет расчет стоимости Заказа по тарифу Компании
        /// </summary>
        /// <returns>Стоимость выполнения заказа по тарифу компании</returns>
        private double GetOrderPrice()
        {
            return OrderDistance * Company.PricePerDistance;
        }

        /// <summary>
        /// Формирует строковое представление с описанием Заказа
        /// </summary>
        /// <returns>Строка, содержащая информацию о Заказе</returns>
        public string GetInfo()
        {
            return $"Order {FromLocation.ToString()} -> {ToLocation.ToString()}" +
                $" ({OrderDistance} distance units) | {OrderPrice} $$ | {Weight} KG | {DeliveryTime} time";
        }

        public void SetPlan(PlanningOption planningOption)
        {
            CurrentPlan = planningOption;
        }

        public void Unplan()
        {
            CurrentPlan = null;
        }
        /*
        /// <summary>
        /// Базовый процесс планирования Заказа
        /// </summary>
        /// <returns></returns>
        public bool PlanOrderAction()
        {
            //Подбираем подходящих курьеров
            var curriers = FindCurriers();

            //Готовим "место" для ответов вариантов от Курьеров
            var planningOptions = new List<PlanningOption>();

            //Для каждого найденного курьера производится поиск варианта планирования
            foreach (var currier in curriers)
            {
                //Спрашиваем курьреа о варианте планирования
                var planningOption = currier.RequestPlanningOptionAction(this);

                //Если курьер вернул предложение, добавляем вариант в список
                if (planningOption != null)
                {
                    planningOptions.Add(planningOption);
                }
            }

            //Если мы получили варианты от курьеров, то производим анализ
            if (planningOptions.Count() > 0)
            {
                //Анализируем варианты и выбираем "лучший" для нас
                var bestOption = GetBestOption(planningOptions);

                //Если есть лучший вариант, то выбираем лучший
                if (bestOption != null)
                {
                    bestOption.Curier.AcceptPlanAction(bestOption);
                    CurrentPlan = bestOption;

                    return true;
                } else
                {
                    Console.WriteLine("POA:no best option for whatever reason (/?/)");
                }
            }
            Console.WriteLine("POA:No Options.");
            return false;
        }


        */

        /// <summary>
        /// Производит поиск курьеров для выполнения заказов
        /// </summary>
        /// <returns>Список подходящих курьеров</returns>        
        private IList<Curier> FindCurriers()
        {
            var curriers = Company.CompanyInstance.GetAvailibleCurriers()
                                .Where(x=>x.CanCarry(this));

            return curriers.ToList();
        }

        /// <summary>
        /// Выбирает лучший для заказа вариант планирования
        /// </summary>
        /// <param name="options">Варианты размещения заказов</param>
        /// <returns>Лучший вариант размещения</returns>
        private PlanningOption GetBestOption(IList<PlanningOption> options)
        {
            var sortedOption = options.OrderByDescending(option => option.Profit);

            Console.WriteLine("O_GBO: F option profit  " + options[0].Profit);

            var bestOption = sortedOption.FirstOrDefault(bestOption => bestOption.Profit > 0);

            return bestOption;
        }        
    }
}
