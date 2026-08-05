using CACES.BLL.DTOs.Precio;
using CACES.BLL.Servicios.Medicos;
using CACES.BLL.Servicios.Precio;
using CACES.BLL.Servicios.Procedimientos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CACES.Controllers
{
    public class PrecioController : Controller
    {
        private readonly IPrecioServicio _precioServicio;
        private readonly IMedicoServicio _medicoServicio;
        private readonly IProcedimientosServicio _procedimientosServicio;

        public PrecioController(IPrecioServicio precioServicio, IMedicoServicio medicoServicio, IProcedimientosServicio procedimientosServicio)
        {
            _precioServicio = precioServicio;
            _medicoServicio = medicoServicio;
            _procedimientosServicio = procedimientosServicio;
        }

        [HttpGet]
        public async Task<IActionResult> GestionPrecios()
        {
            try
            {
                var datos = await _precioServicio.GetAllPreciosAsync() ?? new List<MostrarPrecioDTO>();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(datos);
                }

                return View("GestionPrecios", datos);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPreciosPorMedico(int idMedico)
        {
            try
            {
                var precios = await _precioServicio.GetPreciosByMedicoAsync(idMedico) ?? new List<MostrarPrecioDTO>();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(precios);
                }

                return Json(precios);
            }
            catch (Exception)
            {
                return BadRequest(new { esCorrecto = false, mensaje = "Ocurrió un error al obtener los precios del médico." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> CrearPrecio()
        {
            var medicos = await _medicoServicio.GetEspecialistasActivosAsync();
            var procedimientos = await _procedimientosServicio.ListarProcedimientosAsync();

            ViewBag.Medicos = medicos.Dato.Select(m => new SelectListItem
            {
                Value = m.IdMedico.ToString(),
                Text = $"{m.Usuario.Nombres} {m.Usuario.PrimerApellido} {m.Usuario.SegundoApellido}"
            }).ToList();

            ViewBag.Procedimientos = procedimientos.Select(p => new SelectListItem
            {
                Value = p.Id_Procedimiento.ToString(),
                Text = p.Nombre
            }).ToList();

            return View(new RegistrarPrecioDTO());
        }

        [HttpPost]
        public async Task<IActionResult> CrearPrecio([FromBody] RegistrarPrecioDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { esCorrecto = false, mensaje = "Datos del formulario no válidos." });
            }

            try
            {
                var resultado = await _precioServicio.CreatePrecioAsync(dto);
                if (resultado == null)
                {
                    return BadRequest(new { esCorrecto = false, mensaje = "No se pudo registrar el precio." });
                }

                return Json(new { esCorrecto = true, mensaje = "Precio creado exitosamente.", dato = resultado });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { esCorrecto = false, mensaje = ex.Message });
            }
            catch (Exception)
            {
                return BadRequest(new { esCorrecto = false, mensaje = "Ocurrió un error inesperado al registrar el precio." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ActualizarPrecio(int id)
        {
            try
            {
                var precio = await _precioServicio.GetPrecioByIdAsync(id);

                if (precio == null)
                {
                    return View("Error");
                }

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(precio);
                }

                return View(precio);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarPrecio(int id, [FromBody] EditarPrecioDTO dto)
        {
            dto.IdPrecio = id;

            if (!ModelState.IsValid)
            {
                return BadRequest(new { esCorrecto = false, mensaje = "Datos no válidos." });
            }

            try
            {
                var actualizado = await _precioServicio.UpdatePrecioAsync(dto);

                if (!actualizado)
                {
                    return BadRequest(new { esCorrecto = false, mensaje = "No se pudo actualizar el precio." });
                }

                return Json(new { esCorrecto = true, mensaje = "Precio actualizado exitosamente." });
            }
            catch (Exception)
            {
                return BadRequest(new { esCorrecto = false, mensaje = "Ocurrió un error inesperado al actualizar el precio." });
            }
        }
    }
}
