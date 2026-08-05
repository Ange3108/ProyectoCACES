using System;
using System.Collections.Generic;
using System.Text;
using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Precio;
using CACES.DAL.Entidades;
using CACES.DAL.Repositorios.Precio;

namespace CACES.BLL.Servicios.Precio
{
    public class PrecioServicio : IPrecioServicio
    {
        private readonly IPrecioRepositorio _precioRepositorio;

        public PrecioServicio(IPrecioRepositorio precioRepositorio)
        {
            _precioRepositorio = precioRepositorio;
        }

        public async Task<respuestaErrores<List<MostrarPrecioDTO>>> ObtenerTodosAsync()
        {
            var respuesta = new respuestaErrores<List<MostrarPrecioDTO>>();

            try
            {
                var precios = await _precioRepositorio.ObtenerTodosAsync();

                respuesta.EsCorrecto = true;

                respuesta.Dato = precios.Select(p => new MostrarPrecioDTO
                {
                    IdPrecio = p.Id_Precio,
                    IdMedico = p.Id_Medico,
                    IdProcedimiento = p.Id_Procedimiento,

                    NombreMedico =
                        $"{p.Medico.Usuario.Nombres} {p.Medico.Usuario.PrimerApellido}",

                    NombreEspecialidad =
                        p.Medico.Especialidad.Nombre,

                    NombreProcedimiento =
                        p.Procedimiento.Nombre,

                    PrecioBase =
                        p.Procedimiento.PrecioBase,

                    HonorariosMedico =
                        p.Costo,

                    Detalles =
                        p.Detalles,

                    Estado =
                        p.Estado,

                    FechaDeRegistro =
                        p.FechaDeRegistro,

                    FechaDeModificacion =
                        p.FechaDeModificacion

                }).ToList();
            }
            catch (Exception ex)
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje = ex.Message;
            }

            return respuesta;
        }

        public async Task<respuestaErrores<EditarPrecioDTO>> ObtenerEditarAsync(int idPrecio)
        {
            var respuesta = new respuestaErrores<EditarPrecioDTO>();

            try
            {
                var precio = await _precioRepositorio.ObtenerPorIdAsync(idPrecio);

                if (precio == null)
                {
                    respuesta.EsCorrecto = false;
                    respuesta.mensaje = "No se encontró el precio.";

                    return respuesta;
                }

                respuesta.EsCorrecto = true;

                respuesta.Dato = new EditarPrecioDTO
                {
                    IdPrecio = precio.Id_Precio,
                    IdMedico = precio.Id_Medico,
                    HonorariosMedico = precio.Costo,
                    Detalles = precio.Detalles,
                    Estado = precio.Estado
                };
            }
            catch (Exception ex)
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje = ex.Message;
            }

            return respuesta;
        }

        public async Task<respuestaErrores<MostrarPrecioDTO>> ActualizarAsync(EditarPrecioDTO dto)
        {
            var respuesta = new respuestaErrores<MostrarPrecioDTO>();

            try
            {
                var precio = await _precioRepositorio.ObtenerPorIdAsync(dto.IdPrecio);

                if (precio == null)
                {
                    respuesta.EsCorrecto = false;
                    respuesta.mensaje = "El precio no existe.";

                    return respuesta;
                }

                precio.Id_Medico = dto.IdMedico;
                precio.Costo = dto.HonorariosMedico;
                precio.Detalles = dto.Detalles;
                precio.Estado = dto.Estado;

                var actualizado = await _precioRepositorio.ActualizarAsync(precio);

                respuesta.EsCorrecto = true;

                respuesta.Dato = new MostrarPrecioDTO
                {
                    IdPrecio = actualizado.Id_Precio,
                    IdMedico = actualizado.Id_Medico,
                    IdProcedimiento = actualizado.Id_Procedimiento,

                    NombreMedico =
                        $"{actualizado.Medico.Usuario.Nombres} {actualizado.Medico.Usuario.PrimerApellido}",

                    NombreEspecialidad =
                        actualizado.Medico.Especialidad.Nombre,

                    NombreProcedimiento =
                        actualizado.Procedimiento.Nombre,

                    PrecioBase =
                        actualizado.Procedimiento.PrecioBase,

                    HonorariosMedico =
                        actualizado.Costo,

                    Detalles =
                        actualizado.Detalles,

                    Estado =
                        actualizado.Estado,

                    FechaDeRegistro =
                        actualizado.FechaDeRegistro,

                    FechaDeModificacion =
                        actualizado.FechaDeModificacion
                };
            }
            catch (Exception ex)
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje = ex.Message;
            }

            return respuesta;
        }
    }
}