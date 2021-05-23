using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlcApi.Entities;
using PlcApi.Exceptions;
using PlcApi.Services.Interfaces;

namespace PlcApi.Services
{
    public class BoardService :IBoardService
    {
        public Dictionary<int, List<Point>> Boards = new Dictionary <int, List<Point>>();

        //USUWANIE PUNKTÓW ZRELACJONOWANYCH DO KONKRETNEGO OBIEKTU, PO JEGO USUNIĘCIU/UPDACIE!
        //Eventy wymagają konkretnej instancji obiektu, to chyba zła droga!
        //Event handler w dbContext, który wywołuje tu metodę!
        //musi być blokada do tworzenia obiektów na sobie. Database?


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
        public void CreateBoard(int boardId)
        {
            Boards.Add(boardId, new List<Point>());
        }

        public void SizeModified()
        {
            ;
        }
        











    }
}
