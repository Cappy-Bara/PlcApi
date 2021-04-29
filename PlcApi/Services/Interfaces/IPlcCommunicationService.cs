using S7.Net;

namespace PlcApi.Services
{
    public interface IPlcCommunicationService
    {

        public bool AddPlc(int userId, string ip);
        public bool StartCommunication(int userId);
        public bool StopCommunication(int userId);
        public Plc GetPlc(int userId);
    }
}