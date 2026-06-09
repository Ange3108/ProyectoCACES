using CACES.DAL.DBContext;
using Microsoft.EntityFrameworkCore;


namespace CACES.DAL.Repositorios.Especialidades
{
    public class EspecialidadRepositorio
    {
        //Inyerccion de dependencia de la BD
        private readonly CACESDbContext _context;


        public EspecialidadRepositorio(CACESDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateEspecialidadAsync(Entidades.Especialidad Especialidad)
        {
            if (Especialidad == null) return false;
            await _context.Especialidades.AddAsync(Especialidad);
            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<bool> DeleteEspecialidadAsync(int id)
        {
            var entity = await _context.Especialidades.FindAsync(id);
            if (entity == null) return false;
            _context.Especialidades.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Entidades.Especialidad> GetEspecialidadByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            return await _context.Especialidades.FirstOrDefaultAsync(e => e.Nombre == name);
        }

        public async Task<Entidades.Especialidad> GetEspecialidadByIdAsync(int id)
        {
            return await _context.Especialidades.FirstOrDefaultAsync(e => e.IdEspecialidad == id);
        }

        public async Task<List<Entidades.Especialidad>> GetEspecialidadsAsync()
        {
            return await _context.Especialidades.ToListAsync();
        }

        public async Task<bool> UpdateEspecialidadAsync(Entidades.Especialidad Especialidad)
        {
            if (Especialidad == null) return false;
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
    }
}

