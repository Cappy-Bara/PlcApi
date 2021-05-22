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
    public class PlcDataExchangeService : IPlcDataExchangeService
    {
        private readonly ILogger<PlcDataExchangeService> _logger;
        private readonly PlcDbContext _dbContext;

        //możliwe że trzeba usunąć
        public PlcDataExchangeService()
        {

        }

        public PlcDataExchangeService(ILogger<PlcDataExchangeService> logger, PlcDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public bool checkConnection(Plc plc)
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
            var isConnected = checkConnection(plc);

            if (!isConnected)
                ThrowLostConnection();
            return ReadSingleBitFromPlc(plc, byteAddress, bitAddress, type);
        }
        public void WriteSingleBit(Plc plc, int byteAddress, int bitAddress, string type,bool value)
        {
            var isConnected = checkConnection(plc);
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

        public int AddDiodeToDb(int plcId, CreateDiodeDto dto)
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
        
        public void RefreshInputsAndOutputs(Plc plc,int plcId)
        {
            if (!plc.IsConnected)
                throw new MyPlcException("No connection with PLC.");

            List<InputOutput> IOList = _dbContext.InputsOutputs.Where(n => n.PlcId == plcId).ToList();
            foreach(InputOutput io in IOList)
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


    }
}
