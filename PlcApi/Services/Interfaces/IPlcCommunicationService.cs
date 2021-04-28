using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlcApi.Services.Interfaces
{
    public interface IPlcCommunicationService
    {
       public Boolean GetSingleOutput(int byteAddress, int bitAddress, int plcId);
       public void StartPlcCommunication(int plcId);
       public void CreatePlc(string ip, int model);
    }
}
