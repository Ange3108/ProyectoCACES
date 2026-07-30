using CACES.DAL.Entidades.Configuración;
using CACES.DAL.Repositorios.Base;

public interface INotificacionRepositorio : IRepositorioGenerico<Notificacion>
{
    Task<Notificacion?> ObtenerPorEventoAsync(string evento);
}
