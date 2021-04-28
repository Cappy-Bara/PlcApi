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

namespace PlcApi.Services
{
    public class PlcCommunicationService : IPlcCommunicationService
    {
        private readonly ILogger<PlcCommunicationService> _logger;
        private readonly PlcDbContext _dbContext;
        

        
        //przechowywanie listy typu plc tutaj?
        //tworzenie instancji plc w dbcontext tutaj?


        public PlcCommunicationService(ILogger<PlcCommunicationService> logger, PlcDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

            public Boolean GetSingleOutput(int byteAddress, int bitAddress,int plcId)
            {
                Plc _plc = FindPlc(plcId);
                if (_plc is null)
                    throw new MyPlcException("First start your connection with the PLC!");
                if (!_plc.IsConnected)
                    throw new MyPlcException("Connection with the PLC lost");
                else
                    return (Boolean)_plc.Read($"Q{byteAddress}.{bitAddress}");
            }
        
        public void StartPlcCommunication(int plcId)
        {
            Plc _plc = FindPlc(plcId);
            if(_plc==null)
                throw new MyPlcException("Plc does not exist");
            _plc.Open();
            if (!_plc.IsConnected)
            {
                throw new MyPlcException("Connection with the PLC lost");
            }
        }

        
        public void CreatePlc(string ip, int model)
        {
            PlcModel plcModel = _dbContext.Models.FirstOrDefault(m => m.CpuModel == model);
            _dbContext.PLCs.Add(
                new PlcEntity()
                {
                    ModelId = model,
                    Ip = ip,
                    Plc = new Plc(plcModel.Cpu, ip, plcModel.Rack, plcModel.Slot),
                    Model = plcModel
                }
                );
            _dbContext.SaveChanges();
        }
       
        public Plc FindPlc(int plcId)
        {
            return _dbContext.PLCs.FirstOrDefault(n => n.Id == plcId).Plc;
        }
        
    }
}
