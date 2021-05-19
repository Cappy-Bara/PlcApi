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
        public int X { get; set; }
        public int Y { get; set; }

        public bool _isVertical;
        public bool _isTurnedDownOrLeft;
        public int _length;
        public bool IsVertical
        {
            get
            {
                return _isVertical;
            }
            set
            {
                _isVertical = value;
                UpdateOccupiedPointsList();
            }
        }
        public bool IsTurnedDownOrLeft
        {
            get
            {
                return _isTurnedDownOrLeft;
            }
            set
            {
                _isTurnedDownOrLeft = value;
                UpdateOccupiedPointsList();
            }
        }
        public int Length
        {
            get
            {
                return _length;
            }
            set
            {
                _length = value;
                UpdateOccupiedPointsList();
            }
        }
        public int Speed { get; set; }
        [NotMapped]
        public bool IsRunning { get; set; }
        [NotMapped]
        public List<Point> OccupiedPoints { get; set; }
        [NotMapped]
        public List<Block> BlocksOnConveyor { get; set; }

        public Conveyor()
        {
            OccupiedPoints = new List<Point>();
            BlocksOnConveyor = new List<Block>();
        }
        public Conveyor(int x, int y, int length, int speed)
        {
            X = x;
            Y = y;
            OccupiedPoints = new List<Point>();
            BlocksOnConveyor = new List<Block>();
            Length = length;
        }
        public void AddPointToList(int x, int y)
        {
            OccupiedPoints.Add(new Point(x, y));
        }
        public void UpdateOccupiedPointsList()
        {
            OccupiedPoints.Clear();
            int sign = IsTurnedDownOrLeft ? -1 : 1;
            if (IsVertical)
            {
                for (int i = 0; i < Length; i++)
                    AddPointToList(X, Y + i * sign);
            }
            else
            {
                for (int i = 0; i < Length; i++)
                    AddPointToList(X + i * sign,Y);
            }
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

            if (IsRunning)
            {
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
            RemoveBlocksNotOnConveyor();
            MoveAllBlocks();
            RemoveBlocksNotOnConveyor();
        }
    }
}
