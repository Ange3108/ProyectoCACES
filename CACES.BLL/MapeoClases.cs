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
          CreateMap<Usuario, RegistrarUsuarioDTO>()
            .ForMember(dest => dest.passwordHash, opt => opt.Ignore()) // No mapear password
            .ReverseMap()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        }

    }
}
