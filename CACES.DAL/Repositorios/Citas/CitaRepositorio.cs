using CACES.DAL.DBContext;
using CACES.DAL.Entidades;
using Microsoft.EntityFrameworkCore;

namespace CACES.DAL.Repositorios.Citas
{
    public class CitaRepositorio : ICitaRepositorio
    {
        private readonly CACESDbContext _context;

        public CitaRepositorio(CACESDbContext context)
        {
            _context = context;
        }

        public async Task<List<Cita>> ObtenerTodasAsync()
        {
            return await _context.Citas
                .AsNoTracking()
                .Include(c => c.Paciente)
                    .ThenInclude(p => p.Usuario)
                .Include(c => c.Medico)
                    .ThenInclude(m => m.Usuario)
                .Include(c => c.Especialidad)
                .Include(c => c.Horario)
                .Include(c => c.Receta)
                .Include(c => c.Procedimiento)
                .OrderByDescending(c => c.Fecha)
                .ThenBy(c => c.IdHorario)
                .ToListAsync();
        }

        public async Task<List<Cita>> ObtenerPorPacienteAsync(int idPaciente)
        {
            return await _context.Citas
                .AsNoTracking()
                .Include(c => c.Paciente)
                    .ThenInclude(p => p.Usuario)
                .Include(c => c.Medico)
                    .ThenInclude(m => m.Usuario)
                .Include(c => c.Especialidad)
                .Include(c => c.Horario)
                .Include(c => c.Receta)
                .Include(c => c.Procedimiento)
                .Where(c => c.IdPaciente == idPaciente)
                .OrderByDescending(c => c.Fecha)
                .ThenBy(c => c.IdHorario)
                .ToListAsync();
        }

        public async Task<List<Cita>> ObtenerPorMedicoAsync(int idMedico)
        {
            return await _context.Citas
                .AsNoTracking()
                .Include(c => c.Paciente)
                    .ThenInclude(p => p.Usuario)
                .Include(c => c.Medico)
                    .ThenInclude(m => m.Usuario)
                .Include(c => c.Especialidad)
                .Include(c => c.Horario)
                .Include(c => c.Receta)
                .Include(c => c.Procedimiento)
                .Where(c => c.IdMedico == idMedico)
                .OrderByDescending(c => c.Fecha)
                .ThenBy(c => c.IdHorario)
                .ToListAsync();
        }

        public async Task<Cita?> ObtenerEntidadPorIdAsync(int idCita)
        {
            return await _context.Citas
                .Include(c => c.Paciente)
                    .ThenInclude(p => p.Usuario)
                .Include(c => c.Medico)
                    .ThenInclude(m => m.Usuario)
                .Include(c => c.Especialidad)
                .Include(c => c.Horario)
                .Include(c => c.Receta)
                .Include(c => c.Procedimiento)
                .FirstOrDefaultAsync(c => c.IdCita == idCita);
        }

        public async Task<Cita> RegistrarAsync(Cita cita)
        {
            cita.FechaDeRegistro = DateTime.UtcNow;
            cita.FechaDeModificacion = null;
            cita.Estado = 1;

            await _context.Citas.AddAsync(cita);
            await _context.SaveChangesAsync();

            return cita;
        }

        public async Task<Cita> ActualizarAsync(Cita cita)
        {
            cita.FechaDeModificacion = DateTime.UtcNow;

            _context.Citas.Update(cita);
            await _context.SaveChangesAsync();

            return cita;
        }

        public async Task<bool> ExisteCitaAsync(
            int idMedico,
            DateTime fecha,
            int idHorario,
            int? excluir = null)
        {
            var consulta = _context.Citas
                .AsNoTracking()
                .Where(c =>
                    c.IdMedico == idMedico &&
                    c.Fecha.Date == fecha.Date &&
                    c.IdHorario == idHorario &&
                    c.Estado == 1);

            if (excluir.HasValue)
            {
                consulta = consulta.Where(c => c.IdCita != excluir.Value);
            }

            return await consulta.AnyAsync();
        }

        public async Task<List<Medico>> ObtenerMedicosAsync(int? idEspecialidad = null)
        {
            var query = _context.Medicos
                .AsNoTracking()
                .Include(m => m.Usuario)
                .Include(m => m.Especialidad)
                .Where(m => m.Usuario.Estado);

            if (idEspecialidad.HasValue)
            {
                query = query.Where(m => m.IdEspecialidad == idEspecialidad.Value);
            }

            return await query
                .OrderBy(m => m.Usuario.Nombres)
                .ThenBy(m => m.Usuario.PrimerApellido)
                .ToListAsync();
        }

        public async Task<List<Especialidad>> ObtenerEspecialidadesAsync()
        {
            return await _context.Especialidades
                .AsNoTracking()
                .Where(e => e.Estado)
                .OrderBy(e => e.Nombre)
                .ToListAsync();
        }

        public async Task<List<Procedimiento>> ObtenerProcedimientosFijosAsync(int? idEspecialidad = null)
        {
            var query = _context.Procedimientos
                .AsNoTracking()
                .Where(p => p.Estado == true);

            if (idEspecialidad.HasValue)
            {
                query = query.Where(p => p.Id_Especialidad == idEspecialidad.Value);
            }

            return await query
                .OrderBy(p => p.Nombre)
                .ToListAsync();
        }

        public async Task<List<HorariosDisponibles>> ObtenerHorariosAsync(int idMedico)
        {
            return await _context.HorariosDisponibles
                .AsNoTracking()
                .Where(h => h.Id_Medico == idMedico && h.Estado)
                .OrderBy(h => h.DiaSemana)
                .ThenBy(h => h.HoraInicio)
                .ToListAsync();
        }

        public async Task<int> ContarCitasPorFechaAsync(DateTime fecha)
        {
            return await _context.Citas
                .AsNoTracking()
                .CountAsync(c =>
                    c.Fecha.Date == fecha.Date &&
                    c.Estado == 1);
        }

        public async Task<bool> TieneHorarioActivoAsync(
            int idMedico,
            int diaSemana)
        {
            return await _context.HorariosDisponibles
                .AsNoTracking()
                .AnyAsync(h =>
                    h.Id_Medico == idMedico &&
                    h.DiaSemana == diaSemana &&
                    h.Estado);
        }
        public async Task<bool> ExisteCitaConHorario(int idHorario)
        {
            return await _context.Citas.AnyAsync(c => c.IdHorario == idHorario && c.Estado==1);
        }
    }
}