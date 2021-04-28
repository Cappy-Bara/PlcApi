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
        private readonly IPlcCommunicationService _communicationService;

        public PlcCommunicationController(IPlcCommunicationService communicationService)
        {
            _communicationService = communicationService;
        }

       
        /*
        [HttpGet("{plcId}/{byteAddress}/{bitAddress}")]
        public ActionResult getSingleOutputState([FromRoute] int plcId, [FromRoute] int byteAddress, [FromRoute] int bitAddress)
        {
            return Ok(_communicationService.GetSingleOutput(byteAddress, bitAddress, plcId));
        }
         */
        [HttpPost("{plcId}")]
        public ActionResult ConnectToPlc(int plcId)
        {
            _communicationService.StartPlcCommunication(plcId);
            return Ok("Communication Started");
        }

       

        [HttpPost("{ip}/{model}")]
        public ActionResult CreatePlc(string ip, int model)
        {
            _communicationService.CreatePlc(ip,model);
            return Ok("Communication Started");
        }

    }


}
