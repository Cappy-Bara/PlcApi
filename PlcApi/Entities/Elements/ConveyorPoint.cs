using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlcApi.Entities.Elements
{
    public class ConveyorPoint : Point
    {
        [ForeignKey("ConveyorId")]
        public int ConveyorId { get; set; }
        public bool isMainPoint { get; set; }
        public virtual Conveyor OccupiedByConveyor { get; set; }

        public ConveyorPoint() : base()
        {
            ;
        }
        public ConveyorPoint(int x, int y, int BoardId) : base(x, y, BoardId)
        {
        }
        public ConveyorPoint(int x, int y, int BoardId,int conveyorId) : base(x, y, BoardId)
        {
            ConveyorId = conveyorId;
        }
    }
}
