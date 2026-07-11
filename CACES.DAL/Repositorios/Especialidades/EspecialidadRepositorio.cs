using CACES.DAL.DBContext;
using CACES.DAL.Entidades;
using Microsoft.EntityFrameworkCore;


namespace CACES.DAL.Repositorios.Especialidades
{
    public class EspecialidadRepositorio : IEspecialidadRepositorio
    {
        //Inyerccion de dependencia de la BD
        private readonly CACESDbContext _context;


        public EspecialidadRepositorio(CACESDbContext context)
        {
            _context = context;
        }

        public async Task<List<Especialidad>> GetEspecialidadesAsync()
        {
            return await _context.Especialidades.ToListAsync();
        }
        public async Task<List<Especialidad>> GetEspecialidadesActivasAsync()
        {
            return await _context.Especialidades
                .Include(e => e.Icono)
                .Where(e => e.Estado)
                .ToListAsync();
        }

        public async Task<Especialidad> GetEspecialidadByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            return await _context.Especialidades.FirstOrDefaultAsync(e => e.Nombre == name);
        }


        public async Task<Especialidad> GetEspecialidadByIdAsync(int id)
        {
            return await _context.Especialidades.FirstOrDefaultAsync(e => e.IdEspecialidad == id);
        }

       
        public async Task<bool> CreateEspecialidadAsync(Especialidad Especialidad)
        {
            if (Especialidad == null) return false;
            await _context.Especialidades.AddAsync(Especialidad);
            return await _context.SaveChangesAsync() > 0;

        }
        public async Task<bool> UpdateEspecialidadAsync(Especialidad Especialidad)
        {
            if (Especialidad == null)
                return false;

            _context.Especialidades.Update(Especialidad);

            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> DesactivarEspecialidadAsync(int id)
        {
            var Especialidad = await _context.Especialidades
                .FirstOrDefaultAsync(e => e.IdEspecialidad == id);

            if (Especialidad == null)
                return false;

            Especialidad.Estado = false;

            _context.Especialidades.Update(Especialidad);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Especialidad?> GetEspecialidadDetallesByIdAsync(int id)
        {
            return await _context.Especialidades
                .Include(e => e.Procedimientos)
                .Include(e => e.Icono)
                .Where(p => p.Estado == true)
                .Include(e => e.Medicos)
                .ThenInclude(m => m.Usuario)
                .FirstOrDefaultAsync(e => e.IdEspecialidad == id);
           
        }
    }
}

