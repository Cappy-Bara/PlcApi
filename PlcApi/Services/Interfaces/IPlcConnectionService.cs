using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S7.Net;

namespace PlcApi.Services.Interfaces
{
    public interface IPlcConnectionService
    {
//        public void ThrowLostConnection();
//        public bool PlcIsConnected(Plc plc);
        public void ThrowExceptionIfNotConnected(Plc plc);

    }
}
