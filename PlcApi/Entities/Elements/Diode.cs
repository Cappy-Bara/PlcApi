using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlcApi.Entities
{
    public class Diode
    {
        [Key]
        public int DiodeId { get; set; }
        public string Status { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int OutputId { get; set; }
        public virtual InputOutput Output {get; set;}
    }
}
