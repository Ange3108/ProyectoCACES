using CACES.DAL.Entidades;

namespace CACES.DAL.Repositorios.Medicos
{
    public interface IMedicoRepositorio
    {
        Task<List<Medico>> GetMedicosAsync();
        Task<Medico> GetMedicoByIdAsync(int id);
        Task<Medico?> GetMedicoConUsuarioByIdAsync(int id);

        Task<bool> CreateMedicoAsync(Medico medico);
        Task<bool> UpdateMedicoAsync(Medico medico);
        Task<bool> UpdateMedicoConUsuarioAsync(Medico medico);
        Task<bool> DeleteMedicoAsync(int id);
    }
}