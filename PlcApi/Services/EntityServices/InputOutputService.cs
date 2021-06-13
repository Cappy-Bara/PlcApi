using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PlcApi.Entities;
using PlcApi.Exceptions;
using PlcApi.Models;
using PlcApi.Services.Interfaces;
using S7.Net;

namespace PlcApi.Services.EntityServices
{
    public class InputOutputService : IInputOutputService
    {
        private readonly ILogger<InputOutputService> _logger;
        private readonly PlcDbContext _dbContext;
        private readonly IPlcStorageService _plcStorageService;
        private readonly IPlcDataReadingService _getPlcDataService;
        private readonly IPlcDataWritingService _writePlcDataService;

        public InputOutputService(ILogger<InputOutputService> logger, PlcDbContext dbContext, IPlcStorageService plcStorageService,
            IPlcDataReadingService readValueService, IPlcDataWritingService writeValueService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _plcStorageService = plcStorageService;
            _getPlcDataService = readValueService;
            _writePlcDataService = writeValueService;
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
        public void RefreshInputsAndOutputs(int plcId)
        {
            var plc = _plcStorageService.GetPlc(plcId);

            RefreshInputs(plcId, plc);
            RefreshOutputs(plcId, plc);
        }

        private void RefreshInputs(int plcId, Plc plc)
        {
            foreach(InputOutput input in _dbContext.InputsOutputs.Where(n => n.PlcId == plcId && n.Type == IOType.Input))
            {
                _writePlcDataService.WriteSingleBit(plc, input.Byte, input.Bit, "I", input.Status);
                _dbContext.InputsOutputs.Update(input);
            }
        }
        private void RefreshOutputs(int plcId, Plc plc)
        {
            foreach (InputOutput output in _dbContext.InputsOutputs.Where(n => n.PlcId == plcId && n.Type == IOType.Output))
            {
                output.Status = _getPlcDataService.GetSingleBitStatus(plc, output.Byte, output.Bit, "Q");
                _dbContext.InputsOutputs.Update(output);
            }
        }

        public InputOutput FindOrCreateIOInDb(int plcId, int ioByte, int ioBit, IOType type)
        {
            var io = FindInputOutputInDb(plcId, ioByte, ioBit, type);
            if (io == null)
                io = AddInputOutputToDb(plcId, ioByte, ioBit, type);
            return io;
        }
    }
}
