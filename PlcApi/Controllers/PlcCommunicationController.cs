using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlcApi.Entities;
using PlcApi.Services;
using PlcApi.Services.Interfaces;
using S7.Net;

namespace PlcApi.Controllers
{
    [ApiController]
    [Route("plc")]
    public class PlcCommunicationController : ControllerBase
    {
        private readonly IPlcDataExchangeService _dataExchangeService;
        private readonly IPlcCommunicationService _communicationService;
        private readonly PlcDbContext _dbContext;

        public PlcCommunicationController(IPlcDataExchangeService communicationService,IPlcCommunicationService dbService, PlcDbContext dbContext)
        {
            _dataExchangeService = communicationService;
            _communicationService = dbService;
            _dbContext = dbContext;
        }

        [HttpGet("{plcId}/{byteAddress}/{bitAddress}")]
        public ActionResult getSingleOutputState([FromRoute] int plcId, [FromRoute] int byteAddress, [FromRoute] int bitAddress)
        {
            var plc = _communicationService.GetPlc(plcId);
            return Ok(_dataExchangeService.GetSingleOutput(plc,byteAddress, bitAddress));
        }
         
        [HttpPost("{plcId}")]
        public ActionResult ConnectToPlc(int plcId)
        {
            _communicationService.StartCommunication(plcId);
            return Ok("Communication Started");
        }

        [HttpPost("{ip}/{model}/{id}")]
        public ActionResult CreatePlc(string ip,int model, int id)
        {
            var plcModel = _dbContext.Models.FirstOrDefault(n => n.CpuModel == model);
            _communicationService.AddPlc(id, ip, plcModel);
            return Ok("Plc Created.");
        }
    }
}
