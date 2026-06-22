using AutoMapper;
using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Especialidad;
using CACES.BLL.DTOs.Usuario;
using CACES.BLL.Servicios.ConfirmacionCorreo;
using CACES.BLL.Servicios.Especialidad;
using CACES.DAL.Entidades;
using CACES.DAL.Repositorios.Especialidades;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.Servicios.Especialidad
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;

    namespace ProyectoCACES.CACES.BLL.Servicios
    {
        [Serializable]
        public class EspecialidadServicio : IEspecialidadServicio
        {
            // Reglas de negocio para la especialidad
            // - Validar que el nombre no esté ya registrado

            private readonly IEspecialidadRepositorio _especialidadRepositorio;
            private readonly IMapper _mapper;

            public EspecialidadServicio(IEspecialidadRepositorio especialidadRepositorio, IMapper mapper)
            {
                _especialidadRepositorio = especialidadRepositorio;
                _mapper = mapper;
            }

            public async Task<respuestaErrores<mostrarEspecialidadDTO>> CrearEspecialidadAsync(especialidadDTO especialidadDto)
            {
                try
                {
                    // Validaciones de negocio: Verificar si el nombre ya existe
                    var especialidadExistente = await _especialidadRepositorio.GetEspecialidadByNameAsync(especialidadDto.Nombre);
                    if (especialidadExistente != null)
                    {
                        return new respuestaErrores<mostrarEspecialidadDTO>
                        {
                            EsCorrecto = false,
                            mensaje = "El nombre ya está registrado"
                        };
                    }

                    // Mapear el DTO a la entidad de base de datos
                    var nuevoEspecialidad = _mapper.Map<DAL.Entidades.Especialidad>(especialidadDto);
                    nuevoEspecialidad.FechaDeRegistro = DateTime.Now;
                    nuevoEspecialidad.Estado = true;

                    // Guardar en la base de datos
                    bool resultado = await _especialidadRepositorio.CreateEspecialidadAsync(nuevoEspecialidad);

                    if (resultado)
                    {
                        return new respuestaErrores<mostrarEspecialidadDTO>
                        {
                            EsCorrecto = true,
                            mensaje = "Especialidad registrada exitosamente",
                            Dato = _mapper.Map<mostrarEspecialidadDTO>(nuevoEspecialidad)
                        };
                    }

                    return new respuestaErrores<mostrarEspecialidadDTO>
                    {
                        EsCorrecto = false,
                        mensaje = "Error al registrar"
                    };
                }
                catch (Exception ex)
                {
                    return new respuestaErrores<mostrarEspecialidadDTO>
                    {
                        EsCorrecto = false,
                        mensaje = ex.Message
                    };
                }
            }

            public async Task<respuestaErrores<mostrarEspecialidadDTO>> ActualizarEspecialidadAsync(int id, especialidadDTO especialidadDto)
            {
                try
                {
                    var respuesta = await _especialidadRepositorio.GetEspecialidadByIdAsync(id);

                    if (respuesta == null)
                    {
                        return new respuestaErrores<mostrarEspecialidadDTO>
                        {
                            EsCorrecto = false,
                            mensaje = "Especialidad no encontrada",
                            codigo = 404
                        };
                    }


                    // Actualizar únicamente los campos editables
                    respuesta.Nombre = especialidadDto.Nombre;
                    respuesta.Descripcion = especialidadDto.Descripcion;
                    respuesta.Icono = especialidadDto.Icono;
                    respuesta.Estado = especialidadDto.Estado;

                    bool resultado =
                        await _especialidadRepositorio.UpdateEspecialidadAsync(respuesta);

                    if (resultado)
                    {
                        return new respuestaErrores<mostrarEspecialidadDTO>
                        {
                            EsCorrecto = true,
                            mensaje = "Especialidad actualizada exitosamente",
                            Dato = _mapper.Map<mostrarEspecialidadDTO>(respuesta)
                        };
                    }

                    return new respuestaErrores<mostrarEspecialidadDTO>
                    {
                        EsCorrecto = false,
                        mensaje = "Error al actualizar la especialidad"
                    };
                }
                catch (Exception ex)
                {
                    return new respuestaErrores<mostrarEspecialidadDTO>
                    {
                        EsCorrecto = false,
                        mensaje = ex.Message
                    };
                }
            }

            public async Task<respuestaErrores<mostrarEspecialidadDTO>> GetEspecialidadPorNombreAsync(string nombre)
            {
                var respuesta = new respuestaErrores<mostrarEspecialidadDTO>();
                var especialidad = await _especialidadRepositorio.GetEspecialidadByNameAsync(nombre);

                if (especialidad == null)
                {
                    respuesta.EsCorrecto = false;
                    respuesta.mensaje = "Especialidad no encontrada";
                    respuesta.codigo = 404;
                    return respuesta;
                }

                respuesta.EsCorrecto = true;
                respuesta.Dato = _mapper.Map<mostrarEspecialidadDTO>(especialidad);
                return respuesta;
            }

            public async Task<respuestaErrores<mostrarEspecialidadDTO>> GetEspecialidadPorIdAsync(int id)
            {
                var respuesta = new respuestaErrores<mostrarEspecialidadDTO>();
                var especialidad = await _especialidadRepositorio.GetEspecialidadByIdAsync(id);
                if (especialidad == null)
                {
                    respuesta.EsCorrecto = false;
                    respuesta.mensaje = "Especialidad no encontrada";
                    respuesta.codigo = 404;
                    return respuesta;
                }

                respuesta.EsCorrecto = true;
                respuesta.Dato = _mapper.Map<mostrarEspecialidadDTO>(especialidad);
                return respuesta;
            }

            public async Task<respuestaErrores<List<mostrarEspecialidadDTO>>> GetEspecialidadesAsync()
            {
                var respuesta = new respuestaErrores<List<mostrarEspecialidadDTO>>();
                var listaEspecialidades = await _especialidadRepositorio.GetEspecialidadesAsync();

                respuesta.EsCorrecto = true;
                respuesta.Dato = _mapper.Map<List<mostrarEspecialidadDTO>>(listaEspecialidades);
                return respuesta;
            }

            public async Task<respuestaErrores<mostrarEspecialidadDTO>> DesactivarEspecialidadAsync(int id)
            {
                var respuesta = new respuestaErrores<mostrarEspecialidadDTO>();
                var resultado = await _especialidadRepositorio.DesactivarEspecialidadAsync(id);

                if (resultado)
                {
                    respuesta.EsCorrecto = true;
                    respuesta.mensaje = "Especialidad desactivada exitosamente";
                    respuesta.codigo = 200;
                    return respuesta;
                }

                respuesta.EsCorrecto = false;
                respuesta.mensaje = "Error al desactivar";
                return respuesta;
            }

            public async Task<respuestaErrores<List<especialidadDTO>>> GetListadoEspecialidadesAsync()
            {
                var respuesta = new respuestaErrores<List<especialidadDTO>>();
                var listaEspecialidades = await _especialidadRepositorio.GetEspecialidadesAsync();

                respuesta.EsCorrecto = true;
                respuesta.Dato = _mapper.Map<List<especialidadDTO>>(listaEspecialidades);
                return respuesta;
            }

            public async Task<respuestaErrores<List<mostrarEspecialidadDTO>>> GetEspecialidadesActivasAsync()
            {
                var respuesta = new respuestaErrores<List<mostrarEspecialidadDTO>>();

                var especialidades =
                    await _especialidadRepositorio.GetEspecialidadesActivasAsync();

                respuesta.EsCorrecto = true;
                respuesta.mensaje = "Especialidades activas obtenidas exitosamente";
                respuesta.codigo = 200;
                respuesta.Dato = _mapper.Map<List<mostrarEspecialidadDTO>>(especialidades);

                return respuesta;
            }

            public async Task<respuestaErrores<mostrarDetalleEspecialidadDTO>> GetDetalleEspecialidadAsync(int id)
            {
                var respuesta = new respuestaErrores<mostrarDetalleEspecialidadDTO>();
                var especialidad = await _especialidadRepositorio.GetEspecialidadDetallesByIdAsync(id);

                if (especialidad == null)
                {
                    respuesta.EsCorrecto = false;
                    respuesta.mensaje = "Especialidad no encontrada"; 
                     respuesta.codigo = 404;
                    return respuesta;
                }
                respuesta.EsCorrecto = true;
                respuesta.Dato = _mapper.Map<mostrarDetalleEspecialidadDTO>(especialidad);
                return respuesta;
            }
        }
    }
}


