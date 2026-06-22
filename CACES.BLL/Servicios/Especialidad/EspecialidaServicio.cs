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

        public async Task<respuestaErrores<List<mostrarEspecialidadDTO>>> GetEspecialidadesAsync()
        {
            var lista = await _especialidadRepository.GetEspecialidadesAsync();

            return new respuestaErrores<List<mostrarEspecialidadDTO>>
            {
                EsCorrecto = true,
                mensaje = "Especialidades obtenidas correctamente.",
                Dato = _mapper.Map<List<mostrarEspecialidadDTO>>(lista)
            };
        }

        public async Task<respuestaErrores<List<especialidadDTO>>> GetListadoEspecialidadesAsync()
        {
            var lista = await _especialidadRepository.GetEspecialidadesAsync();

            return new respuestaErrores<List<especialidadDTO>>
            {
                EsCorrecto = true,
                mensaje = "Listado de especialidades obtenido correctamente.",
                Dato = _mapper.Map<List<especialidadDTO>>(lista)
            };
        }

        public async Task<respuestaErrores<List<mostrarEspecialidadDTO>>> GetEspecialidadesActivasAsync()
        {
            var lista = await _especialidadRepository.GetEspecialidadesAsync();
            var activas = lista.Where(e => e.Estado).ToList();

            return new respuestaErrores<List<mostrarEspecialidadDTO>>
            {
                EsCorrecto = true,
                mensaje = "Especialidades activas obtenidas correctamente.",
                Dato = _mapper.Map<List<mostrarEspecialidadDTO>>(activas)
            };
        }

        public async Task<respuestaErrores<mostrarDetalleEspecialidadDTO>> GetDetalleEspecialidadAsync(int id)
        {
            var especialidad = await _especialidadRepository.GetEspecialidadByIdAsync(id);

            if (especialidad == null)
            {
                return new respuestaErrores<mostrarDetalleEspecialidadDTO>
                {
                    EsCorrecto = false,
                    mensaje = "Especialidad no encontrada.",
                    codigo = 404
                };
            }

            return new respuestaErrores<mostrarDetalleEspecialidadDTO>
            {
                EsCorrecto = true,
                mensaje = "Detalle de especialidad obtenido correctamente.",
                Dato = _mapper.Map<mostrarDetalleEspecialidadDTO>(especialidad)
            };
        }

        public async Task<respuestaErrores<mostrarEspecialidadDTO>> GetEspecialidadPorIdAsync(int id)
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

            return new respuestaErrores<mostrarEspecialidadDTO>
            {
                EsCorrecto = true,
                mensaje = "Especialidad encontrada.",
                Dato = _mapper.Map<mostrarEspecialidadDTO>(especialidad)
            };
        }

        public async Task<respuestaErrores<mostrarEspecialidadDTO>> GetEspecialidadPorNombreAsync(string nombre)
        {
            var especialidad = await _especialidadRepository.GetEspecialidadByNameAsync(nombre);

            if (especialidad == null)
            {
                return new respuestaErrores<mostrarEspecialidadDTO>
                {
                    EsCorrecto = false,
                    mensaje = "Especialidad no encontrada.",
                    codigo = 404
                };
            }

            return new respuestaErrores<mostrarEspecialidadDTO>
            {
                EsCorrecto = true,
                mensaje = "Especialidad encontrada.",
                Dato = _mapper.Map<mostrarEspecialidadDTO>(especialidad)
            };
        }

        public async Task<respuestaErrores<mostrarEspecialidadDTO>> CrearEspecialidadAsync(especialidadDTO especialidadDto)
        {
            try
            {
                var existente = await _especialidadRepository.GetEspecialidadByNameAsync(especialidadDto.Nombre);

                if (existente != null)
                {
                    return new respuestaErrores<mostrarEspecialidadDTO>
                    {
                        EsCorrecto = false,
                        mensaje = "El nombre ya está registrado."
                    };
                }

                var nueva = _mapper.Map<DAL.Entidades.Especialidad>(especialidadDto);

                nueva.FechaDeRegistro = DateTime.Now;
                nueva.Estado = true;

                bool resultado = await _especialidadRepository.CreateEspecialidadAsync(nueva);

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
                    Dato = _mapper.Map<mostrarEspecialidadDTO>(nueva)
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