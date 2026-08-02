using CACES.BLL.DTOs.Especialidad;
using CACES.BLL.DTOs.Procedimientos;
using CACES.BLL.Servicios.Especialidad;
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
        private readonly IEspecialidadServicio _especialidadServicio;
        public ProcedimientosController(IProcedimientosServicio procedimientosServicio, IMedicoServicio medicoServicio, IPacienteServicio pacienteServicio, IEspecialidadServicio especialidadServicio)
        {
            _procedimientosServicio = procedimientosServicio;
            _medicoServicio = medicoServicio;
            _pacienteServicio = pacienteServicio;
            _especialidadServicio = especialidadServicio;
        }



        //[HttpGet]
        //public async Task<IActionResult> DescargarReporteProcedimiento(int id)
        //{
        //    var resultado = await _procedimientosServicio.ObtenerDatosReporteAsync(id);

        //    if (!resultado.EsCorrecto || resultado.Dato == null)
        //    {
        //        return NotFound(resultado.mensaje ?? "No se encontró la información del procedimiento quirúrgico.");
        //    }

        //    var cirugia = resultado.Dato;

        //    var pdfBytes = Document.Create(container =>
        //    {
        //        container.Page(page =>
        //        {
        //            page.Size(PageSizes.Letter);
        //            page.Margin(40); 
        //            page.PageColor(Colors.White);
        //            page.DefaultTextStyle(x => x.FontSize(11).FontColor(Colors.Grey.Darken3));

        //            // HEADER
        //            page.Header().Row(row =>
        //            {
        //                row.RelativeItem().Column(col =>
        //                {
        //                    col.Item().Text("SISTEMA MÉDICO CACES").FontSize(20).Bold().FontColor(Colors.Blue.Darken3);
        //                    col.Item().Text("Reporte Oficial de Procedimiento Quirúrgico").FontSize(12).Italic();
        //                });
        //            });

        //            // CUERPO DEL REPORTE
        //            page.Content().PaddingVertical(20).Column(col =>
        //            {
        //                col.Item().Text("Información General del Procedimiento").FontSize(14).Bold().FontColor(Colors.Blue.Darken2);
        //                col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
        //                col.Item().PaddingBottom(15);

        //                col.Item().Table(table =>
        //                {
        //                    table.ColumnsDefinition(columns =>
        //                    {
        //                        columns.ConstantColumn(160);
        //                        columns.RelativeColumn();
        //                    });

        //                    table.Cell().PaddingVertical(5).Text("Nombre del Paciente:").Bold();
        //                    table.Cell().PaddingVertical(5).Text(cirugia.NombrePaciente ?? "No disponible");

        //                    table.Cell().PaddingVertical(5).Text("Procedimiento:").Bold();
        //                    table.Cell().PaddingVertical(5).Text(cirugia.Nombre ?? "No especificado");

        //                    table.Cell().PaddingVertical(5).Text("Fecha programada:").Bold();
        //                    table.Cell().PaddingVertical(5).Text(cirugia.Fecha.ToString("dd/MM/yyyy hh:mm tt"));

        //                    table.Cell().PaddingVertical(5).Text("Estado actual:").Bold();
        //                    table.Cell().PaddingVertical(5).Text(cirugia.Estado ? "Pendiente" : "Realizada / Cancelada")
        //                        .FontColor(cirugia.Estado ? Colors.Orange.Darken2 : Colors.Grey.Darken1).Bold();

        //                    table.Cell().PaddingVertical(5).Text("Médico Responsable:").Bold();
        //                    table.Cell().PaddingVertical(5).Text(cirugia.NombreMedico ?? "No asignado");
        //                });

        //                col.Item().PaddingTop(25);
        //                col.Item().Text("Descripción del Procedimiento").FontSize(14).Bold().FontColor(Colors.Blue.Darken2);
        //                col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
        //                col.Item().PaddingBottom(10);

        //                col.Item().Background(Colors.Grey.Lighten4).Padding(10).Text(cirugia.Descripcion ?? "Sin indicaciones particulares.");
        //            });

        //            // PIE DE PÁGINA
        //            page.Footer().AlignCenter().Text(x =>
        //            {
        //                x.Span("Página ");
        //                x.CurrentPageNumber();
        //                x.Span(" de ");
        //                x.TotalPages();
        //            });
        //        });
        //    }).GeneratePdf();

        //    // Retorna el archivo para descarga nativa e inmediata en el navegador
        //    string nombreArchivo = $"Reporte_Cirugia_{id}_ {cirugia.NombrePaciente}_{DateTime.UtcNow:yyyyMMdd}.pdf";
        //    return File(pdfBytes, "application/pdf", nombreArchivo);
        //}

        [HttpGet]
        public async Task<IActionResult> ObtenerProcedimientosQuirur()
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest" || Request.Headers["Accept"].ToString().Contains("application/json"))
            {
                var procedimientos = await _procedimientosServicio.ListarProcedimientosAsync();
                return Json(new { exito = true, datos = procedimientos });
            }

            return View("~/Views/Procedimiento/ListarProcedimientos.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            var resultadoEspecialidades = await _especialidadServicio.GetEspecialidadesActivasAsync();

            if (resultadoEspecialidades != null && resultadoEspecialidades.EsCorrecto)
            {
                ViewBag.Especialidades = resultadoEspecialidades.Dato;
            }
            else
            {
                ViewBag.Especialidades = new List<mostrarEspecialidadDTO>();
                TempData["Error"] = "No se pudieron cargar las especialidades.";
            }

            return View("~/Views/Procedimiento/InsertarProcedimiento.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear([FromBody] InsertarProcedimientosDto modelo)
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return Json(new { exito = false, errores });
            }

            var resExito = await _procedimientosServicio.GuardarProcedimientoAsync(modelo);

            if (resExito)
            {
                return Json(new { exito = true, mensaje = "Procedimiento registrado exitosamente." });
            }

            return Json(new { exito = false, mensaje = "No se pudo registrar. Ya existe un procedimiento con ese mismo nombre en la especialidad seleccionada." });
        }

        [HttpGet]
        public async Task<IActionResult> EditarProcEnReportes(int id)
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var modelo = await _procedimientosServicio.ObtenerPorIdAsync(id);
                if (modelo == null)
                {
                    return Json(new { exito = false, mensaje = "El procedimiento solicitado no existe." });
                }

                var resultadoEspecialidades = await _especialidadServicio.GetEspecialidadesActivasAsync();

                return Json(new
                {
                    exito = true,
                    procedimiento = modelo,
                    especialidades = resultadoEspecialidades?.Dato
                });
            }

            ViewBag.IdProcedimiento = id;
            return View("~/Views/Procedimiento/EditarProcedimiento.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarProcEnReportes([FromBody] InsertarProcedimientosDto modelo)
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return Json(new { exito = false, errores });
            }

            var resExito = await _procedimientosServicio.EditarProcedimientoAdminAsync(modelo);

            if (resExito)
            {
                return Json(new { exito = true, mensaje = "El procedimiento fue actualizado exitosamente." });
            }

            return Json(new { exito = false, mensaje = "No se pudo actualizar. Ya existe otro procedimiento con ese mismo nombre en la especialidad seleccionada." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(int id)
        {
            var resExito = await _procedimientosServicio.CambiarEstadoProcedimientoAsync(id);

            if (resExito)
            {
                return Json(new { exito = true, mensaje = "El estado del procedimiento se actualizó correctamente." });
            }

            return Json(new { exito = false, mensaje = "No se pudo modificar el estado del procedimiento." });
        }
    }
}
