using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PlcApi.Entities;
using PlcApi.Models;
using PlcApi.Services.Interfaces;

namespace PlcApi.Services.EntityServices
{
    public class DiodeService : IDiodeService
    {
        private readonly ILogger<DiodeService> _logger;
        private readonly PlcDbContext _dbContext;
        private readonly IInputOutputService _ioService;

        public DiodeService(ILogger<DiodeService> logger, PlcDbContext dbContext, IInputOutputService ioService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _ioService = ioService;
        }


        private void UpdateDiodeInDb(Diode diode)
        {
            diode.UpdateStatus();
            _dbContext.Diodes.Update(diode);
        }
        public void RefreshDiodesStatus(int plcId)
        {
            foreach (Diode diode in _dbContext.Diodes.Include(d => d.InputOutput).Where(n => n.InputOutput.PlcId == plcId))
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
        public int AddDiodeToDb(int plcId, CreateDiodeDto dto)  
        {
            var io = _dbContext.InputsOutputs.FirstOrDefault(
                    n => n.PlcId == plcId &&
                    n.Bit == dto.OutputBit &&
                    n.Byte == dto.OutputByte &&
                    n.Type == IOType.Output
                    );
            if (io == null)
                io = _ioService.AddInputOutputToDb(plcId, dto.OutputBit, dto.OutputByte, IOType.Output);

            Diode diode = new Diode()
            {
                PosX = dto.PosX,
                PosY = dto.PosY,
                InputOutputId = io.Id,
            };
            var id = _dbContext.Diodes.Add(diode).Entity.DiodeId;
            _dbContext.SaveChanges();
            return id;
        }
    }
}
