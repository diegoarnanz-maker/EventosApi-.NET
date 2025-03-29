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
            //USUARIO
            CreateMap<RegisterRequestDto, Usuario>();
            CreateMap<Usuario, UsuarioResponseDto>();
            CreateMap<LoginRequestDto, Usuario>();
            //TIPO
            CreateMap<Tipo, TipoResponseDto>();
            CreateMap<TipoRequestDto, Tipo>();

        }
    }
}