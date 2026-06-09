using AutoMapper;
using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Medico;
using CACES.DAL.Entidades;
using CACES.DAL.Repositorios.Medicos;


namespace CACES.BLL.Servicios.Medicos
{
    public class MedicoServicio : IMedicoServicio
    {
        private readonly IMedicoRepositorio _medicoRepositorio;
        private readonly IMapper _mapper;

        public MedicoServicio(IMedicoRepositorio medicoRepositorio, IMapper mapper)
        {
            _medicoRepositorio = medicoRepositorio;
            _mapper = mapper;
        }

        public async Task<List<Medico>> GetMedicosAsync()
        {
            return await _medicoRepositorio.GetMedicosAsync();
        }

        public async Task<EditarMedicoDTO?> GetMedicoParaEditarAsync(int id)
        {
            var medico = await _medicoRepositorio.GetMedicoConUsuarioByIdAsync(id);

            if (medico == null)
                return null;

            return new EditarMedicoDTO
            {
                IdMedico = medico.IdMedico,
                IdUsuario = medico.IdUsuario,

                Nombres = medico.Usuario.Nombres,
                PrimerApellido = medico.Usuario.PrimerApellido,
                SegundoApellido = medico.Usuario.SegundoApellido,
                Telefono = medico.Usuario.Telefono,
                Foto = medico.Usuario.Foto,
                Estado = medico.Usuario.Estado,

                IdEspecialidad = medico.IdEspecialidad,
                Experiencia = medico.Experiencia,
                Certificaciones = medico.Certificaciones
            };
        }

        public async Task<bool> UpdateMedicoConUsuarioAsync(EditarMedicoDTO dto)
        {
            var medico = new Medico
            {
                IdMedico = dto.IdMedico,
                IdUsuario = dto.IdUsuario,
                IdEspecialidad = dto.IdEspecialidad,
                Experiencia = dto.Experiencia,
                Telefono = dto.Telefono,
                Certificaciones = dto.Certificaciones,

                Usuario = new CACES.DAL.Entidades.Usuario
                {
                    IdUsuario = dto.IdUsuario,
                    Nombres = dto.Nombres,
                    PrimerApellido = dto.PrimerApellido,
                    SegundoApellido = dto.SegundoApellido,
                    Telefono = dto.Telefono,
                    Foto = dto.Foto,
                    Estado = dto.Estado
                }
            };

            return await _medicoRepositorio.UpdateMedicoConUsuarioAsync(medico);
        }

        public async Task<respuestaErrores<RegistrarMedicoDTO>> CreateMedicoAsync(RegistrarMedicoDTO registrarMedicoDto)
        {
            var respuesta = new respuestaErrores<RegistrarMedicoDTO>();

            try
            {
                var medico = _mapper.Map<Medico>(registrarMedicoDto);

                medico.FechaDeRegistro = DateTime.Now;

                bool resultado = await _medicoRepositorio.CreateMedicoAsync(medico);

                if (resultado)
                {
                    respuesta.EsCorrecto = true;
                    respuesta.mensaje = "Médico registrado exitosamente.";
                    respuesta.Dato = registrarMedicoDto;
                }
                else
                {
                    respuesta.EsCorrecto = false;
                    respuesta.mensaje = "No se pudo registrar al médico en el sistema.";
                    respuesta.codigo = 500;
                }
            }
            catch (Exception ex)
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje = $"Error interno en el servicio de médicos: {ex.Message}";
                respuesta.codigo = 500;
            }

            return respuesta;
        }

        public async Task<bool> DeleteMedicoAsync(int id)
        {
            return await _medicoRepositorio.DeleteMedicoAsync(id);
        }
    }
}