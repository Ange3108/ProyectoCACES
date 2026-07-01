using CACES.DAL.DBContext;
using CACES.DAL.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.DAL.Repositorios.Horarios
{
    public class HorariosRepositorio : IHorariosRepositorio
    {
        private readonly CACESDbContext _context;

        public HorariosRepositorio(CACESDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegistrarHorariosAsync(IEnumerable<HorariosDisponibles> horarios, CancellationToken cancellationToken)
        {
            await _context.Set<HorariosDisponibles>().AddRangeAsync(horarios, cancellationToken);

            int filasAfectadas = await _context.SaveChangesAsync(cancellationToken);

            return filasAfectadas > 0;
        }
    }
}
