using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Perfil;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.Servicios.Perfil
{
    public interface IPerfilServicio
    {
        Task<respuestaErrores<PerfilUsuarioDTO>> GetPerfilUsuarioPorIdAsync(int id);
        Task<respuestaErrores<ActualizarPerfilDTO>> ActualizarPerfilUsuarioAsync(int id, ActualizarPerfilDTO perfilDto);
    }
}
