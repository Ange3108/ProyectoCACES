using CACES.BLL.DTOs;
using CACES.BLL.DTOs.SeguimientoPostOperatorio;
using CACES.BLL.Mappers;
using CACES.DAL.Entidades.SeguimientoPostOperatorio;
using CACES.DAL.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.Servicios.AlertaStaff
{
    public class AlertaStaffServicio : IAlertaStaffServicio
    {
        private readonly IRepositorioGenerico<DAL.Entidades.SeguimientoPostOperatorio.AlertaStaff> _repositorioAlertas;

        public AlertaStaffServicio(IRepositorioGenerico<DAL.Entidades.SeguimientoPostOperatorio.AlertaStaff> repositorioAlertas)
        {
            _repositorioAlertas = repositorioAlertas;
        }

        public async Task<respuestaErrores<List<AlertaStaffDTO>>> ObtenerTodas()
        {
            var respuesta = new respuestaErrores<List<AlertaStaffDTO>>();
            var lista = await _repositorioAlertas.ObtenerTodosAsync();
            var dtos = lista.Select(a => a.ToMostrarAlertaStaffDTO()!).ToList();

            respuesta.EsCorrecto = true;
            respuesta.Dato = dtos;
            respuesta.mensaje = "Alertas obtenidas correctamente";
            respuesta.codigo = 200;
            return respuesta;
        }

        public async Task<respuestaErrores<List<AlertaStaffDTO>>> ObtenerPendientes()
        {
            var respuesta = new respuestaErrores<List<AlertaStaffDTO>>();
            var lista = await _repositorioAlertas.BuscarTodos(a => a.Estado == EstadoAlerta.Pendiente);
            var dtos = lista.Select(a => a.ToMostrarAlertaStaffDTO()!).ToList();

            respuesta.EsCorrecto = true;
            respuesta.Dato = dtos;
            respuesta.mensaje = "Alertas pendientes obtenidas correctamente";
            respuesta.codigo = 200;
            return respuesta;
        }

        public async Task<respuestaErrores<int>> ContarPendientes()
        {
            var respuesta = new respuestaErrores<int>();
            var lista = await _repositorioAlertas.BuscarTodos(a => a.Estado == EstadoAlerta.Pendiente);

            respuesta.EsCorrecto = true;
            respuesta.Dato = lista.Count;
            respuesta.mensaje = "Conteo obtenido correctamente";
            respuesta.codigo = 200;
            return respuesta;
        }

        public async Task<respuestaErrores<bool>> AtenderAlerta(AtenderAlertaDTO dto)
        {
            var respuesta = new respuestaErrores<bool>();
            var alerta = await _repositorioAlertas.ObtenerPorIdAsync(dto.IdAlerta);
            if (alerta == null)
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje = "No se encontró la alerta";
                respuesta.codigo = 404;
                return respuesta;
            }

            alerta.Estado = dto.Estado;
            alerta.IdUsuarioAtendio = dto.IdUsuarioAtendio;
            alerta.Observaciones = dto.Observaciones;
            alerta.FechaAtencion = DateTime.Now;

            await _repositorioAlertas.Actualizar(alerta);
            await _repositorioAlertas.GuardarCambiosAsync();

            respuesta.EsCorrecto = true;
            respuesta.Dato = true;
            respuesta.mensaje = "Alerta actualizada correctamente";
            respuesta.codigo = 200;
            return respuesta;
        }
    }
}
