using ApiAsamblea.Models;
using ApiAsamblea.Models.Dtos;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiAsamblea.AsambleistasMapper
{
    public class AsambleistasMappers : Profile 
    {
        public AsambleistasMappers()
        {
            CreateMap<Asambleista, AsambleistaDto>().ReverseMap();
            CreateMap<Usuario, UsuarioDto>().ReverseMap();

        }
    }
}
