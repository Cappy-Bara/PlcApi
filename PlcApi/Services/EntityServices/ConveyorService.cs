﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public ConveyorService(ILogger<ConveyorService> logger, PlcDbContext dbContext, IInputOutputService ioService,
            IMapper mapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _ioService = ioService;
            _mapper = mapper;
        }


        public int AddConveyorToDb(int plcId, ConveyorDto dto)
        {
            var io =_ioService.FindOrCreateIOInDb(plcId, dto.OutputByte, dto.OutputBit, IOType.Output);

            var startPoint = FindConveyorPoint(dto.BoardId, dto.X, dto.Y);
            if (startPoint != null)
                throw new Exception("Conveyor collides with something!");
            startPoint = new ConveyorPoint(dto.X, dto.Y, dto.BoardId);
            startPoint.isMainPoint = true;
            Conveyor conveyor = new Conveyor(startPoint, dto.Length, dto.Speed)
            {
                IsTurnedDownOrLeft = dto.IsTurnedDownOrLeft,
                IsVertical = dto.IsVertical,
                InputOutputId = io.Id,
                BoardId = dto.BoardId,
                StartPoint = startPoint,
                Speed = dto.Speed
            };
            conveyor = _dbContext.Conveyors.Add(conveyor).Entity;
            _dbContext.SaveChanges();
            var occupiedPoints = conveyor.ReturnOccupiedPoints();


            foreach (ConveyorPoint occupiedPoint in occupiedPoints)
            {
                var point = FindConveyorPoint(dto.BoardId, occupiedPoint.X, occupiedPoint.Y);
                if (point != null)
                    throw new Exception("Conveyor collides with something!");
            }
            occupiedPoints.FirstOrDefault(n => n.X == startPoint.X && n.Y == startPoint.Y).isMainPoint = true;
            _dbContext.ConveyorPoints.AddRange(occupiedPoints);
            _dbContext.SaveChanges();
            return conveyor.Id;
        }
        public ConveyorPoint FindConveyorPoint(int boardId, int x, int y)
        {
            return _dbContext.ConveyorPoints.FirstOrDefault(n =>
            n.BoardId == boardId &&
            n.X == x &&
            n.Y == y
            );
        }
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
        public List<ConveyorDto> ConveyorsOnBoard(int boardId)
        {
            List<ConveyorDto> output = new List<ConveyorDto>();
            var conveyors = _dbContext.Conveyors.Where(n => n.BoardId == boardId).Include(n => n.OccupiedPoints).Include(m => m.InputOutput);
            foreach (Conveyor conveyor in conveyors)
                conveyor.StartPoint = conveyor.OccupiedPoints.FirstOrDefault(n => n.isMainPoint);
            return _mapper.Map<List<ConveyorDto>>(conveyors);
        }
    }
}
