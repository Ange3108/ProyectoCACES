using AutoMapper;
using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Especialidad;
using CACES.BLL.Servicios.ConfirmacionCorreo;
using CACES.BLL.Servicios.Especialidad;
using CACES.DAL.Repositorios.Especialidades;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.Servicios.Especialidad
{
    public class EspecialidaServicio
    {
        //Reglas de negocio para el especialidad
        //-Validar que el nombre no esté ya registrado



        [Serializable]
        public class EspecialidadServicio : IEspecialidadServicio
        {
            private readonly IEspecialidadRepositorio _especialidadRepository;
            private readonly IMapper _mapper;

            public object BCrypt { get; private set; }

            public EspecialidadServicio(IEspecialidadRepositorio especialidadRepository, IMapper mapper)
            {
                _especialidadRepository = especialidadRepository;
                _mapper = mapper;

            }



            public async Task<respuestaErrores<mostrarEspecialidadDTO>> CrearEspecialidadAsync(especialidadDTO especialidadDto)
            {
                //Validaciones de negocio
                //Nombre
                try
                {


                    var especialidadExistente = await _especialidadRepository.GetEspecialidadByNameAsync(especialidadDto.Nombre);
                    if (especialidadExistente != null)
                        return new respuestaErrores<mostrarEspecialidadDTO> { EsCorrecto = false, mensaje = "El nombre ya está registrado" };

                    // Mapear con AutoMapper
                    var nuevoEspecialidad = _mapper.Map<DAL.Entidades.Especialidad>(especialidadDto);
                    // Agregar lógica específica de negocio
                    nuevoEspecialidad.FechaDeRegistro = DateTime.Now;
                    nuevoEspecialidad.Estado = true;


                    // Guardar
                    bool resultado = await _especialidadRepository.CreateEspecialidadAsync(nuevoEspecialidad);



                    return new respuestaErrores<mostrarEspecialidadDTO> { EsCorrecto = false, mensaje = "Error al registrar" };
                }
                catch (Exception ex)
                {
                    return new respuestaErrores<mostrarEspecialidadDTO> { EsCorrecto = false, mensaje = ex.Message };
                }
            }


            public async Task<respuestaErrores<mostrarEspecialidadDTO>> ActualizarEspecialidadAsync(int id, especialidadDTO especialidadDto)
            {

                var especialidad = await _especialidadRepository.GetEspecialidadByIdAsync(id);
                if (especialidad == null)
                    return new respuestaErrores<mostrarEspecialidadDTO> { EsCorrecto = false, mensaje = "Especialidad no encontrado", codigo = 404 };

                _mapper.Map(especialidadDto, especialidad);

                bool resultado = await _especialidadRepository.UpdateEspecialidadAsync(especialidad);

                if (resultado)
                    return new respuestaErrores<mostrarEspecialidadDTO>
                    {
                        EsCorrecto = true,
                        mensaje = "Especialidad actualizada exitosamente",
                        Dato = _mapper.Map<mostrarEspecialidadDTO>(especialidad)
                    };

                return new respuestaErrores<mostrarEspecialidadDTO> { EsCorrecto = false, mensaje = "Error al actualizar" };

            }

            public async Task<respuestaErrores<mostrarEspecialidadDTO>> EliminarEspecialidadAsync(int id)
            {
                var respuesta = new respuestaErrores<mostrarEspecialidadDTO>();

                if (!await _especialidadRepository.DeleteEspecialidadAsync(id))
                {
                    respuesta.EsCorrecto = false;
                    respuesta.mensaje = "No se pudo eliminar el especialidad";
                    respuesta.codigo = 404;
                }


                return respuesta;
            }

            public async Task<respuestaErrores<mostrarEspecialidadDTO>> GetEspecialidadPorNombreAsync(string nombre)
            {
                var respuesta = new respuestaErrores<mostrarEspecialidadDTO?>();
                var especialidad = await _especialidadRepository.GetEspecialidadByNameAsync(nombre);
                if (especialidad == null)
                {
                    respuesta.EsCorrecto = false;
                    respuesta.mensaje = "Especialidad no encontrado";
                    respuesta.codigo = 404;
                    return respuesta;
                }
                respuesta.Dato = _mapper.Map<mostrarEspecialidadDTO>(especialidad);
                return respuesta;
            }

            public async Task<respuestaErrores<mostrarEspecialidadDTO>> GetEspecialidadPorIdAsync(int id)
            {
                var respuesta = new respuestaErrores<mostrarEspecialidadDTO?>();
                var especialidad = await _especialidadRepository.GetEspecialidadByIdAsync(id);
                if (especialidad == null)
                {
                    respuesta.EsCorrecto = false;
                    respuesta.mensaje = "Especialidad no encontrado";
                    respuesta.codigo = 404;
                    return respuesta;
                }

                respuesta.Dato = _mapper.Map<mostrarEspecialidadDTO>(especialidad);
                return respuesta;
            }

            public async Task<respuestaErrores<List<mostrarEspecialidadDTO>>> GetEspecialidadsAsync()
            {
                var respuesta = new respuestaErrores<List<mostrarEspecialidadDTO>>();
                var listaEspecialidads = await _especialidadRepository.GetEspecialidadesAsync();
                respuesta.Dato = _mapper.Map<List<mostrarEspecialidadDTO>>(listaEspecialidads);

                return respuesta;
            }

            Task<respuestaErrores<mostrarEspecialidadDTO>> IEspecialidadServicio.DesactivarEspecialidadAsync(int id)
            {
                throw new NotImplementedException();
            }
        }
    }
}
