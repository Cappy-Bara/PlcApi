using PlcApi.Entities;
using S7.Net;

namespace PlcApi.Services
{
    public interface IPlcCommunicationService
    {

        public void AddPlc(int plcId, string ip, PlcModel model);
        public void StartCommunication(int plcId);
        public void StopCommunication(int plcId);
        public Plc GetPlc(int plcId);
        public void DeletePlc(int plcId);

    }
}