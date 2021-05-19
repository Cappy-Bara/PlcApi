using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [ForeignKey("InputOutputId")]
        public int InputOutputId { get; set; }
        public virtual InputOutput InputOutput {get; set;}


        public void UpdateStatus()
        {
            if (InputOutput.Status == true)
                Status = "On";
            else
                Status = "Off";
        }











    }






}
