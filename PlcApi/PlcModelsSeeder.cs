using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlcApi.Entities;
using S7.Net;

namespace PlcApi
{
    public class PlcModelsSeeder
    {
        private readonly PlcDbContext _dbContext;

        public PlcModelsSeeder(PlcDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Models.Any())
                {
                    List<PlcModel> models = GetModels();
                    _dbContext.Models.AddRange(models);
                    _dbContext.SaveChanges();

                }
            }
        }
        private List<PlcModel> GetModels()
        {
            return new List<PlcModel>()
            {
                new PlcModel()
                {
                    CpuModel = 1200,
                    Cpu = CpuType.S71200,
                    Rack = 0,
                    Slot = 1
                },
                new PlcModel()
                {
                    CpuModel = 1500,
                    Cpu = CpuType.S71500,
                    Rack = 0,
                    Slot = 1
                },
                new PlcModel()
                {
                    CpuModel = 300,
                    Cpu = CpuType.S7300,
                    Rack = 0,
                    Slot = 2
                },
                new PlcModel()
                {
                    CpuModel = 400,
                    Cpu = CpuType.S7400,
                    Rack = 0,
                    Slot = 2
                }
            };
        }
    }
}
