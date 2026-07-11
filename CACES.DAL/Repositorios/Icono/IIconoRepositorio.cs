namespace CACES.DAL.Repositorios.Icono
{
    public interface IIconoRepositorio
    {
       
            Task<List<Entidades.Icono>> GetTodosLosIconosAsync();
            Task<Entidades.Icono?> GetPorIdAsync(int id);
            Task<bool> CrearAsync(Entidades.Icono icono);
            Task<bool> ActualizarAsync(Entidades.Icono icono);
       
    }
}
