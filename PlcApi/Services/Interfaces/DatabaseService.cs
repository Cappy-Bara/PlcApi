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
        private readonly IInputOutputService _ioService;
        private readonly IPlcStorageService _plcService;


        public DatabaseService()
        {

        }
        public DatabaseService(ILogger<DatabaseService> logger, PlcDbContext dbContext, IInputOutputService ioService, IPlcStorageService plcService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _ioService = ioService;
            _plcService = plcService;           //DA SIE TU WSTRZYKNĄĆ TEN SERWIS, CO ZAŁATWIA WIELE!!!

        }


        public int AddPlcToDb(PlcEntity dto) 
        {             //sprawdzanie czy istnieje już plc dla danego użytkownika/maila
                      //
            var addedValue = _dbContext.PLCs.Add(dto).Entity;
            _dbContext.SaveChanges();
            return addedValue.Id;
        }
    }
}
