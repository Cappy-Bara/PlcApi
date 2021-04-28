using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using S7.Net;

namespace PlcApi.Entities
{
    public class PlcEntity
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public int ModelId { get; set; }
        public virtual PlcModel Model { get; set; }
        public virtual List<InputOutput> InputOutput { get; set; }
        public virtual Plc Plc { get; set; }



    }
}
