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

        public async Task<List<Cirugias?>> ObtenerDetalleCirugiaAsync(int idPaciente)
        {
            return await _context.Cirugias
        .Where(c => c.Id_Paciente == idPaciente)
        .Include(c => c.Procedimiento)
        .Include(c => c.Horario)
        .Include(c => c.Medico).ThenInclude(m => m.Usuario)
        .Include(c => c.Paciente).ThenInclude(p => p.Usuario)
        .ToListAsync();
        }

        public async Task<bool> ActualizarProcedimientoAsync(Cirugias cirugias)
        {
            _context.Cirugias.Update(cirugias);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RegistrarProcedimientosAsync(Cirugias cirugia)
        {
            try
            {
                await _context.Cirugias.AddAsync(cirugia);
                return await _context.SaveChangesAsync() > 0;

            }
            catch(Exception ex) 
            {
                return false;
            }
        }

        public async Task<HorariosDisponibles> ObtenerHorarioPorRangoAsync(int idMedico, int diaSemana, TimeSpan hora)
        {
            return await _context.HorariosDisponibles
                .FirstOrDefaultAsync(h => h.Id_Medico == idMedico
                                       && h.DiaSemana == diaSemana
                                       && h.Activo
                                       && h.HoraInicio <= hora);
        }

        public async Task<List<Procedimiento>> ObtenerProcedimientosFijosAsync()
        {
            return await _context.Procedimientos
                .Where(p => p.Estado == true)
                .OrderBy(p => p.Nombre)
                .ToListAsync();
        }

        public async Task<List<Cirugias>> ObtenerCirugiasPorMedicoAsync(int idMedico)
        {
            return await _context.Cirugias
        .Where(c => c.Id_Medico == idMedico) 
        .Include(c => c.Procedimiento)       
        .Include(c => c.Horario)             
        .Include(c => c.Paciente)           
            .ThenInclude(p => p.Usuario)    
        .Include(c => c.Medico)              
            .ThenInclude(m => m.Usuario)    
        .ToListAsync();
        }

        public async Task<List<Cirugias>> ObtenerTodasLasCirugiasAsync()
        {
            return await _context.Cirugias
                .Include(c => c.Procedimiento)
                .Include(c => c.Paciente)
                    .ThenInclude(p => p.Usuario) 
                .Include(c => c.Medico)
                    .ThenInclude(m => m.Usuario) 
                .Include(c => c.Horario)
                .ToListAsync();
        }

        public async Task<Cirugias?> ObtenerCirugiaParaReporteAsync(int idCirugia)
        {
            return await _context.Cirugias
                .Include(c => c.Procedimiento)

                .Include(c => c.Paciente)

                .Include(c => c.Medico)
                    .ThenInclude(m => m.Usuario)

                .FirstOrDefaultAsync(c => c.Id_Cirugia == idCirugia);
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
