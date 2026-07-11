using CACES.DAL.Entidades.Icono;

namespace CACES.DAL.Repositorios.Icono
{
    public interface IIconoRepositorio
    {
       
            Task<List<Icono>> GetTodosLosIconosAsync();
            Task<Icono?> GetPorIdAsync(int id);
            Task<bool> CrearAsync(Icono icono);
            Task<bool> ActualizarAsync(Icono icono);
       
    }
}
