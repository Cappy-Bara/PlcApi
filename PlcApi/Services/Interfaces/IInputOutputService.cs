using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlcApi.Entities;
using PlcApi.Models;

namespace PlcApi.Services.Interfaces
{
    public interface IInputOutputService
    {
        public InputOutput FindInputOutputInDb(int plcId, int ioByte, int ioBit, IOType type);
        public InputOutput AddInputOutputToDb(int plcId, int bit, int myByte, IOType type);
        public int AddInputOutputToDb(int plcId, IOCreateDto dto);
        public void RefreshInputsAndOutputs(int plcId);


    }
}
