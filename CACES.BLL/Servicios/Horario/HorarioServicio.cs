
using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Especialidad;
using CACES.BLL.DTOs.Horario;
using CACES.BLL.DTOs.Medico;
using CACES.BLL.Mappers;
using CACES.BLL.Servicios.Medicos;
using CACES.DAL.Repositorios.Horarios;
using CACES.DAL.Repositorios.Medicos;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.Servicios.Horario
{
    public class HorarioServicio : IHorarioServicio
    {
        private readonly IHorariosRepositorio _horariosRepositorio;

        private readonly IMedicoServicio _medicoServicio;

        public HorarioServicio(IHorariosRepositorio horariosRepositorio, IMedicoServicio medicoServicio)
        {
            _horariosRepositorio = horariosRepositorio;

            _medicoServicio = medicoServicio;
        }


        //Validaciones de negocio generales
        //Revisar si el medico tiene un horario disponible
        //Revisar si ya eciste un horario activo para ese medico en ese dia de la semana

        public async Task<respuestaErrores<MostrarHorarioDTO>> ActualizarHorarioAsync(int id, EditarHorarioDTO horario)
        {
            var respuesta = await _horariosRepositorio.GetHorarioDisponiblePorIdAsync(id);

            if (respuesta == null)
            {
                return new respuestaErrores<MostrarHorarioDTO>
                {
                    EsCorrecto = false,
                    mensaje = "Horario no encontrado",
                    codigo = 404
                };
            }
            var horarioExistente = await _horariosRepositorio.TieneHorarioActivoAsync(horario.Id_Medico, horario.DiaSemana, id);
            if (horarioExistente)
            {
                return new respuestaErrores<MostrarHorarioDTO>
                {
                    EsCorrecto = false,
                    mensaje = "El médico ya tiene un horario activo para ese día de la semana.",
                    codigo = 400
                };
                
            }
            // Actualizar únicamente los campos editables
            respuesta.DiaSemana = horario.DiaSemana;
            respuesta.HoraInicio = horario.HoraInicio;
            respuesta.Activo = horario.Activo;


            bool resultado =
                await _horariosRepositorio.ActualizarHorarioDisponibleAsync(respuesta);

            if (resultado)
            {
                return new respuestaErrores<MostrarHorarioDTO>
                {
                    EsCorrecto = true,
                    mensaje = "Horario actualizado exitosamente",
                    Dato = respuesta.ToMostrarHorarioDTO(),
                };
            }

            return new respuestaErrores<MostrarHorarioDTO>
            {
                EsCorrecto = false,
                mensaje = "Error al actualizar el horario"
            };
        }
        

        public async Task<respuestaErrores<MostrarHorarioDTO>> CrearHorarioAsync(RegistrarHorarioDTO horario)
        {
            var respuesta = new respuestaErrores<MostrarHorarioDTO>();
            if(horario == null)
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje = "Horario Invalido";
                respuesta.codigo = 400;
                return respuesta;

            }
            var horarioExistente = await _horariosRepositorio.TieneHorarioActivoAsync(horario.Id_Medico, horario.DiaSemana);
            if (horarioExistente )
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje = "El médico ya tiene un horario activo para ese día de la semana.";
                respuesta.codigo = 400;
                return respuesta;
            }
             var nuevoHorario = horario.ToHorariosDisponibles();
             nuevoHorario.Activo = true;

            bool resultado = await _horariosRepositorio.CrearHorarioDisponibleAsync(nuevoHorario);
            if(resultado )
            {
                return new respuestaErrores<MostrarHorarioDTO>
                {
                    EsCorrecto = true,
                    mensaje = "Horario creado exitosamente.",
                    codigo = 200,
                    Dato = nuevoHorario.ToMostrarHorarioDTO()
                };
            }

            return new respuestaErrores<MostrarHorarioDTO>
            {
                EsCorrecto = false,
                mensaje = "Error al registrar"
            };


        }

        public async Task<respuestaErrores<MostrarHorarioDTO>> DesactivarHorarioAsync(int id)
        {
            var respuesta = new respuestaErrores<MostrarHorarioDTO>();
            var resultado = await _horariosRepositorio.DesactivarHorarioDisponibleAsync(id);

            if (resultado)
            {
                respuesta.EsCorrecto = true;
                respuesta.mensaje = "Horario desactivado exitosamente";
                respuesta.codigo = 200;
                return respuesta;
            }

            respuesta.EsCorrecto = false;
            respuesta.mensaje = "Error al desactivar";
            return respuesta;
        }


        public async Task<respuestaErrores<List<MostrarHorarioDTO>>> ObtenerHorarioPorMedicoIdAsync(int idMedico)
        {
            var respuesta = new respuestaErrores<List<MostrarHorarioDTO>>();
            var medico = await _horariosRepositorio.GetHorariosDisponiblesPorMedicoAsync(idMedico);
            if(medico == null || !medico.Any())
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje = "No se encontraron horarios para el médico especificado.";
                respuesta.codigo = 404;
                return respuesta;
            }

            respuesta.EsCorrecto = true;
            respuesta.mensaje = "Horarios obtenidos exitosamente.";
            respuesta.Dato = medico.Select(h => h.ToMostrarHorarioDTO()).ToList();
            return respuesta;
        }
    }

        
    }
