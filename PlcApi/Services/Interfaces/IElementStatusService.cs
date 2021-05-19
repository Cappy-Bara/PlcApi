using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlcApi.Entities;
using PlcApi.Entities.Elements;
using PlcApi.Models;

namespace PlcApi.Services.Interfaces
{
    public interface IElementStatusService
    {
        public void UpdateDiodesStatus();
        public List<Diode> ReturnPlcDiodes(int plcId);
        public List<Block> ReturnPlcBlocks(int plcId);
    }
}
