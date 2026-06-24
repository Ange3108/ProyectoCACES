using CACES.DAL.Entidades;

namespace CACES.DAL.Repositorios.Medicos
{
    public interface IMedicoRepositorio
    {
        Task<List<Medico>> GetMedicosAsync();
        Task<Medico> GetMedicoByIdAsync(int id);
        Task<List<Medico>> GetEspecialistasActivosAsync();
        Task<Medico?> GetMedicoConUsuarioByIdAsync(int id);
        Task<bool> CreateMedicoAsync(Medico medico);
        // Nombre completo para evitar el conflicto con el namespace
        Task<bool> CreateMedicoConUsuarioAsync(
            Entidades.Usuario usuario,
            Medico medico);
        Task<bool> UpdateMedicoAsync(Medico medico);
        Task<bool> UpdateMedicoConUsuarioAsync(Medico medico);
        Task<bool> DesactivarMedicoAsync(int id);
    }
}