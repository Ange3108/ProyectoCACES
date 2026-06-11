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
            return await _context.Medicos
                .Include(x => x.Usuario)
                .ToListAsync();
        }

        public async Task<Medico?> GetMedicoByIdAsync(int id)
        {
            return await _context.Medicos
                .Include(x => x.Usuario)
                .FirstOrDefaultAsync(x => x.IdMedico == id);
        }

        public async Task<Medico?> GetMedicoConUsuarioByIdAsync(int id)
        {
            return await _context.Medicos
                .Include(x => x.Usuario)
                .FirstOrDefaultAsync(x => x.IdMedico == id);
        }

        public async Task<bool> CreateMedicoAsync(Medico medico)
        {
            if (medico == null)
                return false;

            await _context.Medicos.AddAsync(medico);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateMedicoAsync(Medico medico)
        {
            var existing = await _context.Medicos
                .FirstOrDefaultAsync(x => x.IdMedico == medico.IdMedico);

            if (existing == null)
                return false;

            existing.IdEspecialidad = medico.IdEspecialidad;
            existing.IdUsuario = medico.IdUsuario;
            existing.Experiencia = medico.Experiencia;
            existing.Certificaciones = medico.Certificaciones;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateMedicoConUsuarioAsync(Medico medico)
        {
            var existing = await _context.Medicos
                .Include(x => x.Usuario)
                .FirstOrDefaultAsync(x => x.IdMedico == medico.IdMedico);

            if (existing == null)
                return false;

            existing.IdEspecialidad = medico.IdEspecialidad;
            existing.Experiencia = medico.Experiencia;
            existing.Certificaciones = medico.Certificaciones;

            existing.Usuario.Nombres = medico.Usuario.Nombres;
            existing.Usuario.PrimerApellido = medico.Usuario.PrimerApellido;
            existing.Usuario.SegundoApellido = medico.Usuario.SegundoApellido;
            existing.Usuario.Telefono = medico.Usuario.Telefono;
            existing.Usuario.Foto = medico.Usuario.Foto;
            existing.Usuario.Estado = medico.Usuario.Estado;
            existing.Usuario.FechaDeModificacion = DateTime.Now;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteMedicoAsync(int id)
        {
            var medico = await _context.Medicos
                .FirstOrDefaultAsync(x => x.IdMedico == id);

            if (medico == null)
                return false;

            var idUsuario = medico.IdUsuario;

            await _context.Database.ExecuteSqlRawAsync(
                "DELETE FROM Recetas WHERE Id_Cita IN (SELECT Id_Cita FROM Citas WHERE Id_Medico = {0})", id);

            await _context.Database.ExecuteSqlRawAsync(
                "DELETE FROM Citas WHERE Id_Medico = {0}", id);

            await _context.Database.ExecuteSqlRawAsync(
                "DELETE FROM Cirugias WHERE Id_Medico = {0}", id);

            await _context.Database.ExecuteSqlRawAsync(
                "DELETE FROM Precios WHERE Id_Medico = {0}", id);

            await _context.Database.ExecuteSqlRawAsync(
                "DELETE FROM HorariosDisponibles WHERE Id_Medico = {0}", id);

            await _context.Database.ExecuteSqlRawAsync(
                "DELETE FROM AspNetUserRoles WHERE UserId = {0}", idUsuario.ToString());

            _context.Medicos.Remove(medico);

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(x => x.IdUsuario == idUsuario);

            if (usuario != null)
                _context.Usuarios.Remove(usuario);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}