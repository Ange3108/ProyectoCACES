using CACES.DAL.DBContext;
using CACES.DAL.Entidades;
using Microsoft.EntityFrameworkCore;

namespace CACES.DAL.Repositorios.Paquetes
{
    public class PaqueteRepositorio : IPaqueteRepositorio
    {
        public readonly CACESDbContext _context;

        public PaqueteRepositorio(CACESDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreatePaqueteAsync(Paquete paquete)
        {
            if (paquete == null)
                return false;

            await _context.Paquetes.AddAsync(paquete);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task< List<Paquete>> GetPaquetesAsync()
        {
            return await _context.Paquetes.ToListAsync();

        }

        public async Task<List<Paquete>> GetPaquetesSoloActivosAsync()
        {
            return await _context.Paquetes.Where(p => p.Estado).ToListAsync();
        }

        public async Task<bool> UpdatePaqueteAsync(Paquete paquete)
        {
            if (paquete == null) return false;
            _context.Paquetes.Update(paquete);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Paquete> GetPaqueteByIdAsync(int id)
        {
            return await _context.Paquetes.FirstOrDefaultAsync(e => e.IdPaquete == id);
        }
    }
}
