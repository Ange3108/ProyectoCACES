using CACES.BLL.DTOs.Medico;
using CACES.DAL.Entidades;

namespace CACES.BLL.Servicios.Medicos
{
    public interface IMedicoServicio
    {
        Task<List<Medico>> GetMedicosAsync();

        Task<EditarMedicoDTO?> GetMedicoParaEditarAsync(int id);

        Task<bool> UpdateMedicoConUsuarioAsync(EditarMedicoDTO dto);

        Task<bool> CreateMedicoAsync(Medico medico);

        Task<bool> DeleteMedicoAsync(int id);
    }
}