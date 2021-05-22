using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlcApi.Entities;
using PlcApi.Exceptions;

namespace PlcApi.Services
{
    public class BoardService
    {
        public Dictionary<int, List<Point>> Boards = new Dictionary <int, List<Point>>();

        public List<Point> FindBoardInList(int boardId) 
        {
            if (Boards.TryGetValue(boardId, out List<Point> board))
                return board;
            throw new NotFoundException("Board not found.");
        }
        public void AddPointsToBoard(int boardId,List<Point> newPoints)
        {
            var board = FindBoardInList(boardId);
            board.AddRange(newPoints);
        }











    }
}
