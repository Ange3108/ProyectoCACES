using CACES.BLL.DTOs.Precio;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.Servicios.Precio
{
    public interface IPrecioServicio
    {
        Task<IEnumerable<MostrarPrecioDTO>> GetAllPreciosAsync();
        Task<MostrarPrecioDTO?> GetPrecioByIdAsync(int id);
        Task<IEnumerable<MostrarPrecioDTO>> GetPreciosByMedicoAsync(int idMedico);
        Task<MostrarPrecioDTO?> CreatePrecioAsync(RegistrarPrecioDTO dto);
        Task<bool> UpdatePrecioAsync(EditarPrecioDTO dto);

        
    }
}
