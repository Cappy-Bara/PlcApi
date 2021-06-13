using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlcApi.Entities;
using PlcApi.Entities.Elements;
using PlcApi.Models;
using PlcApi.Services.Interfaces;

namespace PlcApi.Services.EntityServices
{
    public class SensorService : ISensorService
    {
        private readonly PlcDbContext _dbContext;
        private readonly IInputOutputService _ioService;
        private readonly IMapper _mapper;

        public SensorService(PlcDbContext dbContext, IInputOutputService ioService, IMapper mapper)
        {
            _dbContext = dbContext;
            _ioService = ioService;
            _mapper = mapper;
        }

        public void AddSensorToDb(int plcId, SensorDto dto)
        {
            var io = _ioService.FindOrCreateIOInDb(plcId, dto.OutputByte, dto.OutputBit, IOType.Input);

            Sensor sensor = new Sensor()
            {
                BoardId = dto.BoardId,
                InputOutputId = io.Id,
                PosX = dto.PosX,
                PosY = dto.PosY
            };

            _dbContext.Sensors.Add(sensor);
            _dbContext.SaveChanges();
        }
        public void UpdateInputsStatus(int boardId)
        {
            var pallets = _dbContext.Pallets.Where(m => m.BoardId == boardId).ToList();
            foreach (Sensor sensor in _dbContext.Sensors.Where(n => n.BoardId == boardId).Include(m => m.InputOutput))
            {
                if (pallets.Where(n => n.PosX == sensor.PosX && n.PosY == sensor.PosY).Count() != 0)
                {
                    sensor.IsSensing = true;
                    sensor.InputOutput.Status = true;
                }
                else
                {
                    sensor.IsSensing = false;
                    sensor.InputOutput.Status = false;
                }
            }
            _dbContext.SaveChanges();
        }
    }
}
