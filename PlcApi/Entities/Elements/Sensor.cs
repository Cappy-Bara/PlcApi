using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlcApi.Entities.Elements
{
    public class Sensor
    {
        public int Id { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int BoardId { get; set; }
        public int InputOutputId { get; set; }
        public virtual InputOutput InputOutput { get; set; }
        public bool IsSensing { get; set; }
    }
}
