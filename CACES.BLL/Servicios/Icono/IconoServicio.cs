using AutoMapper;
using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Icono;
using CACES.BLL.Mappers;
using CACES.DAL.Repositorios.Icono;


namespace CACES.BLL.Servicios.Icono
{
    public class IconoServicio : IIconoServicio
    {

        private readonly IIconoRepositorio _iconoRepositorio;


        public IconoServicio(IIconoRepositorio iconoRepositorio)
        {
            _iconoRepositorio = iconoRepositorio;
      
        }
        public async Task<respuestaErrores<IconoDTO>> ActualizarIconoAsync(int id, IconoDTO iconoDTO)
        {
            var respuesta = new respuestaErrores<IconoDTO>();
            var iconoExistente = await _iconoRepositorio.GetPorIdAsync(id);
            if (iconoExistente == null)
            {
                respuesta.mensaje = "Icono no encontrado.";
                respuesta.EsCorrecto = false;
                respuesta.codigo = 404;
                return respuesta;
            }
            iconoExistente.Codigo = iconoDTO.Codigo;
            iconoExistente.Nombre = iconoDTO.Nombre;

            var actualizado = await _iconoRepositorio.ActualizarAsync(iconoExistente);
           respuesta.EsCorrecto = actualizado;
            respuesta.mensaje = "Icono actualizado correctamente." ;
            respuesta.codigo = 200;
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
            await _iconoRepositorio.CrearAsync(nuevoIcono);
            respuesta.mensaje = "Icono creado correctamente.";
            respuesta.EsCorrecto = true;
            respuesta.codigo = 200;
            respuesta.Dato = nuevoIcono.ToIconoDTO();

            return respuesta;
        }

        public async Task<respuestaErrores<List<IconoDTO>>> GetListadoIconosAsync()
        {
            var respuesta = new respuestaErrores<List<IconoDTO>>();

       

            var iconos = await _iconoRepositorio.GetTodosLosIconosAsync();
            respuesta.Dato = iconos.Select(i => new IconoDTO
            {
                IdIcono = i.IdIcono,
                Codigo = i.Codigo,
                Nombre = i.Nombre,

            }).ToList();

            
            respuesta.mensaje = "Listado de iconos obtenido correctamente.";
            respuesta.EsCorrecto = true;
            respuesta.codigo = 200;
            return respuesta;
        }
    }
}
