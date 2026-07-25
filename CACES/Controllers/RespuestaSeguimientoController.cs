using CACES.BLL.DTOs.SeguimientoPostOperatorio;
using CACES.BLL.Servicios.RespuestaSeguimiento;
using Microsoft.AspNetCore.Mvc;

namespace CACES.Controllers
{
    public class RespuestaSeguimientoController : Controller
    {
       
            private readonly IRespuestaSeguimientoServicio _servicio;

            public RespuestaSeguimientoController(IRespuestaSeguimientoServicio servicio)
            {
                _servicio = servicio;
            }

        [HttpGet]
        public async Task<ActionResult> ObtenerTodas()
        {
            var resultado = await _servicio.ObtenerTodas();
            return Json(resultado);
        }
        [HttpGet]
            public async Task<ActionResult> ObtenerPorSeguimiento(int idSeguimiento)
            {
                var resultado = await _servicio.ObtenerPorSeguimiento(idSeguimiento);
                return Json(resultado);
            }

            [HttpPost]
            public async Task<ActionResult> RegistrarRespuestas([FromBody] List<RegistrarRespuestaSeguimientoDTO> respuestas)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var resultado = await _servicio.RegistrarRespuestas(respuestas);
                if (!resultado.EsCorrecto)
                {
                    ModelState.AddModelError(string.Empty, resultado.mensaje ?? "No se pudieron registrar las respuestas.");
                    return BadRequest(resultado);
                }

                return Json(resultado);
            }
        }
    }

