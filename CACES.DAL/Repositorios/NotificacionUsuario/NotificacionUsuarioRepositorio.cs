using CACES.DAL.DBContext;
using CACES.DAL.Entidades.Configuración;
using CACES.DAL.Repositorios.Base;
using Microsoft.EntityFrameworkCore;

public class NotificacionUsuarioRepositorio : RepositorioGenerico<NotificacionUsuario>, INotificacionUsuarioRepositorio
{
    private readonly CACESDbContext _context;

    public NotificacionUsuarioRepositorio(CACESDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<NotificacionUsuario>> ObtenerPorUsuarioAsync(int idUsuario, bool soloNoLeidas = false, int limite = 20)
    {
        var consulta = _context.Set<NotificacionUsuario>()
            .Where(n => n.IdUsuario == idUsuario);

        if (soloNoLeidas)
            consulta = consulta.Where(n => !n.Leido);

        return await consulta
            .OrderByDescending(n => n.FechaCreacion)
            .Take(limite)
            .ToListAsync();
    }

    public async Task<int> ContarNoLeidasAsync(int idUsuario)
    {
        return await _context.Set<NotificacionUsuario>()
            .CountAsync(n => n.IdUsuario == idUsuario && !n.Leido);
    }

    public async Task MarcarTodasLeidasAsync(int idUsuario)
    {
        var pendientes = await _context.Set<NotificacionUsuario>()
            .Where(n => n.IdUsuario == idUsuario && !n.Leido)
            .ToListAsync();

        foreach (var notificacion in pendientes)
        {
            notificacion.Leido = true;
            notificacion.FechaLectura = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
    }
}
