using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlcApi.Models
{
    public class CreateDiodeDto
    {
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int OutputByte { get; set; }
        public int OutputBit {get;set;}
    }
}
