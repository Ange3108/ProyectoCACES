using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Medico;
using CACES.DAL.Entidades;

namespace CACES.BLL.Servicios.Medicos
{
    public interface IMedicoServicio
    {
        Task<respuestaErrores<List<MedicoDTO>>> GetMedicosAsync();

        Task<respuestaErrores<MedicoDTO>> GetMedicoPorIdAsync(int id);

        Task<respuestaErrores<List<MedicoDTO>>> GetEspecialistasActivosAsync();

        Task<respuestaErrores<EditarMedicoDTO>> GetMedicoParaEditarAsync(int id);

        Task<respuestaErrores<MedicoDTO>> UpdateMedicoConUsuarioAsync(EditarMedicoDTO dto);

        Task<respuestaErrores<MedicoDTO>> CreateMedicoAsync(RegistrarMedicoDTO registrarMedicoDto);

        Task<respuestaErrores<MedicoDTO>> DesactivarMedicoAsync(int id);
    }
}