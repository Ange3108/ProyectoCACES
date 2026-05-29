using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Usuario;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.Servicios.Usuario
{
    public interface IUsuarioService
    {
        Task<respuestaErrores<List<RegistrarUsuarioDTO>>> ObtenerUsuariosAsync();
        Task<respuestaErrores<RegistrarUsuarioDTO>> ObtenerUsuarioPorIdAsync(int id);
        Task<respuestaErrores<RegistrarUsuarioDTO>> ObtenerUsuarioPorDUIAsync(string dui);
        Task<respuestaErrores<RegistrarUsuarioDTO>> CrearUsuarioAsync(RegistrarUsuarioDTO usuario);
        Task<respuestaErrores<RegistrarUsuarioDTO>> ActualizarUsuarioAsync(int id, RegistrarUsuarioDTO usuario);
        Task<respuestaErrores<RegistrarUsuarioDTO>> EliminarUsuarioAsync(int id);


    }
}
