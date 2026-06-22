using AutoMapper;
using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Especialidad;
using CACES.DAL.Repositorios.Especialidades;

namespace CACES.BLL.Servicios.Especialidad
{
    [Serializable]
    public class EspecialidadServicio : IEspecialidadServicio
    {
        private readonly IEspecialidadRepositorio _especialidadRepository;
        private readonly IMapper _mapper;

        public EspecialidadServicio(
            IEspecialidadRepositorio especialidadRepository,
            IMapper mapper)
        {
            _especialidadRepository = especialidadRepository;
            _mapper = mapper;
        }

        public async Task<respuestaErrores<List<mostrarEspecialidadDTO>>> GetEspecialidadsAsync()
        {
            var respuesta = new respuestaErrores<List<mostrarEspecialidadDTO>>();

            var listaEspecialidades = await _especialidadRepository.GetEspecialidadesAsync();

            respuesta.EsCorrecto = true;
            respuesta.mensaje = "Especialidades obtenidas correctamente.";
            respuesta.Dato = _mapper.Map<List<mostrarEspecialidadDTO>>(listaEspecialidades);

            return respuesta;
        }

        public async Task<respuestaErrores<mostrarEspecialidadDTO>> GetEspecialidadPorIdAsync(int id)
        {
            var respuesta = new respuestaErrores<mostrarEspecialidadDTO>();

            var especialidad = await _especialidadRepository.GetEspecialidadByIdAsync(id);

            if (especialidad == null)
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje = "Especialidad no encontrada.";
                respuesta.codigo = 404;
                return respuesta;
            }

            respuesta.EsCorrecto = true;
            respuesta.mensaje = "Especialidad encontrada.";
            respuesta.Dato = _mapper.Map<mostrarEspecialidadDTO>(especialidad);

            return respuesta;
        }

        public async Task<respuestaErrores<mostrarEspecialidadDTO>> GetEspecialidadPorNombreAsync(string nombre)
        {
            var respuesta = new respuestaErrores<mostrarEspecialidadDTO>();

            var especialidad = await _especialidadRepository.GetEspecialidadByNameAsync(nombre);

            if (especialidad == null)
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje = "Especialidad no encontrada.";
                respuesta.codigo = 404;
                return respuesta;
            }

            respuesta.EsCorrecto = true;
            respuesta.mensaje = "Especialidad encontrada.";
            respuesta.Dato = _mapper.Map<mostrarEspecialidadDTO>(especialidad);

            return respuesta;
        }

        public async Task<respuestaErrores<mostrarEspecialidadDTO>> CrearEspecialidadAsync(especialidadDTO especialidadDto)
        {
            try
            {
                var especialidadExistente = await _especialidadRepository
                    .GetEspecialidadByNameAsync(especialidadDto.Nombre);

                if (especialidadExistente != null)
                {
                    return new respuestaErrores<mostrarEspecialidadDTO>
                    {
                        EsCorrecto = false,
                        mensaje = "El nombre ya está registrado."
                    };
                }

                var nuevaEspecialidad = _mapper.Map<DAL.Entidades.Especialidad>(especialidadDto);

                nuevaEspecialidad.FechaDeRegistro = DateTime.Now;
                nuevaEspecialidad.Estado = true;

                bool resultado = await _especialidadRepository
                    .CreateEspecialidadAsync(nuevaEspecialidad);

                if (!resultado)
                {
                    return new respuestaErrores<mostrarEspecialidadDTO>
                    {
                        EsCorrecto = false,
                        mensaje = "Error al registrar la especialidad."
                    };
                }

                return new respuestaErrores<mostrarEspecialidadDTO>
                {
                    EsCorrecto = true,
                    mensaje = "Especialidad registrada exitosamente.",
                    Dato = _mapper.Map<mostrarEspecialidadDTO>(nuevaEspecialidad)
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
            var especialidad = await _especialidadRepository.GetEspecialidadByIdAsync(id);

            if (especialidad == null)
            {
                return new respuestaErrores<mostrarEspecialidadDTO>
                {
                    EsCorrecto = false,
                    mensaje = "Especialidad no encontrada.",
                    codigo = 404
                };
            }

            _mapper.Map(especialidadDto, especialidad);

            bool resultado = await _especialidadRepository.UpdateEspecialidadAsync(especialidad);

            if (!resultado)
            {
                return new respuestaErrores<mostrarEspecialidadDTO>
                {
                    EsCorrecto = false,
                    mensaje = "Error al actualizar la especialidad."
                };
            }

            return new respuestaErrores<mostrarEspecialidadDTO>
            {
                EsCorrecto = true,
                mensaje = "Especialidad actualizada exitosamente.",
                Dato = _mapper.Map<mostrarEspecialidadDTO>(especialidad)
            };
        }

        public async Task<respuestaErrores<mostrarEspecialidadDTO>> DesactivarEspecialidadAsync(int id)
        {
            var especialidad = await _especialidadRepository.GetEspecialidadByIdAsync(id);

            if (especialidad == null)
            {
                return new respuestaErrores<mostrarEspecialidadDTO>
                {
                    EsCorrecto = false,
                    mensaje = "Especialidad no encontrada.",
                    codigo = 404
                };
            }

            especialidad.Estado = false;

            bool resultado = await _especialidadRepository.UpdateEspecialidadAsync(especialidad);

            if (!resultado)
            {
                return new respuestaErrores<mostrarEspecialidadDTO>
                {
                    EsCorrecto = false,
                    mensaje = "No se pudo desactivar la especialidad."
                };
            }

            return new respuestaErrores<mostrarEspecialidadDTO>
            {
                EsCorrecto = true,
                mensaje = "Especialidad desactivada correctamente.",
                Dato = _mapper.Map<mostrarEspecialidadDTO>(especialidad)
            };
        }
    }
}