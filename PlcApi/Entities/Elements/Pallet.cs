using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlcApi.Entities.Elements
{
    public class Pallet
    {
        public int PalletId { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int BoardId { get; set; }
        public int ConveyorId { get; set; }
        public virtual Conveyor Conveyor { get; set; }
    }
}
