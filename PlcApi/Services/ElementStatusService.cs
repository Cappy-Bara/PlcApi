using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlcApi.Entities;
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

        public void UpdateDiodesStatus()
        {
            //var diodes = _dbContext.Diodes.Include(d => d.InputOutput);
            foreach(Diode diode in _dbContext.Diodes.Include(d => d.InputOutput))
            {
                if (diode.InputOutput == null)
                    continue;
                if (diode.InputOutput.Status == true)        //Nie widzi obiektu Output w diodzie.  JAkaś kolekcja w IO?
                    diode.Status = "On";
                else
                    diode.Status = "Off";
                _dbContext.Diodes.Update(diode);
            }
            _dbContext.SaveChanges();
        }

        public List<Diode> ReturnDiodeStatus(int plcId)
        {
            List<Diode> MyList = new List<Diode>();
            MyList.AddRange(
                _dbContext.Diodes.Where(n => n.InputOutput.PlcId == plcId)
                );
            return MyList;
        }


    }
}
