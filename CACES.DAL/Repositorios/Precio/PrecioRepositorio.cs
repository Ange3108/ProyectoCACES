using CACES.DAL.DBContext;
using CACES.DAL.Entidades;
using Microsoft.EntityFrameworkCore;

namespace CACES.DAL.Repositorios.Precio
{
    public class PrecioRepositorio : IPrecioRepositorio
    {
        private readonly CACESDbContext _context;

        public PrecioRepositorio(CACESDbContext context)
        {
            _context = context;
        }

        public async Task<List<Precios>> ObtenerTodosAsync()
        {
            return await _context.Precios
                .AsNoTracking()
                .Include(p => p.Medico)
                    .ThenInclude(m => m.Usuario)
                .Include(p => p.Medico)
                    .ThenInclude(m => m.Especialidad)
                .Include(p => p.Procedimiento)
                .OrderBy(p => p.Procedimiento.Nombre)
                .ToListAsync();
        }

        public async Task<Precios?> ObtenerPorIdAsync(int idPrecio)
        {
            return await _context.Precios
                .Include(p => p.Medico)
                    .ThenInclude(m => m.Usuario)
                .Include(p => p.Medico)
                    .ThenInclude(m => m.Especialidad)
                .Include(p => p.Procedimiento)
                .FirstOrDefaultAsync(p =>
                    p.Id_Precio == idPrecio
                );
        }

        public async Task<List<Precios>> ObtenerPorMedicoIdAsync(
            int idMedico)
        {
            return await _context.Precios
                .AsNoTracking()
                .Include(p => p.Procedimiento)
                .Include(p => p.Medico)
                    .ThenInclude(m => m.Usuario)
                .Include(p => p.Medico)
                    .ThenInclude(m => m.Especialidad)
                .Where(p => p.Id_Medico == idMedico)
                .OrderBy(p => p.Procedimiento.Nombre)
                .ToListAsync();
        }

        public async Task<Precios?>
            ObtenerPorMedicoYProcedimientoAsync(
                int idMedico,
                int idProcedimiento)
        {
            return await _context.Precios
                .Include(p => p.Procedimiento)
                .Include(p => p.Medico)
                    .ThenInclude(m => m.Usuario)
                .FirstOrDefaultAsync(p =>
                    p.Id_Medico == idMedico &&
                    p.Id_Procedimiento == idProcedimiento
                );
        }

        public async Task<Precios> AgregarAsync(
            Precios precio)
        {
            precio.FechaDeRegistro = DateTime.UtcNow;
            precio.Estado = true;

            await _context.Precios.AddAsync(precio);
            await _context.SaveChangesAsync();

            return precio;
        }

        public async Task<Precios> ActualizarAsync(
            Precios precio)
        {
            precio.FechaDeModificacion = DateTime.UtcNow;

            _context.Precios.Update(precio);
            await _context.SaveChangesAsync();

            return precio;
        }
    }
}