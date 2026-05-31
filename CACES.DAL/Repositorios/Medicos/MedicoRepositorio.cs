using CACES.DAL.DBContext;
using CACES.DAL.Entidades;
using Microsoft.EntityFrameworkCore;

namespace CACES.DAL.Repositorios.Medicos
{
    public class MedicoRepositorio : IMedicoRepositorio
    {
        private readonly CACESDbContext _context;

        public MedicoRepositorio(CACESDbContext context)
        {
            _context = context;
        }

        public async Task<List<Medico>> GetMedicosAsync()
        {
            return await _context.Medicos.ToListAsync();
        }


        public async Task<Medico> GetMedicoByIdAsync(int id)
        {
            return await _context.Medicos.FirstOrDefaultAsync(x => x.IdMedico == id);
        }

        public async Task<bool> CreateMedicoAsync(Medico medico)
        {
            if(medico == null) return false;
            await _context.Medicos.AddAsync(medico);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateMedicoAsync(Medico medico)
        {
            var existing = await _context.Medicos
                .FirstOrDefaultAsync(x => x.IdMedico == medico.IdMedico);

            if (existing == null)
                return false;

            existing.Nombre = medico.Nombre;
            existing.Especialidad = medico.Especialidad;
            existing.Experiencia = medico.Experiencia;
            existing.Descripcion = medico.Descripcion;
            existing.Estado = medico.Estado;
            existing.Foto = medico.Foto;

            _context.Medicos.Update(existing);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteMedicoAsync(int id)
        {
            var medico = await _context.Medicos.FindAsync(id);

            if (medico == null)
                return false;

            _context.Medicos.Remove(medico);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}