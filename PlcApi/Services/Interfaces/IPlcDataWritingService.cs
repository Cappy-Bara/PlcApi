using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S7.Net;

namespace PlcApi.Services.Interfaces
{
    public interface IPlcDataWritingService
    {
        public void WriteSingleBit(Plc plc, int byteAddress, int bitAddress, string type, bool value);

    }
}
