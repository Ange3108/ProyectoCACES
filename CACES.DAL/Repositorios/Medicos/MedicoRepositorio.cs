using CACES.DAL.DBContext;
using CACES.DAL.Entidades;
using CACES.DAL.Entidades.Roles;
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
                .Include(x => x.Especialidad)
                .ToListAsync();
        }

        public async Task<Medico?> GetMedicoByIdAsync(int id)
        {
            return await _context.Medicos
                .Include(x => x.Usuario)
                .FirstOrDefaultAsync(x => x.IdMedico == id);
        }

        public async Task<List<Medico>> GetEspecialistasActivosAsync()
        {
            return await _context.Medicos
                .Include(x => x.Usuario)
                .Include(x => x.Especialidad)
                .Where(m => m.Usuario.Estado == true &&
                            m.Especialidad != null &&
                            m.Especialidad.Estado == true)
                .ToListAsync();
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

        public async Task<bool> DesactivarMedicoAsync(int id)
        {
            var medico = await _context.Medicos
                .Include(x => x.Usuario)
                .FirstOrDefaultAsync(x => x.IdMedico == id);

            if (medico == null)
                return false;

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                //await _context.Database.ExecuteSqlRawAsync(
                   // "UPDATE Recetas SET Id_Cita = NULL WHERE Id_Cita IN (SELECT Id_Cita FROM Citas WHERE Id_Medico = {0})", id);

                await _context.Database.ExecuteSqlRawAsync(
                    "DELETE FROM Citas WHERE Id_Medico = {0}", id);

                await _context.Database.ExecuteSqlRawAsync(
                    "UPDATE Cirugias SET Estado = 0 WHERE Id_Medico = {0}", id);

                await _context.Database.ExecuteSqlRawAsync(
                    "UPDATE Precios SET Estado = 0 WHERE Id_Medico = {0}", id);

                await _context.Database.ExecuteSqlRawAsync(
                    "UPDATE HorariosDisponibles SET Activo = 0 WHERE Id_Medico = {0}", id);

                medico.Usuario.Estado = false;
                medico.Usuario.FechaDeModificacion = DateTime.Now;

                var ok = await _context.SaveChangesAsync() > 0;

                await transaction.CommitAsync();
                return ok;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> CreateMedicoConUsuarioAsync(Entidades.Usuario usuario, Medico medico)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.Usuarios.AddAsync(usuario);
                await _context.SaveChangesAsync();

                medico.IdUsuario = usuario.IdUsuario;
                await _context.Medicos.AddAsync(medico);

                var rolMedico = await _context.AspNetRoles
                    .FirstOrDefaultAsync(r => r.Name == "Medico");

                if (rolMedico == null)
                    throw new Exception("No existe el rol Medico.");

                var yaTieneRolUsuarioRoles = await _context.UsuarioRoles
                    .AnyAsync(x => x.IdUsuario == usuario.IdUsuario &&
                                   x.RoleId == rolMedico.Id);

                if (!yaTieneRolUsuarioRoles)
                {
                    await _context.UsuarioRoles.AddAsync(new UsuarioRoles
                    {
                        IdUsuario = usuario.IdUsuario,
                        RoleId = rolMedico.Id
                    });
                }

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}