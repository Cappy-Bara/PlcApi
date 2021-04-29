using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using S7.Net;

namespace PlcApi.Entities
{
    public class MyPlc
    {
        public int UserId { get; set; }
        public string Ip { get; set; }
        public Plc Plc { get;}

        public MyPlc(string ip,int userId)
        {
            Plc = new Plc(CpuType.S71200, ip, 0, 1);
            UserId = userId;
            Ip = ip;
        }
    }
}
