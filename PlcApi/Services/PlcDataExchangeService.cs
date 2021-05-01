﻿using System;
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

namespace PlcApi.Services
{
    public class PlcDataExchangeService : IPlcDataExchangeService
    {
        private readonly ILogger<PlcDataExchangeService> _logger;
        private readonly PlcDbContext _dbContext;



        public PlcDataExchangeService(ILogger<PlcDataExchangeService> logger, PlcDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public Boolean GetSingleOutput(Plc plc, int byteAddress, int bitAddress)
        {
            if (plc is null)
                throw new MyPlcException("First start your connection with the PLC!");
            if (!plc.IsConnected)
                throw new MyPlcException("Connection with the PLC lost");
            else
                return (Boolean)plc.Read($"Q{byteAddress}.{bitAddress}");
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
                OutputId = io.Id,
                Output = io,
            };
            return _dbContext.Diodes.Add(diode).Entity.DiodeId;
        }

        //REFRESH INPUT OUTPUT

        //NOWY SERWIS TYPU ELements handler, który będzie zmeniał stan utilities przy każdym evencie.

    }
}
