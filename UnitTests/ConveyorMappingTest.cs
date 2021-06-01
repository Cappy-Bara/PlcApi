using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PlcApi.Entities.Elements;
using PlcApi.Models;
using Xunit;

namespace UnitTests
{
    public class ConveyorMappingTest
    {
        private readonly IMapper _mapper;

        public ConveyorMappingTest()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ModelMappingProfies());
            });

            _mapper = config.CreateMapper();
        }


        public ConveyorDto dto = new ConveyorDto()
        {
            X = 10,
            Y = 10,
            BoardId = 0,
            OutputByte = 0,
            OutputBit = 0,
            IsVertical = false,
            IsTurnedDownOrLeft = false,
            Length = 5,
            Speed = 1,
        };

        public Conveyor _conveyor = new Conveyor()
        {
            StartPoint = new ConveyorPoint(10,10,0),
            BoardId = 0,
            IsVertical = false,
            IsTurnedDownOrLeft = false,
            Length = 5,
            Speed = 1,
        };




        [Fact]
        public void MappingShouldWork()
        {
            var conveyor = _mapper.Map<Conveyor>(dto);
            Assert.True(conveyor.StartPoint.X == 10 && conveyor.StartPoint.Y == 10);
        }
        [Fact]
        public void ShouldBeOneMainPoint()
        {
            var conveyor = _mapper.Map<Conveyor>(dto);
            Assert.True(conveyor.ReturnOccupiedPoints().Count(n => n.isMainPoint == true) == 1);
        }
        [Fact]
        public void ShouldHaveAsMuchPointsAsLength()
        {
            var conveyor = _mapper.Map<Conveyor>(dto);
            Assert.True(conveyor.ReturnOccupiedPoints().Count() == conveyor.Length);
        }

        [Fact]
        public void ShouldExistReverseMap()
        {
            var dto = _mapper.Map<ConveyorDto>(_conveyor);
            Assert.True(_conveyor.StartPoint.X == dto.X && _conveyor.StartPoint.Y == dto.Y);
        }





    }
}
