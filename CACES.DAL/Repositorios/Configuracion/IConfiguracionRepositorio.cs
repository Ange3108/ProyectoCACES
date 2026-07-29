using CACES.DAL.Entidades.Configuración;
using CACES.DAL.Repositorios.Base;

public interface IConfiguracionRepositorio : IRepositorioGenerico<Configuracion>
{
    Task<Configuracion?> ObtenerPorClaveAsync(string clave);
    Task<List<Configuracion>> ObtenerPorCategoriaAsync(string categoria);
}