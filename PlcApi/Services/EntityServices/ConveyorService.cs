using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PlcApi.Entities;
using PlcApi.Entities.Elements;
using PlcApi.Models;
using PlcApi.Services.Interfaces;

namespace PlcApi.Services.EntityServices
{
    public class ConveyorService : IConveyorService
    {
        private readonly ILogger<ConveyorService> _logger;
        private readonly PlcDbContext _dbContext;
        private readonly IInputOutputService _ioService;

        public ConveyorService(ILogger<ConveyorService> logger, PlcDbContext dbContext, IInputOutputService ioService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _ioService = ioService;

        }


        public int AddConveyorToDb(int plcId, CreateConveyorDto dto)
        {

            var io = _ioService.FindInputOutputInDb(plcId, dto.OutputByte, dto.OutputBit, IOType.Output);
            if (io == null)
                io = _ioService.AddInputOutputToDb(plcId, dto.OutputBit, dto.OutputByte, IOType.Output);

            var startPoint = FindConveyorPoint(dto.BoardId, dto.X, dto.Y);
            if (startPoint != null)
                throw new Exception("Conveyor collides with something!");

            startPoint = new ConveyorPoint(dto.X, dto.Y, dto.BoardId);

            Conveyor conveyor = new Conveyor(startPoint, dto.Length, dto.Speed)
            {
                IsTurnedDownOrLeft = dto.IsTurnedDownOrLeft,
                IsVertical = dto.IsVertical,
                InputOutputId = io.Id,
                BoardId = dto.BoardId
            };

            var conveyorId = _dbContext.Conveyors.Add(conveyor).Entity.ConveyorId;
            var occupiedPoints = conveyor.ReturnOccupiedPoints();

            foreach(ConveyorPoint occupiedPoint in occupiedPoints)
            {
                var Point = FindConveyorPoint(dto.BoardId, occupiedPoint.X, occupiedPoint.Y);
                if (startPoint != null)
                    throw new Exception("Conveyor collides with something!");
            }
            occupiedPoints.First().isMainPoint = true;
            _dbContext.ConveyorPoints.AddRange(occupiedPoints);
            _dbContext.SaveChanges();
            return conveyorId;
        }
        public ConveyorPoint FindConveyorPoint(int boardId, int x, int y)
        {
            return _dbContext.ConveyorPoints.FirstOrDefault(n =>
            n.BoardId == boardId &&
            n.X == x &&
            n.Y == y
            );
        }

        //zmienić na user id?
        public void RefreshConveyorsStatus(int boardId)
        {
            foreach (Conveyor conveyor in _dbContext.Conveyors.Include(n => n.InputOutput).Where(n => n.BoardId == boardId))
                UpdateConveyorStatusInDb(conveyor);
            _dbContext.SaveChanges();
        }
        private void UpdateConveyorStatusInDb(Conveyor conveyor)
        {
            conveyor.UpdateStatus();
            _dbContext.Conveyors.Update(conveyor);
        }



    }
}
