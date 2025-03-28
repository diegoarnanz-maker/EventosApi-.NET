using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EventosApi.Dtos;
using EventosApi.Models;

namespace EventosApi.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //TIPO
            CreateMap<Tipo, TipoResponseDto>();
            CreateMap<TipoRequestDto, Tipo>();

        }
    }
}