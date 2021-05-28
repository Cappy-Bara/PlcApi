using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlcApi.Entities
{
    public class Point
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int BoardId { get; set; }



        public Point() { }
        public Point(int x, int y, int boardId)
        {
            X = x;
            Y = y;
            BoardId = boardId;
        }

    }




}
