using CACES.BLL.DTOs.Cita;
using CACES.BLL.Servicios.Citas;
using CACES.BLL.Servicios.Paciente;
using CACES.DAL.DBContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CACES.Controllers
{
    public class CitaController : Controller
    {
        private readonly ICitaServicio _citaServicio;
        private readonly IPacienteServicio _pacienteServicio;
        private readonly CACESDbContext _context;

        public CitaController(
            ICitaServicio citaServicio,
            IPacienteServicio pacienteServicio,
            CACESDbContext context)
        {
            _citaServicio = citaServicio;
            _pacienteServicio = pacienteServicio;
            _context = context;
        }

        [Authorize(Roles = "Paciente")]
        [HttpGet]
        public async Task<IActionResult> MisCitas()
        {
            var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuario))
                return Unauthorized();

            var paciente = await _pacienteServicio.GetPacienteByUsuarioIdAsync(idUsuario);

            if (paciente == null)
                return NotFound();

            var citas = await _citaServicio.ObtenerCitasPorPacienteAsync(paciente.IdPaciente);

            return View("~/Views/Cita/MisCitas.cshtml", citas);
        }

        [Authorize(Roles = "Paciente")]
        [HttpGet]
        public async Task<IActionResult> RegistrarCita()
        {
            ViewBag.Medicos = await _context.Medicos
                .Include(m => m.Usuario)
                .ToListAsync();

            ViewBag.Especialidades = await _context.Especialidades
                .Where(e => e.Estado == true)
                .ToListAsync();

            return View("~/Views/Cita/RegistrarCita.cshtml", new RegistrarCitaDTO());
        }

        [Authorize(Roles = "Paciente")]
        [HttpPost]
        public async Task<IActionResult> RegistrarCita(RegistrarCitaDTO dto)
        {
            var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuario))
                return Unauthorized();

            var paciente = await _pacienteServicio.GetPacienteByUsuarioIdAsync(idUsuario);

            if (paciente == null)
                return NotFound();

            dto.IdPaciente = paciente.IdPaciente;

            if (!ModelState.IsValid)
            {
                ViewBag.Medicos = await _context.Medicos.Include(m => m.Usuario).ToListAsync();
                ViewBag.Especialidades = await _context.Especialidades.Where(e => e.Estado == true).ToListAsync();

                return View("~/Views/Cita/RegistrarCita.cshtml", dto);
            }

            var cita = new CACES.DAL.Entidades.Cita
            {
                IdPaciente = dto.IdPaciente,
                IdMedico = dto.IdMedico,
                IdEspecialidad = dto.IdEspecialidad,
                IdHorario = dto.IdMedico == 1 ? 1 : 2,
                Fecha = dto.FechaCita.Date,
                Hora = dto.Hora,
                Motivo = dto.Motivo,
                FechaDeRegistro = DateTime.Now,
                Estado = 1
            };

            var resultado = await _citaServicio.RegistrarCitaAsync(cita);

            if (!resultado)
            {
                TempData["Error"] = "No se pudo registrar la cita médica.";
                return RedirectToAction("RegistrarCita");
            }

            TempData["Mensaje"] = "Cita registrada correctamente.";
            return RedirectToAction("MisCitas");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Ticket(int id)
        {
            var cita = await _citaServicio.ObtenerTicketAsync(id);

            if (cita == null)
                return NotFound();

            return View("~/Views/Cita/Ticket.cshtml", cita);
        }

        [Authorize(Roles = "Medico")]
        [HttpGet]
        public async Task<IActionResult> CitasMedico()
        {
            var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuario))
                return Unauthorized();

            var medico = await _context.Medicos
                .Include(m => m.Usuario)
                .FirstOrDefaultAsync(m => m.IdUsuario == idUsuario);

            if (medico == null)
                return NotFound();

            var citas = await _citaServicio.ObtenerCitasPorMedicoAsync(medico.IdMedico);

            return View("~/Views/Cita/CitasMedico.cshtml", citas);
        }

        [Authorize(Roles = "Medico")]
        [HttpPost]
        public async Task<IActionResult> CancelarCita(int idCita)
        {
            var resultado = await _citaServicio.CancelarCitaAsync(idCita);

            TempData[resultado ? "Mensaje" : "Error"] =
                resultado ? "Cita cancelada correctamente." : "No se pudo cancelar la cita.";

            return RedirectToAction("CitasMedico");
        }

        [HttpGet]
        public async Task<IActionResult> GestionCitas()
        {
            var citas = await _citaServicio.GetCitasAsync();
            return View(citas);
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarFechaCita(int idCita, DateTime nuevaFecha)
        {
            var resultado = await _citaServicio.ActualizarFechaCitaAsync(idCita, nuevaFecha);

            TempData[resultado ? "Mensaje" : "Error"] =
                resultado ? "La fecha de la cita se actualizó correctamente." : "No se pudo actualizar la fecha de la cita.";

            return RedirectToAction("GestionCitas");
        }

        [HttpPost]
        public async Task<IActionResult> CancelarCitasPorMedicoYFecha(int idMedico, DateTime fechaCita)
        {
            var resultado = await _citaServicio.CancelarCitasPorMedicoYFechaAsync(idMedico, fechaCita);

            TempData[resultado ? "Mensaje" : "Error"] =
                resultado ? "Las citas del médico fueron canceladas correctamente." : "No se encontraron citas activas para cancelar.";

            return RedirectToAction("GestionCitas");
        }
    }
}