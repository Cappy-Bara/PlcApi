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

namespace PlcApi.Services.EntityServices
{
    public class PalletService : IPalletService
    {
        private readonly PlcDbContext _dbContext;
        private readonly IMapper _mapper;

        public PalletService(PlcDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public void CreatePallet(CreatePalletDto dto)
        {
            Pallet pallet = _mapper.Map<CreatePalletDto, Pallet>(dto);
            _dbContext.Pallets.Add(pallet);
            _dbContext.SaveChanges();
        }

        public void MovePalletsOnConveyor(Conveyor conveyor)
        {
            int sign = conveyor.IsTurnedDownOrLeft ? -1 : 1;
            if (conveyor.IsVertical)
            {
                foreach (Pallet block in conveyor.PalletsOnConveyor)
                {
                    block.PosY += sign * conveyor.Speed;
                }
            }
            else
            {
                foreach (Pallet block in conveyor.PalletsOnConveyor)
                {
                    block.PosX += sign * conveyor.Speed;
                }
            }
        }

        public void UpdatePalletesOnConveyors(int boardId)
        {
            var conveyorPoints = _dbContext.Conveyors.Include(n => n.OccupiedPoints)
                                    .Where(n => n.BoardId == boardId)
                                    .SelectMany(m => m.OccupiedPoints)
                                    .ToList();
            var palletsOnBoard = _dbContext.Pallets.Where(m => m.BoardId == boardId);
            foreach(Pallet pallet in palletsOnBoard)
            {
                var usedConveyorPoint = conveyorPoints.FirstOrDefault(m => m.X == pallet.PosX && m.Y == pallet.PosY);
                if (usedConveyorPoint != null)
                    pallet.ConveyorId = usedConveyorPoint.ConveyorId;
                else
                    pallet.ConveyorId = null;
            }
            _dbContext.SaveChanges();
        }
        public void MovePalletsOnBoard(int boardId)
        {
            UpdatePalletesOnConveyors(boardId);
            var runningConveyors = _dbContext.Conveyors.Where(n => n.BoardId == boardId && n.IsRunning)
                                                       .Include(n => n.OccupiedPoints)
                                                       .Include(n => n.PalletsOnConveyor);

            foreach (Conveyor conveyor in runningConveyors)
            {
                MovePalletsOnConveyor(conveyor);
            }
            UpdatePalletesOnConveyors(boardId);
            _dbContext.SaveChanges();
        }
    }





}
