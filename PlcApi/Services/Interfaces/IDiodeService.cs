using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlcApi.Entities;
using PlcApi.Models;

namespace PlcApi.Services.Interfaces
{
    public interface IDiodeService
    {
        public int AddDiodeToDb(int plcId, CreateDiodeDto dto);
        public void RefreshDiodesStatus(int plcId);
        public List<Diode> ReturnPlcDiodes(int plcId);
    }
}
