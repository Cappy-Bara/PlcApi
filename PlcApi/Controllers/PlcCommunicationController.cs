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
using PlcApi.Services.EntityServices;
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
        private readonly ISensorService _sensorService;
        private readonly IInputOutputService _ioService;
        private readonly IPlcDataReadingService _readingService;
        private readonly IConveyorService _conveyorService;
        //private readonly IDiodeService _diodeService;
        private readonly IPalletService _palletService;

        public PlcCommunicationController(IDatabaseService communicationService, IPlcStorageService dbService, PlcDbContext dbContext,
            IPlcDataReadingService readingService, IInputOutputService ioService, IConveyorService conveyorService,
            ISensorService sensorService, IPalletService palletService)

        {
            _dataExchangeService = communicationService;
            _communicationService = dbService;
            _ioService = ioService;
            _readingService = readingService;
            _conveyorService = conveyorService;
            //_diodeService = diodeService;
            _dbContext = dbContext;
            _sensorService = sensorService;
            _palletService = palletService;
    }

        [HttpGet("{plcId}/{byteAddress}/{bitAddress}")]
        public ActionResult GetSingleOutputState([FromRoute] int plcId, [FromRoute] int byteAddress, [FromRoute] int bitAddress)
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

        /*
        [HttpPost("{plcId}/Diode")]
        public ActionResult CreateDiode([FromRoute] int plcId, [FromBody] CreateDiodeDto dto)
        {
            _diodeService.AddDiodeToDb(plcId, dto);
            return Ok("Diode created");
        }

        */


        [HttpPut("{plcId}")]
        public ActionResult RefreshIOStatus([FromRoute]int plcId)
        {
            _ioService.RefreshInputsAndOutputs(plcId);
            return Ok("Refreshed.");
        }

        /*
        [HttpPut("{plcId}/Diode")]
        public ActionResult RefreshDiodesState([FromRoute] int plcId)
        {
            _diodeService.RefreshDiodesStatus(plcId);
            return Ok("Diode Status Refreshed");
        }

        [HttpGet("{plcId}/Diode")]
        public ActionResult<List<Diode>> GetDiodesStatus([FromRoute] int plcId)
        {
            return Ok(_diodeService.ReturnPlcDiodes(plcId));
        }

        */

        [HttpPost("{plcId}/Conveyor")]
        public ActionResult CreateConveyor([FromRoute] int plcId, [FromBody] ConveyorDto dto)
        {
            _conveyorService.AddConveyorToDb(plcId, dto);
            return Ok();
        }

        [HttpPut("{plcId}/Conveyor/{boardId}")]
        public ActionResult RefreshConveyorState([FromRoute] int plcId, [FromRoute] int boardId )
        {
            _conveyorService.RefreshConveyorsStatus(boardId);
            return Ok();
        }

        [HttpGet("{plcId}/Conveyor/{boardId}")]
        public ActionResult<List<ConveyorDto>> GetConveyorsOnBoard([FromRoute] int plcId, [FromRoute] int boardId )
        {
            return Ok(_conveyorService.ConveyorsOnBoard(boardId));
        }

        [HttpPost("{plcId}/Sensor")]
        public ActionResult CreateSensor([FromRoute]int plcId, [FromBody]SensorDto dto)
        {
            _sensorService.AddSensorToDb(plcId,dto);
            return Ok();
        }

        [HttpPost("Pallet")]
        public ActionResult CreatePallet(CreatePalletDto dto)
        {
            _palletService.CreatePallet(dto);
            return Ok();
        }


        [HttpPut("Pallet/{boardId}")]
        public ActionResult MovePalletsOnBoard([FromRoute]int boardId)
        {
            _palletService.MovePalletsOnBoard(boardId);
            return Ok();
        }


        [HttpGet("Pallet/{boardId}")]
        public ActionResult<List<Pallet>> GetAllPallets([FromRoute] int boardId)
        {
            return Ok(_palletService.GetAllPalletsOnBoard(boardId));
        }


        [HttpGet("timestamp/{plcId}/{boardId}")]
        public ActionResult Timestamp([FromRoute] int plcId,[FromRoute] int boardId)
        {
            _sensorService.UpdateInputsStatus(boardId);
            _ioService.RefreshInputsAndOutputs(plcId);
            _conveyorService.RefreshConveyorsStatus(boardId);
            _palletService.MovePalletsOnBoard(boardId);

            return Ok("Some time passed");
        }



    }
}
