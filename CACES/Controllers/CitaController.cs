using CACES.BLL.DTOs.Cita;
using CACES.BLL.Servicios.Citas;
using CACES.BLL.Servicios.Paciente;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace CACES.Controllers
{
    public class CitaController : Controller
    {
        private readonly ICitaServicio _citaServicio;
        private readonly IPacienteServicio _pacienteServicio;

        public CitaController(
            ICitaServicio citaServicio,
            IPacienteServicio pacienteServicio)
        {
            _citaServicio = citaServicio;
            _pacienteServicio = pacienteServicio;
        }

        [Authorize(Roles = "Paciente")]
        [HttpGet]
        public IActionResult RegistrarCita()
        {
            return View();
        }

        [Authorize(Roles = "Paciente")]
        [HttpGet]
        public IActionResult MisCitas()
        {
            return View();
        }

        [Authorize(Roles = "Medico,Administrador")]
        [HttpGet]
        public IActionResult GestionCitas()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Ticket(int id)
        {
            var resultado = await _citaServicio.ObtenerTicketAsync(id);

            if (!resultado.EsCorrecto || resultado.Dato == null)
                return NotFound();

            return View(resultado.Dato);
        }

        [Authorize(Roles = "Paciente")]
        [HttpPost]
        public async Task<IActionResult> RegistrarCitaJson(
            [FromBody] RegistrarCitaDTO dto)
        {
            var idUsuario = ObtenerIdUsuarioActual();

            if (idUsuario == null)
                return Unauthorized();

            var paciente =
                await _pacienteServicio.GetPacienteByUsuarioIdAsync(
                    idUsuario.Value
                );

            if (paciente == null)
                return NotFound();

            var resultado = await _citaServicio.RegistrarCitaAsync(
                dto,
                paciente.IdPaciente
            );

            return Json(resultado);
        }

        [Authorize(Roles = "Paciente")]
        [HttpGet]
        public async Task<IActionResult> ObtenerMisCitas()
        {
            var idUsuario = ObtenerIdUsuarioActual();

            if (idUsuario == null)
                return Unauthorized();

            var paciente =
                await _pacienteServicio.GetPacienteByUsuarioIdAsync(
                    idUsuario.Value
                );

            if (paciente == null)
                return NotFound();

            var resultado =
                await _citaServicio.ObtenerCitasPorPacienteAsync(
                    paciente.IdPaciente
                );

            return Json(resultado);
        }

        [Authorize(Roles = "Medico")]
        [HttpGet]
        public async Task<IActionResult> ObtenerCitasMedico()
        {
            var idUsuario = ObtenerIdUsuarioActual();

            if (idUsuario == null)
                return Unauthorized();

            var resultado =
                await _citaServicio.ObtenerCitasPorUsuarioMedicoAsync(
                    idUsuario.Value
                );

            return Json(resultado);
        }

        [Authorize(Roles = "Administrador,Medico")]
        [HttpGet]
        public async Task<IActionResult> ObtenerListadoCitas()
        {
            return Json(await _citaServicio.GetCitasAsync());
        }

        [Authorize(Roles = "Paciente,Medico,Administrador")]
        [HttpPost]
        public async Task<IActionResult> CancelarCita(int idCita)
        {
            return Json(
                await _citaServicio.CancelarCitaAsync(idCita)
            );
        }

        [Authorize(Roles = "Administrador,Medico")]
        [HttpPost]
        public async Task<IActionResult> ActualizarFechaCita(
            int idCita,
            DateTime nuevaFecha)
        {
            return Json(
                await _citaServicio.ActualizarFechaCitaAsync(
                    idCita,
                    nuevaFecha
                )
            );
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> CancelarCitasPorMedicoYFecha(
            int idMedico,
            DateTime fechaCita)
        {
            return Json(
                await _citaServicio.CancelarCitasPorMedicoYFechaAsync(
                    idMedico,
                    fechaCita
                )
            );
        }

        [Authorize(Roles = "Paciente,Administrador")]
        [HttpGet]
        public async Task<IActionResult> ObtenerMedicos()
        {
            return Json(
                await _citaServicio.ObtenerMedicosAsync()
            );
        }

        [Authorize(Roles = "Paciente,Administrador")]
        [HttpGet]
        public async Task<IActionResult> ObtenerEspecialidadesActivas()
        {
            return Json(
                await _citaServicio.ObtenerEspecialidadesActivasAsync()
            );
        }

        [Authorize(Roles = "Paciente,Administrador")]
        [HttpGet]
        public async Task<IActionResult> ObtenerHorariosPorMedico(
            int idMedico)
        {
            return Json(
                await _citaServicio.ObtenerHorariosPorMedicoAsync(
                    idMedico
                )
            );
        }

        private int? ObtenerIdUsuarioActual()
        {
            var claimId = User
                .FindFirst(ClaimTypes.NameIdentifier)?
                .Value;

            return int.TryParse(claimId, out var idUsuario)
                ? idUsuario
                : null;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DescargarTicketPdf(int id)
        {
            var resultado = await _citaServicio.ObtenerTicketAsync(id);

            if (!resultado.EsCorrecto || resultado.Dato == null)
                return NotFound();

            var cita = resultado.Dato;

            var pdf = Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().Column(header =>
                    {
                        header.Item()
                            .Background(Colors.Blue.Darken3)
                            .Padding(18)
                            .AlignCenter()
                            .Text("CACES")
                            .FontSize(28)
                            .Bold()
                            .FontColor(Colors.White);

                        header.Item()
                            .PaddingTop(6)
                            .AlignCenter()
                            .Text("Centro Ambulatorio de Cirugía Escalón")
                            .FontSize(13)
                            .FontColor(Colors.Grey.Darken1);
                    });

                    page.Content().PaddingVertical(25).Column(col =>
                    {
                        col.Spacing(14);

                        col.Item()
                            .AlignCenter()
                            .Text("TICKET DE CITA MÉDICA")
                            .FontSize(20)
                            .Bold()
                            .FontColor(Colors.Blue.Darken3);

                        col.Item()
                            .AlignCenter()
                            .Text($"Ticket N.º {cita.IdCita:D6}")
                            .FontSize(14)
                            .SemiBold();

                        col.Item()
                            .Border(1)
                            .BorderColor(Colors.Grey.Lighten2)
                            .Padding(20)
                            .Column(datos =>
                            {
                                datos.Spacing(12);

                                datos.Item().Text(texto =>
                                {
                                    texto.Span("Paciente: ").Bold();
                                    texto.Span(cita.NombrePaciente);
                                });

                                datos.Item().Text(texto =>
                                {
                                    texto.Span("Médico: ").Bold();
                                    texto.Span(cita.NombreMedico);
                                });

                                datos.Item().Text(texto =>
                                {
                                    texto.Span("Especialidad: ").Bold();
                                    texto.Span(cita.NombreEspecialidad);
                                });

                                datos.Item().Text(texto =>
                                {
                                    texto.Span("Fecha de la cita: ").Bold();
                                    texto.Span(cita.FechaCita.ToString("dd/MM/yyyy"));
                                });

                                datos.Item().Text(texto =>
                                {
                                    texto.Span("Hora: ").Bold();
                                    texto.Span(cita.Hora.ToString(@"hh\:mm"));
                                });

                                datos.Item().Text(texto =>
                                {
                                    texto.Span("Motivo: ").Bold();
                                    texto.Span(cita.Motivo);
                                });

                                datos.Item().Text(texto =>
                                {
                                    texto.Span("Estado: ").Bold();

                                    texto.Span(cita.EstadoTexto)
                                        .Bold()
                                        .FontColor(cita.Estado == 1
                                            ? Colors.Green.Darken2
                                            : Colors.Red.Darken2);
                                });

                                datos.Item().Text(texto =>
                                {
                                    texto.Span("Fecha de emisión: ").Bold();
                                    texto.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                                });
                            });

                        col.Item()
                            .PaddingTop(10)
                            .AlignCenter()
                            .Text("Presente este comprobante el día de su cita médica.")
                            .Italic()
                            .FontColor(Colors.Grey.Darken1);

                        col.Item()
                            .PaddingTop(35)
                            .Text("Firma del paciente: ______________________________");
                    });

                    page.Footer().Column(footer =>
                    {
                        footer.Item()
                            .LineHorizontal(1)
                            .LineColor(Colors.Blue.Darken3);

                        footer.Item()
                            .PaddingTop(8)
                            .AlignCenter()
                            .Text("CACES - Centro Ambulatorio de Cirugía Escalón")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken1);
                    });
                });
            }).GeneratePdf();

            return File(
                pdf,
                "application/pdf",
                $"Ticket_Cita_{cita.IdCita:D6}.pdf"
            );
        }

    }
}