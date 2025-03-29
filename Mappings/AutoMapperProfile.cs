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
            //EVENTO
            CreateMap<Evento, EventoResponseDto>()
                .ForMember(dest => dest.NombreTipo, opt => opt.MapFrom(src => src.Tipo.Nombre));

            CreateMap<Evento, EventoDetalleResponseDto>()
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado.ToString()))
                .ForMember(dest => dest.Destacado, opt => opt.MapFrom(src => src.Destacado.ToString()))
                .ForMember(dest => dest.NombreTipo, opt => opt.MapFrom(src => src.Tipo.Nombre));


            CreateMap<EventoRequestDto, Evento>();

        }
    }
}