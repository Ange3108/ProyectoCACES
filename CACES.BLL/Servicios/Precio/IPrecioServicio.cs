using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Precio;

namespace CACES.BLL.Servicios.Precio
{
    public interface IPrecioServicio
    {
        Task<respuestaErrores<List<MostrarPrecioDTO>>>
            ObtenerTodosAsync();

        Task<respuestaErrores<EditarPrecioDTO>>
            ObtenerEditarAsync(int idPrecio);

        Task<respuestaErrores<MostrarPrecioDTO>>
            ActualizarAsync(EditarPrecioDTO dto);
    }
}