using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCurriersSchedulerStudyApp.Domain
{
    /// <summary>
    /// Это локация (точка в пространстве)
    /// </summary>
    public class Location
    {
        /// <summary>
        /// X
        /// </summary>
        public int XCoord { get; set; }

        /// <summary>
        /// Y
        /// </summary>
        public int YCoord { get; set; }

        public Location()
        {
        }

        /// <summary>
        /// Создает новый объект локаци
        /// </summary>
        /// <param name="xCoord">X</param>
        /// <param name="yCoord">Y</param>
        public Location(int xCoord, int yCoord)
        {
            XCoord = xCoord;
            YCoord = yCoord;
        }

        public static Location Create(int xCoord, int yCoord)
        {
            return new Location(xCoord, yCoord);
        }

        public string ToString()
        {
            return $"({XCoord},{YCoord})";
        }
    }

    public static class LocationHelper
    {
        public static double GetDistance(this Location from, Location destination)
        {
            return Math.Round(Math.Pow(
                Math.Pow(destination.XCoord - from.XCoord, 2)
                + Math.Pow(destination.YCoord - from.YCoord, 2)
                , 0.5), 2);
        }
    }
}
