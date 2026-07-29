using CACES.BLL.DTOs;
using CACES.DAL.Repositorios.Base;
using CACES.BLL.Mappers;
using CACES.DAL.Entidades.SeguimientoPostOperatorio;
using CACES.BLL.DTOs.SeguimientoPostOperatorio;

namespace CACES.BLL.Servicios.ConfiguracionCheckPoints
{
    public class ConfiguracionCheckPointServicio : IConfiguracionCheckPointsServicio
    {
        private readonly IRepositorioGenerico<ConfiguracionCheckpoints> _repositorioGenerico;


        public ConfiguracionCheckPointServicio(IRepositorioGenerico<ConfiguracionCheckpoints> repositorioGenerico)
        {
            _repositorioGenerico = repositorioGenerico;
        }
        public async Task<respuestaErrores<ConfiguracionCheckPointDTO>> ActualizarConfiguracionCheckPoint(ConfiguracionCheckPointDTO configuracionCheckPoint)
        {
            var respuesta = new respuestaErrores<ConfiguracionCheckPointDTO>();
            var existenteCheckPoint = await _repositorioGenerico.ObtenerPorIdAsync(configuracionCheckPoint.IdCheckPoint);
            if (existenteCheckPoint == null)
            {
                respuesta.mensaje = "No se encontró la configuración del checkpoint.";
                respuesta.EsCorrecto = false;
                respuesta.codigo = 404;

                return respuesta;

            }
            var entidad = configuracionCheckPoint.ToConfiguracionCheckpoints();
            await _repositorioGenerico.Actualizar(entidad); 
            await _repositorioGenerico.GuardarCambiosAsync();
            respuesta.Dato = configuracionCheckPoint;
            respuesta.EsCorrecto = true;
            respuesta.mensaje = "Checkpoint actualizado correctamente.";
            respuesta.codigo = 200;
            return respuesta;

        }

        public async Task<respuestaErrores<ConfiguracionCheckPointDTO>> CrearConfiguracionCheckPoint(RegistrarConfiguracionCheckpointDTO configuracionCheckPoint)
        {
            var respuesta = new respuestaErrores<ConfiguracionCheckPointDTO>();
            if (configuracionCheckPoint.DiaCheckpoint <0) {
               
                respuesta.mensaje = "El día del checkpoint no puede ser negativo.";
                respuesta.EsCorrecto = false;
                respuesta.codigo = 400;

                return respuesta;
            }
            var entidad = configuracionCheckPoint.ToConfiguracionCheckpoints();
            await _repositorioGenerico.Crear(entidad);
            await _repositorioGenerico.GuardarCambiosAsync();
            var dto = entidad.ToConfiguracionCheckpointDTO();
           
            respuesta.Dato = dto;
            respuesta.EsCorrecto = true;
            respuesta.mensaje = "Checkpoint creado correctamente.";
            respuesta.codigo = 201;
            return respuesta;
        }

        public async Task<respuestaErrores<ConfiguracionCheckPointDTO>> DesactivarConfiguracionCheckPoint(int id)
        {
            var respuesta = new respuestaErrores<ConfiguracionCheckPointDTO>();
            var existenteCheckPoint = await _repositorioGenerico.ObtenerPorIdAsync(id);
            if (existenteCheckPoint == null)
            {
                respuesta.mensaje = "No se encontró la configuración del checkpoint.";
                respuesta.EsCorrecto = false;
                respuesta.codigo = 404;

                return respuesta;
            }
            await _repositorioGenerico.DesactivarEstado(existenteCheckPoint); 
            await _repositorioGenerico.GuardarCambiosAsync();
            respuesta.Dato = existenteCheckPoint.ToConfiguracionCheckpointDTO();
            respuesta.EsCorrecto = true;
            respuesta.mensaje = "Checkpoint desactivado correctamente.";
            respuesta.codigo = 200;
            return respuesta;
        }

        public async Task<respuestaErrores<ConfiguracionCheckPointDTO>> EliminarConfiguracionCheckPoint(int id)
        {
            var respuesta = new respuestaErrores<ConfiguracionCheckPointDTO>();
            var existenteCheckPoint = await _repositorioGenerico.ObtenerPorIdAsync(id);
            if (existenteCheckPoint == null) {
                respuesta.mensaje = "No se encontró la configuración del checkpoint.";
                respuesta.EsCorrecto = false;
                respuesta.codigo = 404;
                return respuesta;
            }
            await _repositorioGenerico.Eliminar(id);
            await _repositorioGenerico.GuardarCambiosAsync();
            respuesta.Dato = existenteCheckPoint.ToConfiguracionCheckpointDTO();
            respuesta.EsCorrecto = true;
            respuesta.mensaje = "Checkpoint eliminado correctamente.";
            respuesta.codigo = 200;
            return respuesta;
        }

        public async Task<respuestaErrores<ConfiguracionCheckPointDTO>> ObtenerConfiguracionCheckPointPorId(int id)
        {
            var respuesta = new respuestaErrores<ConfiguracionCheckPointDTO>();
            var existenteCheckPoint = await _repositorioGenerico.ObtenerPorIdAsync(id);

            if (existenteCheckPoint == null)
            {
                respuesta.mensaje = "No se encontró la configuración del checkpoint.";
                respuesta.EsCorrecto = false;
                respuesta.codigo = 404;
                return respuesta;
            }

            respuesta.EsCorrecto = true;
            respuesta.Dato = existenteCheckPoint.ToConfiguracionCheckpointDTO();
            respuesta.mensaje = "Checkpoint obtenido correctamente.";
            respuesta.codigo = 200;
            return respuesta;
        }

        public async Task<respuestaErrores<List<ConfiguracionCheckPointDTO>>> ObtenerCheckPoints()
        {
            var respuesta = new respuestaErrores<List<ConfiguracionCheckPointDTO>>();
            var checkpoints = await _repositorioGenerico.ObtenerTodosAsync();
            var checkpointsList = checkpoints.Select(c => c.ToConfiguracionCheckpointDTO()!).ToList();
            respuesta.EsCorrecto = true;
            respuesta.Dato = checkpointsList;
            respuesta.mensaje = "Checkpoints obtenidos correctamente.";
            respuesta.codigo = 200;
            return respuesta;
        }

        public async Task<respuestaErrores<List<ConfiguracionCheckPointDTO>>> ObtenerCheckPointsActivas()
        {
            var lista = await _repositorioGenerico.ObtenerActivos();
            var checkpointsList = lista.Select(c => c.ToConfiguracionCheckpointDTO()!).ToList();
            var respuesta = new respuestaErrores<List<ConfiguracionCheckPointDTO>>();
            respuesta.EsCorrecto = true;
            respuesta.Dato = checkpointsList;
            respuesta.mensaje = "Checkpoints activos obtenidos correctamente.";
            respuesta.codigo = 200;
            return respuesta;
        }
    }
}
