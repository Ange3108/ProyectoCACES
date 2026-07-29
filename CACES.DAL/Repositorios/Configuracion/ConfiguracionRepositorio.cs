using CACES.DAL.DBContext;
using CACES.DAL.Entidades.Configuración;
using CACES.DAL.Repositorios.Base;
using Microsoft.EntityFrameworkCore;

public class ConfiguracionRepositorio : RepositorioGenerico<Configuracion>, IConfiguracionRepositorio
{
    private readonly CACESDbContext _context;

    public ConfiguracionRepositorio(CACESDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Configuracion?> ObtenerPorClaveAsync(string clave)
    {
        return await _context.Set<Configuracion>()
            .FirstOrDefaultAsync(c => c.Clave == clave);
    }

    public async Task<List<Configuracion>> ObtenerPorCategoriaAsync(string categoria)
    {
        return await _context.Set<Configuracion>()
            .Where(c => c.Categoria == categoria)
            .ToListAsync();
    }
}