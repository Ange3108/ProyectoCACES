using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Notificacion;
using CACES.BLL.Mappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.Servicios.Notificacion
{
    
        public class NotificacionServicio : INotificacionServicio
        {
            private readonly INotificacionRepositorio _repositorio;

            public NotificacionServicio(INotificacionRepositorio repositorio)
            {
                _repositorio = repositorio;
            }

            public async Task<respuestaErrores<List<NotificacionDTO>>> ObtenerTodos()
            {
                var lista = await _repositorio.ObtenerTodosAsync();
                var dtos = lista.Select(n => n.ToDTO()).ToList();

                return new respuestaErrores<List<NotificacionDTO>>
                {
                    EsCorrecto = true,
                    mensaje = "Notificaciones obtenidas correctamente",
                    codigo = 200,
                    Dato = dtos
                };
            }

            public async Task<respuestaErrores<NotificacionDTO>> ObtenerPorId(int id)
            {
                var entidad = await _repositorio.ObtenerPorIdAsync(id);
                if (entidad == null)
                {
                    return new respuestaErrores<NotificacionDTO>
                    {
                        EsCorrecto = false,
                        mensaje = "Notificación no encontrada",
                        codigo = 404,
                        Dato = null
                    };
                }

                return new respuestaErrores<NotificacionDTO>
                {
                    EsCorrecto = true,
                    mensaje = "Notificación obtenida correctamente",
                    codigo = 200,
                    Dato = entidad.ToDTO()
                };
            }

            public async Task<respuestaErrores<NotificacionDTO>> ObtenerPorEvento(string evento)
            {
                var entidad = await _repositorio.ObtenerPorEventoAsync(evento);
                if (entidad == null)
                {
                    return new respuestaErrores<NotificacionDTO>
                    {
                        EsCorrecto = false,
                        mensaje = $"No existe configuración de notificación para el evento '{evento}'",
                        codigo = 404,
                        Dato = null
                    };
                }

                return new respuestaErrores<NotificacionDTO>
                {
                    EsCorrecto = true,
                    mensaje = "Notificación obtenida correctamente",
                    codigo = 200,
                    Dato = entidad.ToDTO()
                };
            }

            public async Task<respuestaErrores<bool>> Crear(NotificacionDTO dto)
            {
                var existente = await _repositorio.ObtenerPorEventoAsync(dto.Evento);
                if (existente != null)
                {
                    return new respuestaErrores<bool>
                    {
                        EsCorrecto = false,
                        mensaje = "Ya existe una configuración de notificación para ese evento",
                        codigo = 400,
                        Dato = false
                    };
                }

                var entidad = dto.ToEntity();
                await _repositorio.Crear(entidad);
                await _repositorio.GuardarCambiosAsync();

                return new respuestaErrores<bool>
                {
                    EsCorrecto = true,
                    mensaje = "Notificación creada correctamente",
                    codigo = 200,
                    Dato = true
                };
            }

            public async Task<respuestaErrores<bool>> Actualizar(NotificacionDTO dto)
            {
                var entidad = await _repositorio.ObtenerPorIdAsync(dto.Id_Notificacion);
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

                entidad.Evento = dto.Evento;
                entidad.CanalPlataforma = dto.CanalPlataforma;
                entidad.CanalEmail = dto.CanalEmail;

                _repositorio.Actualizar(entidad);
                await _repositorio.GuardarCambiosAsync();

                return new respuestaErrores<bool>
                {
                    EsCorrecto = true,
                    mensaje = "Notificación actualizada correctamente",
                    codigo = 200,
                    Dato = true
                };
            }

            public async Task<respuestaErrores<bool>> CambiarEstado(int id)
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

                entidad.Estado = !entidad.Estado;
                _repositorio.Actualizar(entidad);
                await _repositorio.GuardarCambiosAsync();

                return new respuestaErrores<bool>
                {
                    EsCorrecto = true,
                    mensaje = "Estado actualizado correctamente",
                    codigo = 200,
                    Dato = true
                };
            }
        }
    }

