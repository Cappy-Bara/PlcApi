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
    public class PlcConnectionService : IPlcConnectionService
    {
        private void ThrowLostConnection()
        {
            throw new MyPlcException("Connection with PLC Lost");
        }
        private bool PlcIsConnected(Plc plc)
        {
            if (plc is null)
                throw new MyPlcException("First start your connection with the PLC!");
            if (!plc.IsConnected)
                return false;
            return true;
        }
        public void ThrowExceptionIfNotConnected(Plc plc)
        {
            var isConnected = PlcIsConnected(plc);
            if (!plc.IsConnected)
                ThrowLostConnection();
        }
    }
}
