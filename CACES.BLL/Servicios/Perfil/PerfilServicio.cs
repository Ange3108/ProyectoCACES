using AutoMapper;
using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Perfil;
using CACES.DAL.Repositorios.Usuario;

namespace CACES.BLL.Servicios.Perfil
{
    [Serializable]
    public class PerfilServicio : IPerfilServicio
    {
        private readonly IUsuarioRepositorio _usuarioRepository;
        private readonly IMapper _mapper;

        public PerfilServicio(IUsuarioRepositorio usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        public async Task<respuestaErrores<PerfilUsuarioDTO>> GetPerfilUsuarioPorIdAsync(int id)
        {
            var respuesta = new respuestaErrores<PerfilUsuarioDTO>();

            var usuario = await _usuarioRepository.GetUsuarioByIdAsync(id);

            if (usuario == null)
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje = "Usuario no encontrado";
                respuesta.codigo = 404;
                return respuesta;
            }

            respuesta.Dato = _mapper.Map<PerfilUsuarioDTO>(usuario);
            respuesta.EsCorrecto = true;

            return respuesta;
        }

        public async Task<respuestaErrores<ActualizarPerfilDTO>> ActualizarPerfilUsuarioAsync(int id, ActualizarPerfilDTO perfilDto)
        {
            var respuesta = new respuestaErrores<ActualizarPerfilDTO>();

            try
            {
                var usuario = await _usuarioRepository.GetUsuarioByIdAsync(id);
                if (usuario == null)
                {
                    respuesta.EsCorrecto = false;
                    respuesta.mensaje = "Usuario no encontrado";
                    respuesta.codigo = 404;
                    return respuesta;
                }


                _mapper.Map(perfilDto, usuario);

                usuario.FechaDeModificacion = DateTime.Now;

                bool resultado = await _usuarioRepository.UpdateUsuarioAsync(usuario);

                if (resultado)
                {
                    respuesta.EsCorrecto = true;
                    respuesta.mensaje = "Perfil actualizado exitosamente";
                    respuesta.Dato = _mapper.Map<ActualizarPerfilDTO>(usuario); 
                }
                else
                {
                    respuesta.EsCorrecto = false;
                    respuesta.mensaje = "No se pudieron guardar los cambios en el perfil";
                    respuesta.codigo = 500;
                }
            }
            catch (Exception ex)
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje = $"Error interno al actualizar el perfil: {ex.Message}";
                respuesta.codigo = 500;
            }

            return respuesta;
        }
        public async Task<respuestaErrores<ActualizarPerfilDTO>> GetPerfilParaActualizarPorIdAsync(int id)
        {
            var respuesta = new respuestaErrores<ActualizarPerfilDTO>();
            var usuario = await _usuarioRepository.GetUsuarioByIdAsync(id);

            if (usuario == null)
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje = "Usuario no encontrado";
                respuesta.codigo = 404;
                return respuesta;
            }

            // Mapeamos directamente la entidad Usuario a ActualizarPerfilDTO
            respuesta.Dato = _mapper.Map<ActualizarPerfilDTO>(usuario);
            respuesta.EsCorrecto = true;

            return respuesta;
        }
    }
}
