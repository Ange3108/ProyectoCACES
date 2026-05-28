using CACES.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.Servicios.Usuario
{
    public interface IUsuarioService
    {
        Task<respuestaErrores<List<UsuarioDTO>>> ObtenerUsuariosAsync();
        Task<respuestaErrores<UsuarioDTO>> ObtenerUsuarioPorIdAsync(int id);
        Task<respuestaErrores<UsuarioDTO>> ObtenerUsuarioPorDUIAsync(string dui);
        Task<respuestaErrores<UsuarioDTO>> CrearUsuarioAsync(UsuarioDTO usuario);
        Task<respuestaErrores<UsuarioDTO>> ActualizarUsuarioAsync(int id, UsuarioDTO usuario);
        Task<respuestaErrores<UsuarioDTO>> EliminarUsuarioAsync(int id);


    }
}
