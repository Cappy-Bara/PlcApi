using PlcApi.Models;

namespace PlcApi.Services.EntityServices
{
    public interface ISensorService
    {
        void AddSensorToDb(int plcId, SensorDto dto);
        void UpdateInputsStatus(int boardId);
    }
}