using PlcApi.Models;

namespace PlcApi.Services.EntityServices
{
    public interface IPalletService
    {
        void CreatePallet(CreatePalletDto dto);
        void MovePalletsOnBoard(int boardId);
    }
}