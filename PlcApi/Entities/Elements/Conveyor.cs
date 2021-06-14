using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PlcApi.Entities.Elements
{
    public class Conveyor
    {
        public int Id { get; set; }
        public int BoardId { get; set; }
        [NotMapped]
        public ConveyorPoint StartPoint { get; set; }
        public bool IsVertical { get; set; }
        public bool IsTurnedDownOrLeft { get; set; }
        public int Length { get; set; }
        public int Speed { get; set; }
        public bool IsRunning { get; set; }
        public virtual List<ConveyorPoint> OccupiedPoints { get; set; }
        public virtual List<Pallet> PalletsOnConveyor { get; set; }
        public int InputOutputId { get; set; }
        public virtual InputOutput InputOutput { get; set; }

        public Conveyor(ConveyorPoint mainPoint, int length, int speed)
        {
            mainPoint.isMainPoint = true;
            StartPoint = mainPoint;
            Length = length;
            Speed = speed;
        }
        public Conveyor()
        {
            ;
        }
        public void UpdateStatus()
        {
            if (InputOutput.Status == true)
                IsRunning = true;
            else
                IsRunning = false;
        }
        public List<ConveyorPoint> ReturnOccupiedPoints()
        {
            var output = new List<ConveyorPoint>();
            int sign = IsTurnedDownOrLeft ? -1 : 1;
            if (IsVertical)
            {
                for (int i = 0; i < Length; i++)
                    output.Add(new ConveyorPoint(StartPoint.X, StartPoint.Y + i * sign, BoardId, Id));
            }
            else
            {
                for (int i = 0; i < Length; i++)
                    output.Add(new ConveyorPoint(StartPoint.X + i * sign, StartPoint.Y, BoardId, Id));
            }
            output.FirstOrDefault().isMainPoint = true;
            return output;
        }


    }
}