using AutoMapper;
using CACES.BLL.DTOs.Usuario;
using CACES.DAL.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL
{
    public class MapeoClases : Profile
    {
        public MapeoClases()
        {
            //Mapeo de los 3 DTO de usuario
          CreateMap<Usuario, RegistrarUsuarioDTO>()
            .ForMember(dest => dest.passwordHash, opt => opt.Ignore()) // No mapear password
            .ReverseMap()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<Usuario, ActualizarUsuarioDTO>()
                .ReverseMap();
            CreateMap<Usuario, MostrarUsuarioDTO>()
                .ReverseMap();


            //Mapeo de los DTOs de paciente


            //Mapeo de los DTOs de médicos
           
        }

    }
}
