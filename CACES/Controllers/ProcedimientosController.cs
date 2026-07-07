using CACES.BLL.DTOs.Procedimientos;
using CACES.BLL.Servicios.Medicos;
using CACES.BLL.Servicios.Paciente;
using CACES.BLL.Servicios.Procedimientos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Security.Claims;

namespace CACES.Controllers
{
    public class ProcedimientosController : Controller
    {
        private readonly IProcedimientosServicio _procedimientosServicio;
        private readonly IMedicoServicio _medicoServicio;
        private readonly IPacienteServicio _pacienteServicio;
        public ProcedimientosController(IProcedimientosServicio procedimientosServicio, IMedicoServicio medicoServicio, IPacienteServicio pacienteServicio)
        {
            _procedimientosServicio = procedimientosServicio;
            _medicoServicio = medicoServicio;
            _pacienteServicio = pacienteServicio;
        }



        [HttpGet]
        [Authorize(Roles = "Administrador,Medico,Paciente")]
        public async Task<IActionResult> ObtenerProcedimientos(int? idPaciente)
        {
            if (User.IsInRole("Paciente"))
            {
                var idUsuarioClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(idUsuarioClaim))
                {
                    return View("~/Views/Procedimiento/Quirurgicos.cshtml", new List<MostrarProcedimientosDTO>());
                }

                int idUsuarioLogueado = int.Parse(idUsuarioClaim);

                int idPacienteReal = await _pacienteServicio.ObtenerIdPacientePorUsuarioIdAsync(idUsuarioLogueado);

                if (idPacienteReal == 0)
                {
                    ModelState.AddModelError("", "No se encontró un perfil de paciente asociado a este usuario.");
                    return View("~/Views/Procedimiento/Quirurgicos.cshtml", new List<MostrarProcedimientosDTO>());
                }

                var procedimientosPaciente = await _procedimientosServicio.ObtenerDetalleCirugiaAsync(idPacienteReal);
                return View("~/Views/Procedimiento/Quirurgicos.cshtml", procedimientosPaciente);
            }

            if (idPaciente == null || idPaciente <= 0)
            {
                var todosLosProcedimientos = await _procedimientosServicio.ObtenerTodasLasCirugiasAsync();
                return View("~/Views/Procedimiento/Quirurgicos.cshtml", todosLosProcedimientos);
            }

            var procedimientosEspecificos = await _procedimientosServicio.ObtenerDetalleCirugiaAsync(idPaciente.Value);
            return View("~/Views/Procedimiento/Quirurgicos.cshtml", procedimientosEspecificos);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int idPaciente, int id)
        {
            var modelo = await _procedimientosServicio.ObtenerPorIdParaEditarAsync(idPaciente, id);

            if (modelo == null)
            {
                return NotFound();
            }

            ViewData["IdPaciente"] = idPaciente;

            return View("~/Views/Procedimiento/EditarProcedimientos.cshtml", modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(EditarProcedimientosDTO dto, int idPaciente)
        {
            bool exito = false;

            if (ModelState.IsValid)
            {
                exito = await _procedimientosServicio.ActualizarProcedimientoAsync(dto, idPaciente);
                if (exito)
                {
                    return RedirectToAction("ObtenerProcedimientos");
                }
            }
            var datosOriginales = await _procedimientosServicio.ObtenerPorIdParaEditarAsync(idPaciente, dto.Id_Procedimiento);

            if (datosOriginales != null)
            {
                dto.Nombre = datosOriginales.Nombre;
                dto.NombreMedico = datosOriginales.NombreMedico;
                dto.NombrePaciente = datosOriginales.NombrePaciente;
                dto.PrimerApellidoPaciente = datosOriginales.PrimerApellidoPaciente;
                dto.SegundoApellidoPaciente = datosOriginales.SegundoApellidoPaciente;
            }

            ViewData["IdPaciente"] = idPaciente;

            ModelState.AddModelError("", "No se pudieron guardar los cambios. Verifique los datos ingresados.");
            return View("~/Views/Procedimiento/EditarProcedimientos.cshtml", dto);
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Registrar(int? idPaciente)
        {


            var dto = new RegistrarProcedimientosDto
            {
                Id_Paciente = idPaciente ?? 0
            };

            var medicos = await _medicoServicio.GetEspecialistasActivosAsync();
            var procedimientos = await _procedimientosServicio.ObtenerProcedimientosFijosAsync();
            var pacientes = await _pacienteServicio.ObtenerPacientesActivosAsync();
            ViewBag.Medicos = medicos.Dato;
            ViewBag.Procedimientos = procedimientos;
            ViewBag.Pacientes = pacientes;
            return View("~/Views/Procedimiento/RegistrarProcedimiento.cshtml", dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Registrar(RegistrarProcedimientosDto dto)
        {
            if (!ModelState.IsValid)
            {
                var medicos = await _medicoServicio.GetEspecialistasActivosAsync();

                ViewBag.Medicos = medicos.Dato;

                ViewBag.Procedimientos = await _procedimientosServicio.ObtenerProcedimientosFijosAsync();

                ViewBag.Pacientes = await _pacienteServicio.ObtenerPacientesActivosAsync(); 

                return View("~/Views/Procedimiento/RegistrarProcedimiento.cshtml", dto);
            }

            bool guardadoExitoso = await _procedimientosServicio.RegistrarProcedimientoAsync(dto);

            if (guardadoExitoso)
            {
                return RedirectToAction("ObtenerProcedimientos", new { idPaciente = dto.Id_Paciente });
            }

            ModelState.AddModelError("", "No se pudo agendar el procedimiento quirúrgico. Verifique la disponibilidad del médico.");

            var medicosFallo = await _medicoServicio.GetEspecialistasActivosAsync();

            ViewBag.Medicos = medicosFallo.Dato;
            ViewBag.Procedimientos = await _procedimientosServicio.ObtenerProcedimientosFijosAsync();
            ViewBag.Pacientes = await _pacienteServicio.ObtenerPacientesActivosAsync();

            return View("~/Views/Procedimiento/RegistrarProcedimiento.cshtml", dto);
        }

        [HttpGet]
        public async Task<IActionResult> DescargarReporteProcedimiento(int id)
        {
            var resultado = await _procedimientosServicio.ObtenerDatosReporteAsync(id);

            if (!resultado.EsCorrecto || resultado.Dato == null)
            {
                return NotFound(resultado.mensaje ?? "No se encontró la información del procedimiento quirúrgico.");
            }

            var cirugia = resultado.Dato;

            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.Letter);
                    page.Margin(40); 
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11).FontColor(Colors.Grey.Darken3));

                    // HEADER
                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("SISTEMA MÉDICO CACES").FontSize(20).Bold().FontColor(Colors.Blue.Darken3);
                            col.Item().Text("Reporte Oficial de Procedimiento Quirúrgico").FontSize(12).Italic();
                        });
                    });

                    // CUERPO DEL REPORTE
                    page.Content().PaddingVertical(20).Column(col =>
                    {
                        col.Item().Text("Información General del Procedimiento").FontSize(14).Bold().FontColor(Colors.Blue.Darken2);
                        col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                        col.Item().PaddingBottom(15);

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(160);
                                columns.RelativeColumn();
                            });

                            table.Cell().PaddingVertical(5).Text("Nombre del Paciente:").Bold();
                            table.Cell().PaddingVertical(5).Text(cirugia.NombrePaciente ?? "No disponible");

                            table.Cell().PaddingVertical(5).Text("Procedimiento:").Bold();
                            table.Cell().PaddingVertical(5).Text(cirugia.Nombre ?? "No especificado");

                            table.Cell().PaddingVertical(5).Text("Fecha programada:").Bold();
                            table.Cell().PaddingVertical(5).Text(cirugia.Fecha.ToString("dd/MM/yyyy hh:mm tt"));

                            table.Cell().PaddingVertical(5).Text("Estado actual:").Bold();
                            table.Cell().PaddingVertical(5).Text(cirugia.Estado ? "Pendiente" : "Realizada / Cancelada")
                                .FontColor(cirugia.Estado ? Colors.Orange.Darken2 : Colors.Grey.Darken1).Bold();

                            table.Cell().PaddingVertical(5).Text("Médico Responsable:").Bold();
                            table.Cell().PaddingVertical(5).Text(cirugia.NombreMedico ?? "No asignado");
                        });

                        col.Item().PaddingTop(25);
                        col.Item().Text("Descripción del Procedimiento").FontSize(14).Bold().FontColor(Colors.Blue.Darken2);
                        col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                        col.Item().PaddingBottom(10);

                        col.Item().Background(Colors.Grey.Lighten4).Padding(10).Text(cirugia.Descripcion ?? "Sin indicaciones particulares.");
                    });

                    // PIE DE PÁGINA
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Página ");
                        x.CurrentPageNumber();
                        x.Span(" de ");
                        x.TotalPages();
                    });
                });
            }).GeneratePdf();

            // Retorna el archivo para descarga nativa e inmediata en el navegador
            string nombreArchivo = $"Reporte_Cirugia_{id}_ {cirugia.NombrePaciente}_{DateTime.Now:yyyyMMdd}.pdf";
            return File(pdfBytes, "application/pdf", nombreArchivo);
        }
    }
}
