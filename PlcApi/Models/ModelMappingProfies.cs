using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PlcApi.Entities.Elements;

namespace PlcApi.Models
{
    public class ModelMappingProfies : Profile
    {
        public ModelMappingProfies()
        {
            CreateMap<ConveyorDto, Conveyor>()
                .ForMember(m => m.StartPoint, k => k.MapFrom(s => new ConveyorPoint(s.X, s.Y, s.BoardId)));

            CreateMap<Conveyor, ConveyorDto>()
                .ForMember(m => m.X, k => k.MapFrom(s => s.StartPoint.X))
                .ForMember(m => m.Y, k => k.MapFrom(s => s.StartPoint.Y))
                .ForMember(m => m.OutputBit, k => k.MapFrom(s => s.InputOutput.Bit))
                .ForMember(m => m.OutputByte, k => k.MapFrom(s => s.InputOutput.Byte));

        }
    }
}
