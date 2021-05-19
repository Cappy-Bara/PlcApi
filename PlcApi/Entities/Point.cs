using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlcApi.Entities
{
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point() { }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Point stringToPoint(string pointInString)
        {
            string[] pointArray = pointInString.Split(";");
            int x = int.Parse(pointArray[0]);
            int y = int.Parse(pointArray[0]);
            return new Point(x,y);
        }

        public override string ToString()
        {
            return $"{X};{Y}";
        }
    



    }




}
