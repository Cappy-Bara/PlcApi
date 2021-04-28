using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using S7.Net;

namespace PlcApi.Entities
{
    public static class PlcConverter
    {
        public static string toPlcProvider(Plc plc)
        {
            var str = $"{plc.IP}/{plc.CPU.ToString()}/{plc.Rack}/{plc.Slot}";
            return str;
        }
        public static Plc toPlc(string plcProv)
        {
            var tab = plcProv.Split("/");
            return new Plc(Enum.Parse<CpuType>(tab[1]),tab[0], short.Parse(tab[2]), short.Parse(tab[3]));
        }

    }
}
