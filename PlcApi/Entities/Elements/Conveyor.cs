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
        private ConveyorPoint _mainPoint;

        public int ConveyorId { get; set; }
        public int BoardId { get; set; }
        [NotMapped]
        public ConveyorPoint StartPoint 
        {
            get
            {
                return OccupiedPoints.FirstOrDefault(n => n.isMainPoint == true);
            }

            set
            {
                _mainPoint = value;
            }

        }
        public bool IsVertical { get; set; }
        public bool IsTurnedDownOrLeft { get; set; }
        public int Length { get; set; }
        public int Speed { get; set; }
        public bool IsRunning { get; set; }
        public virtual List<ConveyorPoint> OccupiedPoints { get; set; }
        public virtual List<Block> BlocksOnConveyor { get; set; }
        public int InputOutputId { get; set; }
        public virtual InputOutput InputOutput { get; set; }

        public Conveyor(ConveyorPoint mainPoint, int length, int speed)
        {
            mainPoint.isMainPoint = true;
            StartPoint = mainPoint;
            Length = length;
            UpdateStatus();
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
            return output;
        }



        public void AddBlockToList(Block block)
        {
            BlocksOnConveyor.Add(block);
        }
        public void RemoveBlockFromList(Block block)
        {
            BlocksOnConveyor.Remove(block);
        }
        public void MoveAllBlocks()
        {
            int sign = IsTurnedDownOrLeft ? -1 : 1;
            if (IsVertical)
            {
                foreach (Block block in BlocksOnConveyor)
                {
                    block.PosY += sign * Speed;
                }
            }
            else
            {
                foreach (Block block in BlocksOnConveyor)
                {
                    block.PosX += sign * Speed;
                }
            }
        }
        public bool CheckIfBlockOnConveyor(Block block)
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
            foreach (Block block in BlocksOnConveyor)
            {
                if (CheckIfBlockOnConveyor(block))
                    BlocksOnConveyor.Remove(block);
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