using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PlcApi.Services.Interfaces;
using S7.Net;

namespace PlcApi.Services
{
    public class PlcCommunicationService : IPlcCommunicationService
    {
        private readonly ILogger<PlcCommunicationService> _logger;
        private  Plc _plc = null;
        private readonly string ipAddress = "127.0.0.1";
        private readonly short Rack = 0;
        private readonly short Slot = 1;

        public PlcCommunicationService(ILogger<PlcCommunicationService> logger)
        {
            _logger = logger;
        }

        public int GetSingleOutput(int byteAddress, int bitAddress)
        {
            if (!_plc.IsConnected)
                throw new PlcException(S7.Net.ErrorCode.ConnectionError);
            else
                return (int)_plc.Read($"Q{byteAddress}.{bitAddress}");
        }


        public void StartPlcCommunication()
        {
            _plc = new Plc(CpuType.S71200, ipAddress, Rack, Slot);
            _plc.Open();
            if (!_plc.IsConnected)
            {
                throw new PlcException(ErrorCode.ConnectionError);
            }
            
        }
    }
}
