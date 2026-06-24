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
    }
}
