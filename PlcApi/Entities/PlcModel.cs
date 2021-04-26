using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S7.Net;

namespace PlcApi.Entities
{
    public class PlcModel
    {
        public int Id { get; set; }
        public int CpuModel { get; set; }
        public CpuType Cpu { get; set; }
        public short Rack { get; set; }
        public short Slot { get; set; }
    }
}
