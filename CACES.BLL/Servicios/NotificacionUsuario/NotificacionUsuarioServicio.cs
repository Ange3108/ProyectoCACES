using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Notificacion;
using CACES.BLL.Mappers;

namespace CACES.BLL.Servicios.Notificacion
{
    public class NotificacionUsuarioServicio : INotificacionUsuarioServicio
    {
        private readonly INotificacionUsuarioRepositorio _repositorio;

        public NotificacionUsuarioServicio(INotificacionUsuarioRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<respuestaErrores<List<NotificacionUsuarioDTO>>> ObtenerPorUsuario(int idUsuario, bool soloNoLeidas = false)
        {
            var lista = await _repositorio.ObtenerPorUsuarioAsync(idUsuario, soloNoLeidas);
            var dtos = lista.Select(n => n.ToDTO()).ToList();

            return new respuestaErrores<List<NotificacionUsuarioDTO>>
            {
                EsCorrecto = true,
                mensaje = "Notificaciones obtenidas correctamente",
                codigo = 200,
                Dato = dtos
            };
        }

        public async Task<respuestaErrores<int>> ContarNoLeidas(int idUsuario)
        {
            var conteo = await _repositorio.ContarNoLeidasAsync(idUsuario);

            return new respuestaErrores<int>
            {
                EsCorrecto = true,
                mensaje = "Conteo obtenido correctamente",
                codigo = 200,
                Dato = conteo
            };
        }

        public async Task<respuestaErrores<bool>> Crear(NotificacionUsuarioDTO dto)
        {
            var entidad = dto.ToEntity();
            await _repositorio.Crear(entidad);
            await _repositorio.GuardarCambiosAsync();

            return new respuestaErrores<bool>
            {
                EsCorrecto = true,
                mensaje = "Notificación de usuario creada correctamente",
                codigo = 200,
                Dato = true
            };
        }

        public async Task<respuestaErrores<bool>> MarcarLeida(int id)
        {
            var entidad = await _repositorio.ObtenerPorIdAsync(id);
            if (entidad == null)
            {
                return new respuestaErrores<bool>
                {
                    EsCorrecto = false,
                    mensaje = "Notificación no encontrada",
                    codigo = 404,
                    Dato = false
                };
            }

            entidad.Leido = true;
            entidad.FechaLectura = DateTime.Now;

            _repositorio.Actualizar(entidad);
            await _repositorio.GuardarCambiosAsync();

            return new respuestaErrores<bool>
            {
                EsCorrecto = true,
                mensaje = "Notificación marcada como leída",
                codigo = 200,
                Dato = true
            };
        }

        public async Task<respuestaErrores<bool>> MarcarTodasLeidas(int idUsuario)
        {
            await _repositorio.MarcarTodasLeidasAsync(idUsuario);

            return new respuestaErrores<bool>
            {
                EsCorrecto = true,
                mensaje = "Notificaciones marcadas como leídas",
                codigo = 200,
                Dato = true
            };
        }
    }
}
