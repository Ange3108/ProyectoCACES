
using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Historial;
using CACES.BLL.DTOs.Perfil;
using CACES.BLL.Mappers;
using CACES.DAL.Repositorios.Pacientes;
using CACES.DAL.Repositorios.Usuario;

namespace CACES.BLL.Servicios.Perfil
{
    [Serializable]
    public class PerfilServicio : IPerfilServicio
    {
        private readonly IUsuarioRepositorio _usuarioRepository;

        private readonly IPacienteRepositorio _pacienteRepositorio;

        public PerfilServicio(IUsuarioRepositorio usuarioRepository,  IPacienteRepositorio pacienteRepositorio)
        {
            _usuarioRepository = usuarioRepository;

            _pacienteRepositorio = pacienteRepositorio;
        }

        public async Task<respuestaErrores<PerfilUsuarioDTO>> GetPerfilUsuarioPorIdAsync(int id)
        {
            var respuesta = new respuestaErrores<PerfilUsuarioDTO>();

            var usuarioConInfoMedica = await _pacienteRepositorio.GetInfoMedicaByIdAsync(id);

            if (usuarioConInfoMedica == null)
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje = "Usuario no encontrado";
                respuesta.codigo = 404;
                return respuesta;
            }

            respuesta.Dato = usuarioConInfoMedica.ToPerfilUsuarioDTO();
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


                usuario.UpdateFromActualizarPerfilDTO(perfilDto);

                usuario.FechaDeModificacion = DateTime.Now;

                bool resultado = await _usuarioRepository.UpdateUsuarioAsync(usuario);

                if (resultado)
                {
                    respuesta.EsCorrecto = true;
                    respuesta.mensaje = "Perfil actualizado exitosamente";
                    respuesta.Dato = usuario.ToActualizarPerfilDTO(); 
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
            respuesta.Dato = usuario.ToActualizarPerfilDTO();
            respuesta.EsCorrecto = true;

            return respuesta;
        }
    }
}
