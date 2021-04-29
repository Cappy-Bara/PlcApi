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
    public class PlcDataExchangeService : IPlcDataExchangeService
    {
        private readonly ILogger<PlcDataExchangeService> _logger;
        private readonly PlcDbContext _dbContext;
        

        
        //przechowywanie listy typu plc tutaj?
        //tworzenie instancji plc w dbcontext tutaj?
            

        public PlcDataExchangeService(ILogger<PlcDataExchangeService> logger, PlcDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        
        public Boolean GetSingleOutput(Plc plc,int byteAddress, int bitAddress)
        {
            if (plc is null)
                throw new MyPlcException("First start your connection with the PLC!");
            if (!plc.IsConnected)
                throw new MyPlcException("Connection with the PLC lost");
            else
                return (Boolean)plc.Read($"Q{byteAddress}.{bitAddress}");
        }
        
    }
}
