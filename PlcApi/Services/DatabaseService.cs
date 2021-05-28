using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PlcApi.Entities;
using PlcApi.Exceptions;
using PlcApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using S7.Net;
using PlcApi.Models;
using PlcApi.Entities.Elements;

namespace PlcApi.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly ILogger<DatabaseService> _logger;
        private readonly PlcDbContext _dbContext;
        
        public DatabaseService()
        {

        }
        public DatabaseService(ILogger<DatabaseService> logger, PlcDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public bool CheckConnection(Plc plc)
        {
            if (plc is null)
                throw new MyPlcException("First start your connection with the PLC!");
            if (!plc.IsConnected)
                return false;
            return true;
        }
        public Boolean ReadSingleBitFromPlc(Plc plc, int byteAddress, int bitAddress, string type)
        {
            return (Boolean)plc.Read($"{type}{byteAddress}.{bitAddress}");
        }
        public void WriteSingleBitToPlc(Plc plc, int byteAddress, int bitAddress, string type, bool value)
        {
            plc.Write($"{type}{byteAddress}.{bitAddress}", value);
        }
        public void ThrowLostConnection()
        {
            throw new MyPlcException("Connection with PLC Lost");
        }
        public Boolean GetSingleBit(Plc plc, int byteAddress, int bitAddress, string type)
        {
            var isConnected = CheckConnection(plc);

            if (!isConnected)
                ThrowLostConnection();
            return ReadSingleBitFromPlc(plc, byteAddress, bitAddress, type);
        }
        public void WriteSingleBit(Plc plc, int byteAddress, int bitAddress, string type,bool value)
        {
            var isConnected = CheckConnection(plc);
            if (!plc.IsConnected)
                ThrowLostConnection();
            else
                WriteSingleBitToPlc(plc, byteAddress, bitAddress, type, value);
        }
        public int AddPlcToDb(PlcEntity dto) {             //sprawdzanie czy istnieje już plc dla danego użytkownika/maila
                                                           //
            var addedValue = _dbContext.PLCs.Add(dto).Entity;
            _dbContext.SaveChanges();
            return addedValue.Id;
        }
        public void RefreshInputsAndOutputs(Plc plc, int plcId)
        {
            if (!plc.IsConnected)
                throw new MyPlcException("No connection with PLC.");

            List<InputOutput> IOList = _dbContext.InputsOutputs.Where(n => n.PlcId == plcId).ToList();
            foreach (InputOutput io in IOList)
            {
                if (io.Type == IOType.Output)
                {
                    io.Status = GetSingleBit(plc, io.Byte, io.Bit, "Q");
                    _dbContext.InputsOutputs.Update(io);
                }
                else if (io.Type == IOType.Input)
                    WriteSingleBit(plc, io.Byte, io.Bit, "I", io.Status);
                _dbContext.SaveChanges();
            }

        }

        public int AddInputOutputToDb(int plcId, IOCreateDto dto)
        {
            var plc = _dbContext.PLCs.FirstOrDefault(n => n.Id == plcId) ?? throw new NotFoundException("This Plc does not exist.");
            object? type;
            if (!Enum.TryParse(typeof(IOType), dto.Type, out type))
                throw new NotFoundException("Wrong type. Select Input or output.");
            var addedValue = _dbContext.InputsOutputs.Add(
                new InputOutput
                {
                    PlcId = plcId,
                    Plc = plc,
                    Byte = dto.Byte,
                    Bit = dto.Bit,
                    Type = (IOType)type
                }
                ).Entity;
            _dbContext.SaveChanges();
            return addedValue.Id;
        }
        public InputOutput AddInputOutputToDb(int plcId, int bit, int myByte, IOType type)
        {
            var plc = _dbContext.PLCs.FirstOrDefault(n => n.Id == plcId) ?? throw new NotFoundException("This Plc does not exist.");
            var addedValue = _dbContext.InputsOutputs.Add(
                new InputOutput
                {
                    PlcId = plcId,
                    Plc = plc,
                    Byte = myByte,
                    Bit = bit,
                    Type = type
                }
                ).Entity;
            _dbContext.SaveChanges();
            return addedValue;
        }
        public InputOutput FindInputOutputInDb(int plcId, int ioByte, int ioBit, IOType type)
        {
            return _dbContext.InputsOutputs.FirstOrDefault(
            n => n.PlcId == plcId &&
            n.Bit == ioBit &&
            n.Byte == ioByte &&
            n.Type == type
            );
        }

        //Do innego serwisu

        public ConveyorPoint FindConveyorPoint(int boardId, int x, int y)
        {
            return _dbContext.ConveyorPoints.FirstOrDefault(n =>
            n.BoardId == boardId &&
            n.X == x &&
            n.Y == y
            );
        }


        public int AddDiodeToDb(int plcId, CreateDiodeDto dto)      //REFACTOR!
        {
            var io = _dbContext.InputsOutputs.FirstOrDefault(
                    n => n.PlcId == plcId &&
                    n.Bit == dto.OutputBit &&
                    n.Byte == dto.OutputByte &&
                    n.Type == IOType.Output
                    );
            if (io == null)
                io = AddInputOutputToDb(plcId, dto.OutputBit, dto.OutputByte, IOType.Output);

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
        public int AddBlockToDb(int plcId, CreateBlockDto dto)
        {
            //Czy trzeba przehowywać bloki w db?
            Block block = new Block()
            {
                PosX = dto.PosX,
                PosY = dto.PosY,
                PlcId = plcId,
            };
            var id = _dbContext.Blocks.Add(block).Entity.BlockId;
            _dbContext.SaveChanges();
            return id;
        }
        public int AddConveyorToDb(int plcId, CreateConveyorDto dto)
        {
            var io = FindInputOutputInDb(plcId, dto.OutputByte, dto.OutputBit, IOType.Output);
            if (io == null)
                io = AddInputOutputToDb(plcId, dto.OutputBit, dto.OutputByte, IOType.Output);

            var startPoint = FindConveyorPoint(dto.BoardId, dto.X, dto.Y);
            if (startPoint != null)
                throw new Exception("Conveyor collides with something!");

            startPoint = new ConveyorPoint(dto.X,dto.Y,dto.BoardId);

            Conveyor conveyor = new Conveyor(startPoint, dto.Length, dto.Speed)
            {
                IsTurnedDownOrLeft = dto.IsTurnedDownOrLeft,
                IsVertical = dto.IsVertical,
                InputOutputId = io.Id,
                BoardId = dto.BoardId
            };

            var conveyorId = _dbContext.Conveyors.Add(conveyor).Entity.ConveyorId;
            var occupiedPoints = conveyor.ReturnOccupiedPoints();
            occupiedPoints.First().isMainPoint = true;
            _dbContext.ConveyorPoints.AddRange(occupiedPoints);
            _dbContext.SaveChanges();
            return conveyorId;
        }


    }
}
