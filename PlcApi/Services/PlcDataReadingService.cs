using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlcApi.Exceptions;
using PlcApi.Services.Interfaces;
using S7.Net;

namespace PlcApi.Services
{
    public class PlcDataReadingService : IPlcDataReadingService
    {
        private readonly IPlcConnectionService _connectionService;

        public PlcDataReadingService(IPlcConnectionService connectionService)
        {
            _connectionService = connectionService;
        }
        public Boolean GetSingleBitStatus(Plc plc, int byteAddress, int bitAddress, string type)
        {
            _connectionService.ThrowExceptionIfNotConnected(plc);
            return ReadSingleBitFromPlc(plc, byteAddress, bitAddress, type);
        }
        private Boolean ReadSingleBitFromPlc(Plc plc, int byteAddress, int bitAddress, string type)
        {
            return (Boolean)plc.Read($"{type}{byteAddress}.{bitAddress}");
        }

    }
}
