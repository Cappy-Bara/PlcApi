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
        public int ConveyorId { get; set; }
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
                IsRunning = true;
        }
        public List<ConveyorPoint> ReturnOccupiedPoints()
        {
            var output = new List<ConveyorPoint>();
            int sign = IsTurnedDownOrLeft ? -1 : 1;
            if (IsVertical)
            {
                for (int i = 0; i < Length; i++)
                    output.Add(new ConveyorPoint(StartPoint.X, StartPoint.Y + i * sign, BoardId, ConveyorId));
            }
            else
            {
                for (int i = 0; i < Length; i++)
                    output.Add(new ConveyorPoint(StartPoint.X + i * sign, StartPoint.Y, BoardId, ConveyorId));
            }
            output.FirstOrDefault().isMainPoint = true;
            return output;
        }
        public void AddBlockToList(Pallet block)
        {
            PalletsOnConveyor.Add(block);
        }
        public void RemoveBlockFromList(Pallet block)
        {
            PalletsOnConveyor.Remove(block);
        }
        public void MoveAllBlocks()
        {
            int sign = IsTurnedDownOrLeft ? -1 : 1;
            if (IsVertical)
            {
                foreach (Pallet block in PalletsOnConveyor)
                {
                    block.PosY += sign * Speed;
                }
            }
            else
            {
                foreach (Pallet block in PalletsOnConveyor)
                {
                    block.PosX += sign * Speed;
                }
            }
        }
        public bool CheckIfBlockOnConveyor(Pallet block)
        {
            if (OccupiedPoints.Contains(
                new Point
                {
                X = block.PosX,
                Y = block.PosY,
                }))
                return true;
            return false;
        }
        public void RemoveBlocksNotOnConveyor()
        {
            foreach (Pallet block in PalletsOnConveyor)
            {
                if (CheckIfBlockOnConveyor(block))
                    PalletsOnConveyor.Remove(block);
            }
        }
        public void ConveyorWorkingScheme()
        {
            UpdateStatus();
            RemoveBlocksNotOnConveyor();
            if (IsRunning)
                MoveAllBlocks();
            RemoveBlocksNotOnConveyor();
        }
    }
}