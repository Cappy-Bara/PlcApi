using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlcApi.Models;

namespace PlcApi.Services.Interfaces
{
    public interface IConveyorService
    {
        public int AddConveyorToDb(int plcId, ConveyorDto dto);
        public void RefreshConveyorsStatus(int boardId);
        public List<ConveyorDto> ConveyorsOnBoard(int boardId);
    }
}
