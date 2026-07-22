using CACES.BLL.DTOs;
using CACES.BLL.DTOs.CheckPoint;
using CACES.BLL.DTOs.Preguntas;
using CACES.BLL.Mappers;
using CACES.DAL.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CACES.BLL.Servicios.PreguntasPOp
{
    public class PreguntasPOpServicio : IPreguntasPOpServicio
    {
        private readonly IRepositorioGenerico<DAL.Entidades.PreguntaSeguimiento> _repositorioGenerico;

        public PreguntasPOpServicio(IRepositorioGenerico<DAL.Entidades.PreguntaSeguimiento> repositorioGenerico)
        {
            _repositorioGenerico = repositorioGenerico;
        }

        public async Task<respuestaErrores<PreguntasPOpDTO>> Actualizar(PreguntasPOpDTO preguntaSeguimiento)
        {
            var respuesta = new respuestaErrores<PreguntasPOpDTO>();
            var preguntaExistente = await _repositorioGenerico.ObtenerPorIdAsync(preguntaSeguimiento.idPregunta);
            if (preguntaExistente == null)
            {
                respuesta.mensaje = "No se encontró la pregunta de seguimiento.";
                respuesta.EsCorrecto = false;
                respuesta.codigo = 404;
                return respuesta;
            }

            var entidad = preguntaSeguimiento.ToPreguntaSeguimiento();
            await _repositorioGenerico.Actualizar(entidad!);
            await _repositorioGenerico.GuardarCambiosAsync();

            respuesta.Dato = preguntaSeguimiento;
            respuesta.EsCorrecto = true;
            respuesta.mensaje = "Pregunta actualizada correctamente.";
            respuesta.codigo = 200;
            return respuesta;
        }

        public async Task<respuestaErrores<PreguntasPOpDTO>> Crear(RegistrarPreguntasPOpDTO preguntaSeguimiento)
        {
            var respuesta = new respuestaErrores<PreguntasPOpDTO>();
            if (preguntaSeguimiento.ValorMinimo >= preguntaSeguimiento.ValorMaximo)
            {
                respuesta.mensaje = "El valor mínimo no puede ser mayor o igual al valor máximo.";
                respuesta.EsCorrecto = false;
                respuesta.codigo = 400;
                return respuesta;
            }

            if (preguntaSeguimiento.UmbralAlerta < preguntaSeguimiento.ValorMinimo || preguntaSeguimiento.UmbralAlerta > preguntaSeguimiento.ValorMaximo)
            {
                respuesta.mensaje = "El umbral de alerta debe estar entre el valor mínimo y el valor máximo.";
                respuesta.EsCorrecto = false;
                respuesta.codigo = 400;
                return respuesta;
            }

            var entidad = preguntaSeguimiento.ToPreguntaSeguimiento();
            await _repositorioGenerico.Crear(entidad!);
            await _repositorioGenerico.GuardarCambiosAsync();

            var dto = entidad!.ToPreguntaSeguimientoDTO();

            respuesta.Dato = dto;
            respuesta.EsCorrecto = true;
            respuesta.mensaje = "Pregunta creada correctamente.";
            respuesta.codigo = 201;
            return respuesta;
        }

        public async Task<respuestaErrores<PreguntasPOpDTO>> Desactivar(int id)
        {
            var respuesta = new respuestaErrores<PreguntasPOpDTO>();
            var preguntaExistente = await _repositorioGenerico.ObtenerPorIdAsync(id);
            if (preguntaExistente == null)
            {
                respuesta.mensaje = "No se encontró la pregunta de seguimiento.";
                respuesta.EsCorrecto = false;
                respuesta.codigo = 404;
                return respuesta;
            }

            await _repositorioGenerico.DesactivarEstado(preguntaExistente);
            await _repositorioGenerico.GuardarCambiosAsync();

            respuesta.Dato = preguntaExistente.ToPreguntaSeguimientoDTO();
            respuesta.EsCorrecto = true;
            respuesta.mensaje = "Pregunta desactivada correctamente.";
            respuesta.codigo = 200;
            return respuesta;
        }

        public async Task<respuestaErrores<PreguntasPOpDTO>> Eliminar(int id)
        {
            var respuesta = new respuestaErrores<PreguntasPOpDTO>();
            var preguntaExistente = await _repositorioGenerico.ObtenerPorIdAsync(id);
            if (preguntaExistente == null)
            {
                respuesta.mensaje = "No se encontró la pregunta de seguimiento.";
                respuesta.EsCorrecto = false;
                respuesta.codigo = 404;
                return respuesta;
            }

            var dto = preguntaExistente.ToPreguntaSeguimientoDTO();
            await _repositorioGenerico.Eliminar(id);
            await _repositorioGenerico.GuardarCambiosAsync();

            respuesta.Dato = dto;
            respuesta.EsCorrecto = true;
            respuesta.mensaje = "Pregunta eliminada correctamente.";
            respuesta.codigo = 200;
            return respuesta;
        }

        public async Task<respuestaErrores<PreguntasPOpDTO>> ObtenerPorId(int id)
        {
            var respuesta = new respuestaErrores<PreguntasPOpDTO>();
            var preguntaExistente = await _repositorioGenerico.ObtenerPorIdAsync(id);
            if (preguntaExistente == null)
            {
                respuesta.mensaje = "No se encontró la pregunta de seguimiento.";
                respuesta.EsCorrecto = false;
                respuesta.codigo = 404;
                return respuesta;
            }

            respuesta.EsCorrecto = true;
            respuesta.Dato = preguntaExistente.ToPreguntaSeguimientoDTO();
            respuesta.mensaje = "Pregunta obtenida correctamente.";
            respuesta.codigo = 200;
            return respuesta;
        }

        public async Task<respuestaErrores<List<PreguntasPOpDTO>>> ObtenerTodos()
        {
            var respuesta = new respuestaErrores<List<PreguntasPOpDTO>>();
            var preguntas = await _repositorioGenerico.ObtenerTodosAsync();
            var preguntasList = preguntas.Select(p => p.ToPreguntaSeguimientoDTO()!).ToList();

            respuesta.EsCorrecto = true;
            respuesta.Dato = preguntasList;
            respuesta.mensaje = "Preguntas obtenidas correctamente.";
            respuesta.codigo = 200;
            return respuesta;
        }

        public async Task<respuestaErrores<List<PreguntasPOpDTO>>> ObtenerActivas()
        {
            var respuesta = new respuestaErrores<List<PreguntasPOpDTO>>();
            var lista = await _repositorioGenerico.ObtenerActivos();
            var preguntasList = lista.Select(p => p.ToPreguntaSeguimientoDTO()!).ToList();

            respuesta.EsCorrecto = true;
            respuesta.Dato = preguntasList;
            respuesta.mensaje = "Preguntas activas obtenidas correctamente.";
            respuesta.codigo = 200;
            return respuesta;
        }
    }
}