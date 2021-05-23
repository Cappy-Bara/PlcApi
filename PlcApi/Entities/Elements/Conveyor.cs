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
        public int _x;
        public int _y;
        public bool _isVertical;
        public bool _isTurnedDownOrLeft;
        public int _length;

        public int ConveyorId { get; set; }
        public int BoardId { get; set; }
        public int X 
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
                UpdateOccupiedPointsList();
            }
        }
        public int Y 
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
                UpdateOccupiedPointsList();
            }
        }
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
        public int InputOutputId { get; set; }
        public virtual InputOutput InputOutput { get; set; }

        public Conveyor()
        {
            OccupiedPoints = new List<Point>();
            BlocksOnConveyor = new List<Block>();
            UpdateStatus();
            UpdateOccupiedPointsList();
        }
        public Conveyor(int x, int y, int length, int speed)
        {
            X = x;
            Y = y;
            OccupiedPoints = new List<Point>();
            BlocksOnConveyor = new List<Block>();
            Length = length;
            UpdateStatus();
            UpdateOccupiedPointsList();
        }

        public event EventHandler<List<Point>> OccupiedPointsChanged; 

        public void UpdateStatus()
        {
            if (InputOutput.Status == true)
                IsRunning = true;
            else
                IsRunning = true;
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
            OccupiedPointsChanged?.Invoke(this, OccupiedPoints);
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