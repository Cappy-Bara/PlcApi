using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlcApi.Services.Interfaces;
using S7.Net;

namespace PlcApi.Services
{
    public class PlcDataWritingService : IPlcDataWritingService
    {
        private readonly IPlcConnectionService _connectionService;


        public PlcDataWritingService(IPlcConnectionService connectionService)
        {
            _connectionService = connectionService;
        }
        public void WriteSingleBit(Plc plc, int byteAddress, int bitAddress, string type, bool value)
        {
            _connectionService.ThrowExceptionIfNotConnected(plc);
            WriteSingleBitToPlc(plc, byteAddress, bitAddress, type, value);
        }
        public void WriteSingleBitToPlc(Plc plc, int byteAddress, int bitAddress, string type, bool value)
        {
            plc.Write($"{type}{byteAddress}.{bitAddress}", value);
        }

    }
}
