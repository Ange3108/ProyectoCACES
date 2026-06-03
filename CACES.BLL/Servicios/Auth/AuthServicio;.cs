using System;
using System.Collections.Generic;
using System.Text;
using CACES.BLL.DTOs.Auth;
using CACES.BLL.DTOs.Usuario;
using CACES.DAL.Repositorios.Usuario;

namespace CACES.BLL.Servicios.Auth
{
    public class AuthServicio : IAuthServicio
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public AuthServicio(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public async Task<bool> LoginAsync(LoginDTO dto)
        {
            var usuario = await _usuarioRepositorio.GetUsuarioByEmailAsync(dto.CorreoElectronico);

            if (usuario == null)
                return false;

            if (!usuario.Estado)
                return false;

            var passwordHash = HashContraseña(dto.Password);

            return usuario.PasswordHash == passwordHash;
        }

        public async Task<DAL.Entidades.Usuario> AutenticarAsync(LoginDTO dto)
        {
            var usuario = await _usuarioRepositorio.GetUsuarioByEmailAsync(dto.CorreoElectronico);

            if (usuario == null || !usuario.Estado)
                return null;

            var passwordHash = HashContraseña(dto.Password);

            if (usuario.PasswordHash != passwordHash)
                return null;

            return usuario;
        }

        private string HashContraseña(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}