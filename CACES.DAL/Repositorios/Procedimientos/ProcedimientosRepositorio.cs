using CACES.DAL.DBContext;
using CACES.DAL.Entidades;
using Microsoft.EntityFrameworkCore;

namespace CACES.DAL.Repositorios.Procedimientos
{
    public class ProcedimientosRepositorio : IProcedimientosRepositorio
    {
        private readonly CACESDbContext _context;

        public ProcedimientosRepositorio(CACESDbContext context)
        {
            _context = context;
        }



        public async Task<List<Procedimiento>> ObtenerTodosLosProcedimientosAsync()
        {
            return await _context.Procedimientos
                .Include(p => p.Especialidad)
                .ToListAsync();
        }

        public async Task<bool> InsertarProcedimientoAsync(Procedimiento entidad)
        {
            bool yaExiste = await _context.Procedimientos
        .AnyAsync(p => p.Nombre.ToLower().Trim() == entidad.Nombre.ToLower().Trim()
                    && p.Id_Especialidad == entidad.Id_Especialidad);

            if (yaExiste)
            {
                return false;
                
            }

            await _context.Procedimientos.AddAsync(entidad);
            var filasAfectadas = await _context.SaveChangesAsync();

            return filasAfectadas > 0;
        }

        public async Task<Procedimiento> ObtenerProcedimientoPorIdAsync(int id)
        {
            return await _context.Procedimientos
                .Include(p => p.Especialidad)
                .FirstOrDefaultAsync(p => p.Id_Procedimiento == id); 
        }

        public async Task<bool> ActualizarProcedimientoAdminAsync(Procedimiento entidad)
        {
            bool yaExiste = await _context.Procedimientos
                .AnyAsync(p => p.Nombre.ToLower().Trim() == entidad.Nombre.ToLower().Trim()
                            && p.Id_Especialidad == entidad.Id_Especialidad
                            && p.Id_Procedimiento != entidad.Id_Procedimiento);

            if (yaExiste)
            {
                return false;
            }

            _context.Procedimientos.Update(entidad);
            var filasAfectadas = await _context.SaveChangesAsync();
            return filasAfectadas > 0;
        }

        public async Task<bool> CambiarEstadoProcedimientoAsync(int id)
        {
            var procedimiento = await _context.Procedimientos.FindAsync(id);

            if (procedimiento == null)
            {
                return false; 
            }

            procedimiento.Estado = !procedimiento.Estado;

            _context.Procedimientos.Update(procedimiento);
            var filasAfectadas = await _context.SaveChangesAsync();

            return filasAfectadas > 0;
        }
    }
}
