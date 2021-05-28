using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PlcApi.Entities;

namespace PlcApi.Services.EntityServices
{
    public class ConveyorService
    {
        private readonly ILogger<ConveyorService> _logger;
        private readonly PlcDbContext _dbContext;

        public ConveyorService(ILogger<ConveyorService> logger, PlcDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }








    }
}
