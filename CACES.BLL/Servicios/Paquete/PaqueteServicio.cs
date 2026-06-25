using AutoMapper;
using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Paquete;
using CACES.DAL.Repositorios.Paquetes;

namespace CACES.BLL.Servicios.Paquete
{
    public class PaqueteServicio : IPaqueteServicio
    {
        private readonly IPaqueteRepositorio _paqueteRepositorio;
        private readonly IMapper _mapper;

        public PaqueteServicio(IPaqueteRepositorio paqueteRepositorio, IMapper mapper)
        {
            _paqueteRepositorio = paqueteRepositorio;
            _mapper = mapper;
        }

        public async Task<respuestaErrores<PaqueteDTO>> CreatePaqueteAsync(PaqueteDTO registrarPaqueteDto)
        {
            var respuesta = new respuestaErrores<PaqueteDTO>();

            try
            {
                var paquete = _mapper.Map<DAL.Entidades.Paquete>(registrarPaqueteDto);
                paquete.FechaDeRegistro = DateTime.Now;
                paquete.Estado = true;

                bool resultado = await _paqueteRepositorio.CreatePaqueteAsync(paquete);

                if (resultado)
                {
                    respuesta.EsCorrecto = true;
                    respuesta.mensaje = "Paquete registrado exitosamente.";
                    respuesta.Dato = _mapper.Map<PaqueteDTO>(paquete);
                }
                else
                {
                    respuesta.EsCorrecto = false;
                    respuesta.mensaje = "No se pudo registrar el paquete en el sistema.";
                    respuesta.codigo = 500;
                }
            }
            catch (Exception ex)
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje = $"Error al crear el paquete: {ex.Message}";
                respuesta.codigo = 500;
            }

            return respuesta;
        }


        public async Task<List<PaqueteDTO>> GetPaquetesAsync()
        {
            var entidades = await _paqueteRepositorio.GetPaquetesAsync();
            return _mapper.Map<List<PaqueteDTO>>(entidades);
        }

        public async Task<List<PaqueteDTO>> GetPaquetesSoloActivosAsync()
        {
            var entidades = await _paqueteRepositorio.GetPaquetesSoloActivosAsync();
            return _mapper.Map<List<PaqueteDTO>>(entidades);
        }

        public async Task<respuestaErrores<PaqueteDTO>> UpdatePaqueteAsync(int id, PaqueteDTO registrarPaqueteDTO)
        {
            try
            {
                var paquete = await _paqueteRepositorio.GetPaqueteByIdAsync(id);

                if (paquete == null)
                {
                    return new respuestaErrores<PaqueteDTO>
                    {
                        EsCorrecto = false,
                        mensaje = "Paquete no encontrado",
                        codigo = 404
                    };
                }

                _mapper.Map(registrarPaqueteDTO, paquete);

                bool resultado = await _paqueteRepositorio.UpdatePaqueteAsync(paquete);

                if (resultado)
                {
                    return new respuestaErrores<PaqueteDTO>
                    {
                        EsCorrecto = true,
                        mensaje = "Paquete actualizado exitosamente",
                        Dato = _mapper.Map<PaqueteDTO>(paquete)
                    };
                }

                return new respuestaErrores<PaqueteDTO>
                {
                    EsCorrecto = false,
                    mensaje = "Error al actualizar el paquete"
                };
            }
            catch (Exception ex)
            {
                return new respuestaErrores<PaqueteDTO>
                {
                    EsCorrecto = false,
                    mensaje = ex.Message
                };
            }
        }

        public async Task<respuestaErrores<PaqueteDTO>> GetPaquetePorIdAsync(int id)
        {
            var respuesta = new respuestaErrores<PaqueteDTO>();
            var paquete = await _paqueteRepositorio.GetPaqueteByIdAsync(id);

            if (paquete == null)
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje = "Paquete no encontrado";
                respuesta.codigo = 404;
                return respuesta;
            }

            respuesta.EsCorrecto = true;
            respuesta.Dato = _mapper.Map<PaqueteDTO>(paquete);
            return respuesta;
        }
    }
}