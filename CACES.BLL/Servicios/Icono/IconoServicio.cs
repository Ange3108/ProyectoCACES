using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Icono;
using CACES.BLL.Mappers;
using CACES.DAL.Repositorios.Base;


namespace CACES.BLL.Servicios.Icono
{
    public class IconoServicio : IIconoServicio
    {
        
        private readonly IRepositorioGenerico<DAL.Entidades.Icono> _iconoRepositorio;

        public IconoServicio(IRepositorioGenerico<DAL.Entidades.Icono> iconoRepositorio)
        {
            _iconoRepositorio = iconoRepositorio;
        }

        public async Task<respuestaErrores<IconoDTO>> ActualizarIconoAsync(int id, IconoDTO iconoDTO)
        {
            var respuesta = new respuestaErrores<IconoDTO>();
            var iconoExistente = await _iconoRepositorio.ObtenerPorIdAsync(id);
            if (iconoExistente == null)
            {
                respuesta.mensaje = "Icono no encontrado.";
                respuesta.EsCorrecto = false;
                respuesta.codigo = 404;
                return respuesta;
            }

            iconoExistente.Codigo = iconoDTO.Codigo;
            iconoExistente.Nombre = iconoDTO.Nombre;

            await _iconoRepositorio.Actualizar(iconoExistente);
            var guardado = await _iconoRepositorio.GuardarCambiosAsync();

            respuesta.EsCorrecto = guardado;
            respuesta.mensaje = guardado ? "Icono actualizado correctamente." : "No se pudo actualizar el icono.";
            respuesta.codigo = guardado ? 200 : 400;
            respuesta.Dato = iconoExistente.ToIconoDTO();
            return respuesta;
        }

        public async Task<respuestaErrores<IconoDTO>> CrearIconoAsync(IconoDTO iconoDTO)
        {
            var respuesta = new respuestaErrores<IconoDTO>();
            var nuevoIcono = new DAL.Entidades.Icono
            {
                Codigo = iconoDTO.Codigo,
                Nombre = iconoDTO.Nombre,
            };

            await _iconoRepositorio.Crear(nuevoIcono);
            await _iconoRepositorio.GuardarCambiosAsync();

            respuesta.mensaje = "Icono creado correctamente.";
            respuesta.EsCorrecto = true;
            respuesta.codigo = 200;
            respuesta.Dato = nuevoIcono.ToIconoDTO();
            return respuesta;
        }

        public async Task<respuestaErrores<bool>> EliminarIconoAsync(int id)
        {
            var respuesta = new respuestaErrores<bool>();
            var iconoExistente = await _iconoRepositorio.ObtenerPorIdAsync(id);
            if (iconoExistente == null)
            {
                respuesta.mensaje = "Icono no encontrado.";
                respuesta.EsCorrecto = false;
                respuesta.codigo = 404;
                return respuesta;
            }

            await _iconoRepositorio.Eliminar(id);
            var guardado = await _iconoRepositorio.GuardarCambiosAsync();

            respuesta.EsCorrecto = guardado;
            respuesta.Dato = guardado;
            respuesta.mensaje = guardado ? "Icono eliminado correctamente." : "No se pudo eliminar el icono.";
            respuesta.codigo = guardado ? 200 : 400;
            return respuesta;
        }

        public async Task<respuestaErrores<List<IconoDTO>>> GetListadoIconosAsync()
        {
            var respuesta = new respuestaErrores<List<IconoDTO>>();
            var iconos = await _iconoRepositorio.ObtenerTodosAsync();

            respuesta.Dato = iconos.Select(i => i.ToIconoDTO()!).ToList();
            respuesta.mensaje = "Listado de iconos obtenido correctamente.";
            respuesta.EsCorrecto = true;
            respuesta.codigo = 200;
            return respuesta;
        }
    }
}

