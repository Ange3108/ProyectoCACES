using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Receta;
using CACES.BLL.Mappers;
using CACES.DAL.Entidades;
using CACES.DAL.Repositorios.Recetas;

namespace CACES.BLL.Servicios.Recetas
{
    public class RecetaServicio : IRecetaServicio
    {
        private readonly IRecetaRepositorio _repositorio;


        public RecetaServicio(IRecetaRepositorio repositorio
                             )
        {
            _repositorio = repositorio;

        }

        public async Task<respuestaErrores<MostrarRecetaDTO>> RegistrarAsync(RegistrarRecetaDTO dto)
        {
            var respuesta = new respuestaErrores<MostrarRecetaDTO>();

            try
            {
                if (await _repositorio.ExistePorCitaAsync(dto.IdCita))
                {
                    respuesta.EsCorrecto = false;
                    respuesta.mensaje = "Esta cita ya posee una receta.";
                    return respuesta;
                }

                var receta = new Receta
                {
                    IdCita = dto.IdCita,
                    Medicamentos = dto.Medicamentos,
                    Instrucciones = dto.Instrucciones,
                    FechaDeRegistro = DateTime.Now,
                    FechaDeVencimiento = dto.FechaDeVencimiento
                };

                await _repositorio.RegistrarAsync(receta);

                var recetaCompleta = await _repositorio.ObtenerPorIdAsync(receta.IdReceta);

                respuesta.EsCorrecto = true;
                respuesta.mensaje = "Receta registrada correctamente.";
                respuesta.Dato = recetaCompleta.ToMostrarRecetaDTO();

                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje = ex.Message;
                return respuesta;
            }
        }

        public async Task<respuestaErrores<MostrarRecetaDTO>> ObtenerPorIdAsync(int idReceta)
        {
            var respuesta = new respuestaErrores<MostrarRecetaDTO>();

            var receta = await _repositorio.ObtenerPorIdAsync(idReceta);

            if (receta == null)
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje = "No existe la receta.";
                return respuesta;
            }

            respuesta.EsCorrecto = true;
            respuesta.Dato = receta.ToMostrarRecetaDTO();

            return respuesta;
        }

        public async Task<respuestaErrores<MostrarRecetaDTO>> ObtenerPorCitaAsync(int idCita)
        {
            var respuesta = new respuestaErrores<MostrarRecetaDTO>();

            var receta = await _repositorio.ObtenerPorCitaAsync(idCita);

            if (receta == null)
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje = "No existe receta para esta cita.";
                return respuesta;
            }

            respuesta.EsCorrecto = true;
            respuesta.Dato = receta.ToMostrarRecetaDTO();

            return respuesta;
        }

        public async Task<respuestaErrores<List<MostrarRecetaDTO>>> ObtenerPorPacienteAsync(int idPaciente)
        {
            var respuesta = new respuestaErrores<List<MostrarRecetaDTO>>();

            var lista = await _repositorio.ObtenerPorPacienteAsync(idPaciente);

            respuesta.EsCorrecto = true;
            respuesta.Dato = lista.Select(l => l.ToMostrarRecetaDTO()).ToList();

            return respuesta;
        }

        public async Task<respuestaErrores<List<MostrarRecetaDTO>>> ObtenerPorMedicoAsync(int idMedico)
        {
            var respuesta = new respuestaErrores<List<MostrarRecetaDTO>>();

            var lista = await _repositorio.ObtenerPorMedicoAsync(idMedico);

            respuesta.EsCorrecto = true;
            respuesta.Dato = lista.Select(l => l.ToMostrarRecetaDTO()).ToList();

            return respuesta;
        }

        public async Task<respuestaErrores<MostrarRecetaDTO>> ActualizarAsync(RegistrarRecetaDTO dto)
        {
            var respuesta = new respuestaErrores<MostrarRecetaDTO>();

            var receta = await _repositorio.ObtenerPorCitaAsync(dto.IdCita);

            if (receta == null)
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje = "No existe la receta.";
                return respuesta;
            }

            receta.Medicamentos = dto.Medicamentos;
            receta.Instrucciones = dto.Instrucciones;
            receta.FechaDeVencimiento = dto.FechaDeVencimiento;

            await _repositorio.ActualizarAsync(receta);

            respuesta.EsCorrecto = true;
            respuesta.mensaje = "Receta actualizada.";
            respuesta.Dato = receta.ToMostrarRecetaDTO();

            return respuesta;
        }
    }
}