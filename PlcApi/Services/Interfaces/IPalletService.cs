using System.Collections.Generic;
using PlcApi.Entities.Elements;
using PlcApi.Models;

namespace PlcApi.Services.EntityServices
{
    public interface IPalletService
    {
        void CreatePallet(CreatePalletDto dto);
        void MovePalletsOnBoard(int boardId);
        public List<Pallet> GetAllPalletsOnBoard(int boardId);
    }
}