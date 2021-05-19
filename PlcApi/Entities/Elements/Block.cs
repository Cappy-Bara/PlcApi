using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlcApi.Entities.Elements
{
    public class Block
    {
        public int BlockId { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int PlcId { get; set; }
        public virtual PlcEntity Plc { get; set; }
    }
}
