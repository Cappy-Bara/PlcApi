using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlcApi.Exceptions;
using PlcApi.Services;
using S7.Net;
using Xunit;

namespace UnitTests
{
    public class PlcDataExchangeServiceUnitTests
    {
        public DatabaseService _service = new DatabaseService();
        public Plc plc = new Plc(CpuType.S71200,"127.0.0.1",0,1);
        public Plc plc1;

        [Fact]
        public void checkConnection_shouldNotConnect()
        {
            bool isConnected = _service.checkConnection(plc);
            Assert.False(isConnected);
        }

        [Fact]
        public void checkConnection_shouldThrowException()
        {
            Assert.Throws<MyPlcException>(() => _service.checkConnection(plc1));
        }













    }
}
