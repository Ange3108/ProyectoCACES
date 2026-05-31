using AutoMapper;
using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Usuario;
using CACES.BLL.Servicios.ConfirmacionCorreo;
using CACES.DAL.Entidades;
using CACES.DAL.Repositorios.Usuario;
using Microsoft.Extensions.Logging;
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
        private readonly IEmailServicio _emailServicio;
        private readonly ILogger _logger;

        public object BCrypt { get; private set; }

        public UsuarioServicio(IUsuarioRepositorio usuarioRepository, IMapper mapper, IEmailServicio emailServicio, ILogger logger)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
            _emailServicio = emailServicio;
            _logger = logger;
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

        public async Task<respuestaErrores<MostrarUsuarioDTO>> CrearUsuarioAsync(RegistrarUsuarioDTO usuarioDto)
        {
            //Validaciones de negocio
            //Correo, DUI y contraseña segura
            try
            {
                var validacion = ValidarContraseña(usuarioDto.passwordHash);
                if (!validacion.IsValid)
                    return new respuestaErrores<MostrarUsuarioDTO> { EsCorrecto = false, mensaje = validacion.Message };

                var usuarioExistente = await _usuarioRepository.GetUsuarioByEmailAsync(usuarioDto.CorreoElectronico);
                if (usuarioExistente != null)
                    return new respuestaErrores<MostrarUsuarioDTO> { EsCorrecto = false, mensaje = "El correo ya está registrado" };

                var usuarioDui = await _usuarioRepository.GetUsuarioByDUIAsync(usuarioDto.DUI);
                if (usuarioDui != null)
                    return new respuestaErrores<MostrarUsuarioDTO> { EsCorrecto = false, mensaje = "El DUI ya está registrado" };

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
                    try
                    {
                        string asunto = "¡Bienvenido a nuestro sistema!";
                        string cuerpo = $"<h1>Hola {nuevoUsuario.Nombres}!</h1><p>Tu cuenta ha sido creada exitosamente con el correo {nuevoUsuario.CorreoElectronico}.</p>";

                        // Lo ejecutamos de forma asíncrona para no bloquear el flujo principal
                        await _emailServicio.EnviarCorreoAsync(nuevoUsuario.CorreoElectronico, asunto, cuerpo);
                    }
                    catch (Exception ex)
                    {
                        
                        _logger.LogError("Fallo al enviar correo de bienvenida a {Email}: {ErrorMessage}", nuevoUsuario.CorreoElectronico, ex);
                    }
                    var usuarioRetorno = _mapper.Map<MostrarUsuarioDTO>(nuevoUsuario);
                    return new respuestaErrores<MostrarUsuarioDTO>
                    {
                        EsCorrecto = true,
                        mensaje = "Usuario registrado exitosamente",
                        Dato = usuarioRetorno
                    };
                }

                return new respuestaErrores<MostrarUsuarioDTO> { EsCorrecto = false, mensaje = "Error al registrar" };
            }
            catch (Exception ex)
            {
                return new respuestaErrores<MostrarUsuarioDTO> { EsCorrecto = false, mensaje = ex.Message };
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

        public async Task<respuestaErrores<MostrarUsuarioDTO>> ActualizarUsuarioAsync(int id, ActualizarUsuarioDTO usuarioDto)
        {
           
            var usuario = await _usuarioRepository.GetUsuarioByIdAsync(id);
            if (usuario == null)
                return new respuestaErrores<MostrarUsuarioDTO> { EsCorrecto = false, mensaje = "Usuario no encontrado", codigo = 404 };
            
            _mapper.Map(usuarioDto, usuario);
            usuario.FechaDeModificacion = DateTime.Now;

            bool resultado = await _usuarioRepository.UpdateUsuarioAsync(usuario);

            if (resultado)
                return new respuestaErrores<MostrarUsuarioDTO>
                {
                    EsCorrecto = true,
                    mensaje = "Usuario actualizado exitosamente",
                    Dato = _mapper.Map<MostrarUsuarioDTO>(usuario)
                };

            return new respuestaErrores<MostrarUsuarioDTO> { EsCorrecto = false, mensaje = "Error al actualizar" };

        }

        public async Task<respuestaErrores<MostrarUsuarioDTO>> EliminarUsuarioAsync(int id)
        {
            var respuesta = new respuestaErrores<MostrarUsuarioDTO>();

            if (!await _usuarioRepository.DeleteUsuarioAsync(id))
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje = "No se pudo eliminar el usuario";
                respuesta.codigo = 404;
            }

            return respuesta;
        }

        public async Task<respuestaErrores<MostrarUsuarioDTO>> GetUsuarioPorDUIAsync(string dui)
        {
            var respuesta = new respuestaErrores<MostrarUsuarioDTO?>();
            var usuario = await _usuarioRepository.GetUsuarioByDUIAsync(dui);
            if (usuario == null)
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje = "Usuario no encontrado";
                respuesta.codigo = 404;
                return respuesta;
            }
            respuesta.Dato = _mapper.Map<MostrarUsuarioDTO>(usuario);
            return respuesta;
        }

        public async Task<respuestaErrores<MostrarUsuarioDTO>> GetUsuarioPorIdAsync(int id)
        {
            var respuesta = new respuestaErrores<MostrarUsuarioDTO?>();
            var usuario = await _usuarioRepository.GetUsuarioByIdAsync(id);
            if (usuario == null)
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje = "Usuario no encontrado";
                respuesta.codigo = 404;
                return respuesta;
            }

            respuesta.Dato = _mapper.Map<MostrarUsuarioDTO>(usuario);
            return respuesta;
        }

        public async Task<respuestaErrores<List<MostrarUsuarioDTO>>> GetUsuariosAsync()
        {
            var respuesta = new respuestaErrores<List<MostrarUsuarioDTO>>();
            var listaUsuarios = await _usuarioRepository.GetUsuariosAsync();
            respuesta.Dato = _mapper.Map<List<MostrarUsuarioDTO>>(listaUsuarios);

            return respuesta;
        }
    }
}
