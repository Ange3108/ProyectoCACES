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
                .Include(c => c.Procedimiento)
                .Include(c => c.Medico) 
                    .ThenInclude(m => m.Usuario)
                .Where(c=> c.Id_Paciente == idPaciente)
                .Include(c => c.Horario)      
                .ToListAsync();
        }
    }
}
