using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlcApi.Entities;
using PlcApi.Entities.Elements;
using PlcApi.Exceptions;
using PlcApi.Models;
using PlcApi.Services;
using PlcApi.Services.Interfaces;
using S7.Net;

namespace PlcApi.Controllers
{
    [ApiController]
    [Route("plc")]
    public class PlcCommunicationController : ControllerBase
    {
        private readonly IDatabaseService _dataExchangeService;
        private readonly IPlcStorageService _communicationService;
        private readonly PlcDbContext _dbContext;
        private readonly IInputOutputService _ioService;
        private readonly IPlcDataReadingService _readingService;
        private readonly IConveyorService _conveyorService;
        private readonly IDiodeService _diodeService;

        public PlcCommunicationController(IDatabaseService communicationService, IPlcStorageService dbService, PlcDbContext dbContext,
            IPlcDataReadingService readingService, IInputOutputService ioService, IConveyorService conveyorService, IDiodeService diodeService)

        {
            _dataExchangeService = communicationService;
            _communicationService = dbService;
            _ioService = ioService;
            _readingService = readingService;
            _conveyorService = conveyorService;
            _diodeService = diodeService;
            _dbContext = dbContext;
        }

        [HttpGet("{plcId}/{byteAddress}/{bitAddress}")]
        public ActionResult getSingleOutputState([FromRoute] int plcId, [FromRoute] int byteAddress, [FromRoute] int bitAddress)
        {
            var plc = _communicationService.GetPlc(plcId);
            return Ok(_readingService.GetSingleBitStatus(plc, byteAddress, bitAddress,"Q"));
        }

        [HttpPost("communication/{plcId}")]
        public ActionResult ConnectToPlc(int plcId)
        {
            _communicationService.StartCommunication(plcId);
            return Ok("Communication Started");
        }

        [HttpPost]
        //TO EDYTOWAĆ
        public ActionResult CreatePlc([FromBody] PlcEntity dto)
        {
            int model = dto.ModelId;
            var plcModel = _dbContext.Models.FirstOrDefault(n => n.CpuModel == model);
            if (plcModel == null)
                throw new NotFoundException("Wrong Plc Model!");
            dto.Model = plcModel;
            int id = _dataExchangeService.AddPlcToDb(dto);
            _communicationService.AddPlc(id, dto.Ip, plcModel);
            return Ok("Plc Created.");
        }   //poprawić

        [HttpPost("{plcId}/IO")]
        public ActionResult CreateIO([FromRoute]int plcId, [FromBody] IOCreateDto dto)
        {
            _ioService.AddInputOutputToDb(plcId, dto);
            return Ok("I/O Created");
        }

        [HttpPost("{plcId}/Diode")]
        public ActionResult CreateDiode([FromRoute] int plcId, [FromBody] CreateDiodeDto dto)
        {
            _diodeService.AddDiodeToDb(plcId, dto);
            return Ok("Diode created");
        }

        [HttpPut("{plcId}")]
        public ActionResult RefreshIOStatus([FromRoute]int plcId)
        {
            _ioService.RefreshInputsAndOutputs(plcId);
            return Ok("Refreshed.");
        }


        [HttpPut("{plcId}/Diode")]
        public ActionResult RefreshElementsState([FromRoute] int plcId)
        {
            _diodeService.RefreshDiodesStatus(plcId);
            return Ok("Diode Status Refreshed");
        }


        [HttpGet("{plcId}/Diode")]
        public ActionResult<List<Diode>> GetDiodesStatus([FromRoute] int plcId)
        {
            return Ok(_diodeService.ReturnPlcDiodes(plcId));
        }

        [HttpGet("timestamp/{plcId}")]
        //to zaktualizować
        public ActionResult Timestamp([FromRoute] int plcId)
        {
            var plc = _communicationService.GetPlc(plcId);
            _ioService.RefreshInputsAndOutputs(plcId);
            _diodeService.RefreshDiodesStatus(plcId);

            return Ok("Some time passed");
        }



    }
}
