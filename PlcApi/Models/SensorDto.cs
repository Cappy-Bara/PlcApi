using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlcApi.Models
{
    public class SensorDto
    {
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int BoardId { get; set; }
        public int OutputByte { get; set; }
        public int OutputBit { get; set; }
    }
}
