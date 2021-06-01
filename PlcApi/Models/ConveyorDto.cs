using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlcApi.Models
{
    public class ConveyorDto
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int BoardId { get; set; }
        public int OutputByte { get; set; }
        public int OutputBit { get; set; }
        public bool IsVertical { get; set; }
        public bool IsTurnedDownOrLeft { get; set; }
        public int Length { get; set; }
        public int Speed { get; set; }
    }
}
