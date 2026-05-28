using AutoMapper;
using CACES.BLL.DTOs;
using CACES.DAL.Entidades;
using CACES.DAL.Repositorios.Usuario;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.Servicios.Usuario
{
    //Reglas de negocio para el usuario
    //-Validar que el correo electrónico no esté ya registrado
    //-Validar que el DUI no esté ya registrado
    //-Validar que la contraseña sea de al menos 8 caracteres, contenga al menos un número y un carácter especial


    [Serializable]
    public class UsuarioServicio : IUsuarioService
    {
        private readonly IUsuarioRepositorio _usuarioRepository;
        private readonly IMapper _mapper;

        public UsuarioServicio(IUsuarioRepositorio usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        public (bool IsValid, string Message) ValidarContraseña(string password)
        {
            if (string.IsNullOrEmpty(password))
                return (false, "La contraseña es requerida");

            if (password.Length < 8)
                return (false, "La contraseña debe tener mínimo 8 caracteres");

            if (!password.Any(char.IsDigit))
                return (false, "La contraseña debe contener al menos un número");

            if (!password.Any(c => !char.IsLetterOrDigit(c)))
                return (false, "La contraseña debe contener al menos un carácter especial");

            return (true, "Contraseña válida");
        }

        public async Task<respuestaErrores<UsuarioDTO>> CrearUsuarioAsync(UsuarioDTO usuarioDto)
        {
            try
            {
                var validacion = ValidarContraseña(usuarioDto.passwordHash);
                if (!validacion.IsValid)
                    return new respuestaErrores<UsuarioDTO> { EsCorrecto = false, mensaje = validacion.Message };

                var usuarioDui = await _usuarioRepository.GetUsuarioByDUIAsync(usuarioDto.DUI);
                if (usuarioDui != null)
                    return new respuestaErrores<UsuarioDTO> { EsCorrecto = false, mensaje = "El DUI ya está registrado" };

                var usuario = _mapper.Map<DAL.Entidades.Usuario>(usuarioDto);
                usuario.PasswordHash = HashContraseña(usuarioDto.passwordHash);
                usuario.FechaDeRegistro = DateTime.Now;

                bool resultado = await _usuarioRepository.CreateUsuarioAsync(usuario);

                if (resultado)
                    return new respuestaErrores<UsuarioDTO> { EsCorrecto = true, mensaje = "Usuario registrado exitosamente", Dato = usuarioDto };
                else
                    return new respuestaErrores<UsuarioDTO> { EsCorrecto = false, mensaje = "Error al registrar el usuario" };
            }
            catch (Exception ex)
            {
                return new respuestaErrores<UsuarioDTO> { EsCorrecto = false, mensaje = ex.Message };
            }
        }

        private string HashContraseña(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public async Task<respuestaErrores<UsuarioDTO>> ActualizarUsuarioAsync(int id, UsuarioDTO usuarioDto)
        {
            throw new NotImplementedException();
        }

        public async Task<respuestaErrores<bool>> EliminarUsuarioAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<respuestaErrores<UsuarioDTO>> ObtenerUsuarioPorDUIAsync(string dui)
        {
            throw new NotImplementedException();
        }

        public async Task<respuestaErrores<UsuarioDTO>> ObtenerUsuarioPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<respuestaErrores<List<UsuarioDTO>>> ObtenerUsuariosAsync()
        {
            throw new NotImplementedException();
        }
    }
}
