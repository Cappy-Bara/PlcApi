using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlcApi.Entities;
using PlcApi.Models;
using S7.Net;

namespace PlcApi.Services.Interfaces
{
    public interface IPlcDataExchangeService
    {
        public Boolean GetSingleBit(Plc plc, int byteAddress, int bitAddress, string type);
        public void WriteSingleBit(Plc plc, int byteAddress, int bitAddress, string type, bool value);
        public int AddPlcToDb(PlcEntity dto);
        public int AddInputOutputToDb(int plcId, IOCreateDto IO);
        public InputOutput AddInputOutputToDb(int plcId, int bit, int myByte, IOType type);
        public int AddDiodeToDb(int plcId, CreateDiodeDto dto);
        public int AddBlockToDb(int plcId, CreateBlockDto dto);
        public void RefreshInputsAndOutputs(Plc plc, int plcId);

    }
}
