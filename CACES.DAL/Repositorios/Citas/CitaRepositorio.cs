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

        public async Task<List<Cita>> GetCitasAsync()
        {
            return await _context.Citas
                .OrderBy(c => c.Fecha)
                .ToListAsync();
        }

        public async Task<Cita?> GetCitaByIdAsync(int idCita)
        {
            return await _context.Citas
                .FirstOrDefaultAsync(c => c.IdCita == idCita);
        }

        public async Task<bool> ActualizarFechaCitaAsync(int idCita, DateTime nuevaFecha)
        {
            var cita = await _context.Citas
                .FirstOrDefaultAsync(c => c.IdCita == idCita);

            if (cita == null)
                return false;

            cita.Fecha = nuevaFecha.Date;
            cita.FechaDeModificacion = DateTime.Now;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> CancelarCitasPorMedicoYFechaAsync(int idMedico, DateTime fechaCita)
        {
            var citas = await _context.Citas
                .Where(c => c.IdMedico == idMedico &&
                            c.Fecha.Date == fechaCita.Date &&
                            c.Estado == 1)
                .ToListAsync();

            if (!citas.Any())
                return false;

            foreach (var cita in citas)
            {
                cita.Estado = 0;
                cita.FechaDeModificacion = DateTime.Now;
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RegistrarCitaAsync(Cita cita)
        {
            var filas = await _context.Database.ExecuteSqlInterpolatedAsync($@"
                INSERT INTO Citas
                (Id_Paciente, Id_Medico, Id_Especialidad, Id_Horario, Fecha, Motivo, FechaDeRegistro, FechaDeModificacion, Estado)
                VALUES
                ({cita.IdPaciente}, {cita.IdMedico}, {cita.IdEspecialidad}, {cita.IdHorario}, {cita.Fecha}, {cita.Motivo}, {cita.FechaDeRegistro}, NULL, {cita.Estado})
            ");

            return filas > 0;
        }

        public async Task<List<Cita>> ObtenerCitasPorPacienteAsync(int idPaciente)
        {
            return await _context.Citas
                .Where(c => c.IdPaciente == idPaciente)
                .OrderByDescending(c => c.Fecha)
                .ToListAsync();
        }

        public async Task<Cita?> ObtenerTicketAsync(int idCita)
        {
            return await _context.Citas
                .FirstOrDefaultAsync(c => c.IdCita == idCita);
        }

        public async Task<List<Cita>> ObtenerCitasPorMedicoAsync(int idMedico)
        {
            return await _context.Citas
                .Where(c => c.IdMedico == idMedico)
                .OrderBy(c => c.Fecha)
                .ToListAsync();
        }

        public async Task<bool> CancelarCitaAsync(int idCita)
        {
            var cita = await _context.Citas
                .FirstOrDefaultAsync(c => c.IdCita == idCita);

            if (cita == null)
                return false;

            cita.Estado = 0;
            cita.FechaDeModificacion = DateTime.Now;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> ContarCitasPorFechaAsync(DateTime fecha)
        {
            return await _context.Citas
                .CountAsync(c => c.Fecha.Date == fecha.Date && c.Estado == 1);
        }

        public async Task<bool> TieneHorarioActivoAsync(int idMedico, int diaSemana)
        {
            return await _context.HorariosDisponibles
                .AnyAsync(h => h.Id_Medico == idMedico &&
                               h.DiaSemana == diaSemana &&
                               h.Activo == true);
        }
    }
}