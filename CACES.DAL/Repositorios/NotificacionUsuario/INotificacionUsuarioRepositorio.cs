using CACES.DAL.Entidades.Configuración;
using CACES.DAL.Repositorios.Base;

public interface INotificacionUsuarioRepositorio : IRepositorioGenerico<NotificacionUsuario>
{
    Task<List<NotificacionUsuario>> ObtenerPorUsuarioAsync(int idUsuario, bool soloNoLeidas = false, int limite = 20);
    Task<int> ContarNoLeidasAsync(int idUsuario);
    Task MarcarTodasLeidasAsync(int idUsuario);
}
