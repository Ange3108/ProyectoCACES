using AutoMapper;
using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Usuario;
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

        public object BCrypt { get; private set; }

        public UsuarioServicio(IUsuarioRepositorio usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        //Metodo para crear parametros para una contrasena segura
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

        public async Task<respuestaErrores<RegistrarUsuarioDTO>> CrearUsuarioAsync(RegistrarUsuarioDTO usuarioDto)
        {
            //Validaciones de negocio
            //Correo, DUI y contraseña segura
            try
            {
                var validacion = ValidarContraseña(usuarioDto.passwordHash);
                if (!validacion.IsValid)
                    return new respuestaErrores<RegistrarUsuarioDTO> { EsCorrecto = false, mensaje = validacion.Message };

                var usuarioExistente = await _usuarioRepository.GetUsuarioByEmailAsync(usuarioDto.CorreoElectronico);
                if (usuarioExistente != null)
                    return new respuestaErrores<RegistrarUsuarioDTO> { EsCorrecto = false, mensaje = "El correo ya está registrado" };

                var usuarioDui = await _usuarioRepository.GetUsuarioByDUIAsync(usuarioDto.DUI);
                if (usuarioDui != null)
                    return new respuestaErrores<RegistrarUsuarioDTO> { EsCorrecto = false, mensaje = "El DUI ya está registrado" };

                // Mapear con AutoMapper
                var nuevoUsuario = _mapper.Map<DAL.Entidades.Usuario>(usuarioDto);

                // Agregar lógica específica de negocio
                nuevoUsuario.PasswordHash = HashContraseña(usuarioDto.passwordHash);
                nuevoUsuario.FechaDeRegistro = DateTime.Now;
                nuevoUsuario.SecurityStamp = Guid.NewGuid().ToString();
                nuevoUsuario.Estado = true;

                // Guardar
                bool resultado = await _usuarioRepository.CreateUsuarioAsync(nuevoUsuario);

                if (resultado)
                {
                    var usuarioRetorno = _mapper.Map<RegistrarUsuarioDTO>(nuevoUsuario);
                    return new respuestaErrores<RegistrarUsuarioDTO>
                    {
                        EsCorrecto = true,
                        mensaje = "Usuario registrado exitosamente",
                        Dato = usuarioRetorno
                    };
                }

                return new respuestaErrores<RegistrarUsuarioDTO> { EsCorrecto = false, mensaje = "Error al registrar" };
            }
            catch (Exception ex)
            {
                return new respuestaErrores<RegistrarUsuarioDTO> { EsCorrecto = false, mensaje = ex.Message };
            }
        }
        private string HashContraseña(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public async Task<respuestaErrores<RegistrarUsuarioDTO>> ActualizarUsuarioAsync(int id, RegistrarUsuarioDTO usuarioDto)
        {
            throw new NotImplementedException();
        }

        public async Task<respuestaErrores<RegistrarUsuarioDTO>> EliminarUsuarioAsync(int id)
        {
            var respuesta = new respuestaErrores<RegistrarUsuarioDTO>();

            if (!await _usuarioRepository.DeleteUsuarioAsync(id))
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje = "No se pudo eliminar el usuario";
                respuesta.codigoError = 404;
            }

            return respuesta;
        }

        public async Task<respuestaErrores<RegistrarUsuarioDTO>> ObtenerUsuarioPorDUIAsync(string dui)
        {
            throw new NotImplementedException();
        }

        public async Task<respuestaErrores<RegistrarUsuarioDTO>> ObtenerUsuarioPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<respuestaErrores<List<RegistrarUsuarioDTO>>> ObtenerUsuariosAsync()
        {
            throw new NotImplementedException();
        }
    }
}
