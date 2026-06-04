using CACES.BLL.DTOs.Medico;
using CACES.DAL.Entidades;
using CACES.DAL.Repositorios.Medicos;

namespace CACES.BLL.Servicios.Medicos
{
    public class MedicoServicio : IMedicoServicio
    {
        private readonly IMedicoRepositorio _medicoRepositorio;

        public MedicoServicio(IMedicoRepositorio medicoRepositorio)
        {
            _medicoRepositorio = medicoRepositorio;
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
                Foto = medico.Foto,
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
                Foto = dto.Foto, 

                Usuario = new CACES.DAL.Entidades.Usuario
                {
                    IdUsuario = dto.IdUsuario,
                    Nombres = dto.Nombres,
                    PrimerApellido = dto.PrimerApellido,
                    SegundoApellido = dto.SegundoApellido,
                    Telefono = dto.Telefono,
                    Estado = dto.Estado
                }
            };

            return await _medicoRepositorio.UpdateMedicoConUsuarioAsync(medico);
        }

        public async Task<bool> CreateMedicoAsync(Medico medico)
        {
            return await _medicoRepositorio.CreateMedicoAsync(medico);
        }

        public async Task<bool> DeleteMedicoAsync(int id)
        {
            return await _medicoRepositorio.DeleteMedicoAsync(id);
        }
    }
}