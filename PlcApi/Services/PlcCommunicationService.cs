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
using S7.Net;

namespace PlcApi.Services
{
    public class PlcCommunicationService : IPlcCommunicationService
    {
        private readonly ILogger<PlcCommunicationService> _logger;
        private readonly PlcDbContext _dbContext;
        
        private  Plc _plc = null;
        private readonly string ipAddress = "127.0.0.1";
        private readonly short Rack = 0;
        private readonly short Slot = 1;

        //przechowywanie listy typu plc tutaj?
        //mapper?


        public PlcCommunicationService(ILogger<PlcCommunicationService> logger, PlcDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public Boolean GetSingleOutput(int byteAddress, int bitAddress)
        {
            if (_plc is null)
                throw new MyPlcException("First start your connection with the PLC!");
            if (!_plc.IsConnected)
                throw new MyPlcException("Connection with the PLC lost");
            else
                return (Boolean)_plc.Read($"Q{byteAddress}.{bitAddress}");
        }


        public void StartPlcCommunication()
        {
            _plc = new Plc(CpuType.S71200, ipAddress, Rack, Slot);
            _plc.Open();
            if (!_plc.IsConnected)
            {
                throw new MyPlcException("Connection with the PLC lost");
            }
            
        }
    }
}
