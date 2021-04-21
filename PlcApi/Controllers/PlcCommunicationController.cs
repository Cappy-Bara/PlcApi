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


        [HttpGet("{byteAddress}/{bitAddress}")]
        public ActionResult getSingleOutputState([FromRoute] int byteAddress, [FromRoute] int bitAddress)
        {
            return Ok(_communicationService.GetSingleOutput(byteAddress, bitAddress));
        }

        [HttpPost]
        public ActionResult ConnectToPlc()
        {
            _communicationService.StartPlcCommunication();
            return Ok("Communication Started");
        }


    }


}
