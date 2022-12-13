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

        public int StartTime { get; set; }

        public int EndTime { get; set; }

        public double Distance
        {
            get
            {
                return Order.OrderDistance;
            }
        }

        public int StartTimeEstimation
        {
            get
            {
                //what in God's name is even that logic
                return (int)Math.Round(EndTime - Order.OrderDistance / Curier.Speed);
            }
        }

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
