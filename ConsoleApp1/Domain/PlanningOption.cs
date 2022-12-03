using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCurriersSchedulerStudyApp.Domain
{
    /// <summary>
    /// Вариант планирования выполнения заказ на курьером
    /// </summary>
    internal class PlanningOption
    {
        /// <summary>
        /// Заказ
        /// </summary>
        public Order Order { get; set; }

        /// <summary>
        /// Курьер
        /// </summary>
        public Curier Curier { get; set; }

        /// <summary>
        /// Себестоимость выполнения заказа Курьром
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Приыбыль
        /// </summary>
        public double Profit
        {
            get
            {
                return Order.OrderPrice - Price;
            }
        }

        public string GetInfo()
        {
            return Order.GetInfo() + "  " + Curier.GetInfo() + "   ";
        }
    }
}
