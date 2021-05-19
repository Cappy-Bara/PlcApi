using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using PlcApi;
using PlcApi.Entities;

namespace UnitTests
{

    public class DiodeTests
    {
        public Diode diode = new Diode()
        {
            Status = "On",
            PosY = 0,
            PosX = 0,
        };


        [Fact]
        public void DiodeTest()
        {
            Assert.True(diode.Status == "On");
        }

    }
}
