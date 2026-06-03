using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Usuario;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.Servicios.Usuario
{
    public interface IUsuarioService
    {
        Task<respuestaErrores<List<MostrarUsuarioDTO>>> GetUsuariosAsync();
        Task<respuestaErrores<MostrarUsuarioDTO>> GetUsuarioPorIdAsync(int id);
        Task<respuestaErrores<MostrarUsuarioDTO>> GetUsuarioPorDUIAsync(string dui);
        Task<respuestaErrores<MostrarUsuarioDTO>> CrearUsuarioAsync(RegistrarUsuarioDTO usuario);
        Task<respuestaErrores<MostrarUsuarioDTO>> ActualizarUsuarioAsync(int id, ActualizarUsuarioDTO usuario);
        Task<respuestaErrores<MostrarUsuarioDTO>> EliminarUsuarioAsync(int id);
        (bool IsValid, string Message) ValidarContraseña(string password);
    }
}
