using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Paquete;

namespace CACES.BLL.Servicios.Paquete
{
    public interface IPaqueteServicio
    {
        Task<respuestaErrores<PaqueteDTO>> CreatePaqueteAsync(PaqueteDTO registrarPaqueteDto);
        Task<List<PaqueteDTO>> GetPaquetesAsync();
        Task<List<PaqueteDTO>> GetPaquetesSoloActivosAsync();
        Task<respuestaErrores<PaqueteDTO>> UpdatePaqueteAsync(int id, PaqueteDTO registrarPaqueteDTO);
        Task<respuestaErrores<PaqueteDTO>> GetPaquetePorIdAsync(int id);
    }
}
