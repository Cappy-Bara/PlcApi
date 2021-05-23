using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlcApi.Entities;

namespace PlcApi.Services.Interfaces
{
    public interface IBoardService
    {
        public List<Point> FindBoardInList(int boardId);
        public void AddPointsToBoard(int boardId, List<Point> newPoints);

        public void CreateBoard(int boardId);

        public void SizeModified();
    }
}
