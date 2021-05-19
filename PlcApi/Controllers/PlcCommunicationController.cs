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
        private readonly IPlcDataExchangeService _dataExchangeService;
        private readonly IPlcCommunicationService _communicationService;
        private readonly IElementStatusService _elementsService;
        private readonly PlcDbContext _dbContext;

        public PlcCommunicationController(IPlcDataExchangeService communicationService, IPlcCommunicationService dbService,
                                          IElementStatusService elementsService ,PlcDbContext dbContext)

        {
            _dataExchangeService = communicationService;
            _communicationService = dbService;
            _dbContext = dbContext;
            _elementsService = elementsService;
        }

        [HttpGet("{plcId}/{byteAddress}/{bitAddress}")]
        public ActionResult getSingleOutputState([FromRoute] int plcId, [FromRoute] int byteAddress, [FromRoute] int bitAddress)
        {
            var plc = _communicationService.GetPlc(plcId);
            return Ok(_dataExchangeService.GetSingleBit(plc, byteAddress, bitAddress,"Q"));
        }

        [HttpPost("communication/{plcId}")]
        public ActionResult ConnectToPlc(int plcId)
        {
            _communicationService.StartCommunication(plcId);
            return Ok("Communication Started");
        }

        [HttpPost]
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
        }

        [HttpPost("{plcId}/IO")]
        public ActionResult CreateIO([FromRoute]int plcId, [FromBody] IOCreateDto dto)
        { 
            _dataExchangeService.AddInputOutputToDb(plcId,dto);
            return Ok("I/O Created");
        }

        [HttpPost("{plcId}/Diode")]
        public ActionResult CreateDiode([FromRoute] int plcId, [FromBody] CreateDiodeDto dto)
        {
            _dataExchangeService.AddDiodeToDb(plcId, dto);
            return Ok("Diode created");
        }

        [HttpPost("{plcId}/Block")]
        public ActionResult CreateBlock([FromRoute] int plcId, [FromBody] CreateBlockDto dto)
        {
            _dataExchangeService.AddBlockToDb(plcId, dto);
            return Ok("Block created");
        }


        [HttpPut("{plcId}")]
        public ActionResult RefreshIOStatus([FromRoute]int plcId)
        {
            _dataExchangeService.RefreshInputsAndOutputs(_communicationService.GetPlc(plcId), plcId);
            return Ok("Refreshed.");
        }


        [HttpPut("refresh")]
        public ActionResult RefreshElementsState()
        {
            _elementsService.UpdateDiodesStatus();
            return Ok("Diode Status Refreshed");
        }


        [HttpGet("{plcId}/Diode")]
        public ActionResult<List<Diode>> GetDiodesStatus([FromRoute] int plcId)
        {
            return Ok(_elementsService.ReturnPlcDiodes(plcId));
        }

        [HttpGet("{plcId}/Block")]
        public ActionResult<List<Block>> GetBlockStatus([FromRoute] int plcId)
        {
            return Ok(_elementsService.ReturnPlcBlocks(plcId));
        }


    }
}
