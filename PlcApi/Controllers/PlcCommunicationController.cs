using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlcApi.Services;
using PlcApi.Services.Interfaces;

namespace PlcApi.Controllers
{
    [ApiController]
    [Route("plc")]
    public class PlcCommunicationController : ControllerBase
    {
        private readonly IPlcDataExchangeService _dataExchangeService;
        private readonly IPlcCommunicationService _communicationService;

        public PlcCommunicationController(IPlcDataExchangeService communicationService,IPlcCommunicationService dbService)
        {
            _dataExchangeService = communicationService;
            _communicationService = dbService;
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

        [HttpPost("{ip}/{model}")]
        public ActionResult CreatePlc(string ip, int id)
        {
            _communicationService.AddPlc(id, ip);
            return Ok("Plc Created.");
        }
    }
}
