using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlcApi.Models
{
    public class IOCreateDto
    {
        public int Byte { get; set; }
        public int Bit { get; set; }
        public string Type { get; set; }

    }
}
