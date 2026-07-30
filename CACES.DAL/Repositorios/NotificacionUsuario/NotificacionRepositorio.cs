using CACES.DAL.DBContext;
using CACES.DAL.Entidades.Configuración;
using CACES.DAL.Repositorios.Base;
using Microsoft.EntityFrameworkCore;

public class NotificacionRepositorio : RepositorioGenerico<Notificacion>, INotificacionRepositorio
{
    private readonly CACESDbContext _context;

    public NotificacionRepositorio(CACESDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Notificacion?> ObtenerPorEventoAsync(string evento)
    {
        return await _context.Set<Notificacion>()
            .FirstOrDefaultAsync(n => n.Evento == evento);
    }
}