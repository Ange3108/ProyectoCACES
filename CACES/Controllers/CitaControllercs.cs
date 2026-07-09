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

        // ---------- VISTAS (solo devuelven el Razor, sin datos pesados) ----------

        [Authorize(Roles = "Paciente")]
        [HttpGet]
        public IActionResult MisCitas()
        {
            return View("~/Views/Cita/MisCitas.cshtml");
        }

        [Authorize(Roles = "Paciente")]
        [HttpGet]
        public IActionResult RegistrarCita()
        {
            return View("~/Views/Cita/RegistrarCita.cshtml");
        }

        [Authorize(Roles = "Medico")]
        [HttpGet]
        public IActionResult CitasMedico()
        {
            return View("~/Views/Cita/CitasMedico.cshtml");
        }

        [HttpGet]
        public IActionResult GestionCitas()
        {
            return View("~/Views/Cita/GestionCitas.cshtml");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Ticket(int id)
        {
            var resultado = await _citaServicio.ObtenerTicketAsync(id);

            if (!resultado.EsCorrecto || resultado.Dato == null)
                return NotFound();

            return View("~/Views/Cita/Ticket.cshtml", resultado.Dato);
        }

        // ---------- ENDPOINTS JSON ----------

        [Authorize(Roles = "Paciente")]
        [HttpGet]
        public async Task<IActionResult> ObtenerMisCitas()
        {
            var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuario))
                return Unauthorized();

            var paciente = await _pacienteServicio.GetPacienteByUsuarioIdAsync(idUsuario);
            if (paciente == null)
                return NotFound();

            var resultado = await _citaServicio.ObtenerCitasPorPacienteAsync(paciente.IdPaciente);
            return Json(resultado);
        }

        [Authorize(Roles = "Paciente")]
        [HttpGet]
        public async Task<IActionResult> ObtenerMedicos()
        {
            var medicos = await _context.Medicos
                .Include(m => m.Usuario)
                .Select(m => new
                {
                    idMedico = m.IdMedico,
                    nombre = m.Usuario.Nombres + " " + m.Usuario.PrimerApellido
                })
                .ToListAsync();

            return Json(medicos);
        }

        [Authorize(Roles = "Paciente")]
        [HttpGet]
        public async Task<IActionResult> ObtenerEspecialidadesActivas()
        {
            var especialidades = await _context.Especialidades
                .Where(e => e.Estado == true)
                .Select(e => new { idEspecialidad = e.IdEspecialidad, nombre = e.Nombre })
                .ToListAsync();

            return Json(especialidades);
        }

        [Authorize(Roles = "Paciente")]
        [HttpPost]
        public async Task<IActionResult> RegistrarCita(RegistrarCitaDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuario))
                return Unauthorized();

            var paciente = await _pacienteServicio.GetPacienteByUsuarioIdAsync(idUsuario);
            if (paciente == null)
                return NotFound();

            var resultado = await _citaServicio.RegistrarCitaAsync(dto, paciente.IdPaciente);
            return Json(resultado);
        }

        [Authorize(Roles = "Medico")]
        [HttpGet]
        public async Task<IActionResult> ObtenerCitasMedico()
        {
            var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuario))
                return Unauthorized();

            var medico = await _context.Medicos.FirstOrDefaultAsync(m => m.IdUsuario == idUsuario);
            if (medico == null)
                return NotFound();

            var resultado = await _citaServicio.ObtenerCitasPorMedicoAsync(medico.IdMedico);
            return Json(resultado);
        }

        [Authorize(Roles = "Medico")]
        [HttpPost]
        public async Task<IActionResult> CancelarCita(int idCita)
        {
            var resultado = await _citaServicio.CancelarCitaAsync(idCita);
            return Json(resultado);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerListadoCitas()
        {
            var resultado = await _citaServicio.GetCitasAsync();
            return Json(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarFechaCita(int idCita, DateTime nuevaFecha)
        {
            var resultado = await _citaServicio.ActualizarFechaCitaAsync(idCita, nuevaFecha);
            return Json(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> CancelarCitasPorMedicoYFecha(int idMedico, DateTime fechaCita)
        {
            var resultado = await _citaServicio.CancelarCitasPorMedicoYFechaAsync(idMedico, fechaCita);
            return Json(resultado);
        }
    }
}