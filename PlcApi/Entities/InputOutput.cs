using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlcApi.Entities
{
    public class InputOutput
    {
        [Key]
        public int Id { get; set; }
        public int Bit { get; set; }
        public int Byte { get; set; }
        [EnumDataType(typeof(IOType))]
        public IOType Type { get; set; }
        public int PlcId { get; set; }
        public virtual PlcEntity Plc { get; set; }
        public bool Status { get; set; }
    }
}
