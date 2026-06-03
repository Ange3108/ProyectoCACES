using System;
using System.Collections.Generic;
using System.Text;
using CACES.BLL.DTOs.Auth;
using CACES.BLL.DTOs.Usuario;

namespace CACES.BLL.Servicios.Auth
{
    public interface IAuthServicio
    {
        Task<bool> LoginAsync(LoginDTO dto);
        Task<DAL.Entidades.Usuario> AutenticarAsync(LoginDTO dto);

        Task<(bool Exito, string Token, string Mensaje)> GenerarTokenRecuperacionAsync(OlvidoContrasenaDTO dto);
        Task<(bool Exito, string Mensaje)> RestablecerContraseñaAsync(RestablecerContrasenaDTO dto);
    }
}