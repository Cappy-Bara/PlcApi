using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlcApi.Entities;
using PlcApi.Entities.Elements;
using PlcApi.Models;
using PlcApi.Services.Interfaces;

namespace PlcApi.Services
{
    public class ElementStatusService : IElementStatusService
    {
        private readonly PlcDbContext _dbContext;

        public ElementStatusService(PlcDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void UpdateDiodeInDb(Diode diode)
        {
            diode.UpdateStatus();
            _dbContext.Diodes.Update(diode);
        }
        
        public void UpdateDiodesStatus()
        {
            foreach(Diode diode in _dbContext.Diodes.Include(d => d.InputOutput))
            {
                UpdateDiodeInDb(diode);
            }
            _dbContext.SaveChanges();
        }

        public List<Diode> ReturnPlcDiodes(int plcId)
        {
            List<Diode> MyList = new List<Diode>();
            MyList.AddRange(
                _dbContext.Diodes.Where(n => n.InputOutput.PlcId == plcId)
                );
            return MyList;
        }

        public List<Block> ReturnPlcBlocks(int plcId)
        {
            List<Block> MyList = new List<Block>();
            MyList.AddRange(
                _dbContext.Blocks.Where(n => n.PlcId == plcId)
                );
            return MyList;
        }


    }
}
