using CACES.BLL.DTOs.SeguimientoPostOperatorio;
using CACES.BLL.Servicios.PreguntasPOp;
using Microsoft.AspNetCore.Mvc;

namespace CACES.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreguntasPOpController : ControllerBase
    {
        private readonly IPreguntasPOpServicio _servicio;

        public PreguntasPOpController(IPreguntasPOpServicio servicio)
        {
            _servicio = servicio;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var respuesta = await _servicio.ObtenerTodos();
            return Ok(respuesta);
        }

        [HttpGet("activas")]
        public async Task<IActionResult> ObtenerActivas()
        {
            var respuesta = await _servicio.ObtenerActivas();
            return Ok(respuesta);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var respuesta = await _servicio.ObtenerPorId(id);
            return Ok(respuesta);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(RegistrarPreguntasPOpDTO dto)
        {
            var respuesta = await _servicio.Crear(dto);
            return Ok(respuesta);
        }

        [HttpPut]
        public async Task<IActionResult> Actualizar(PreguntasPOpDTO dto)
        {
            var respuesta = await _servicio.Actualizar(dto);
            return Ok(respuesta);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var respuesta = await _servicio.Eliminar(id);
            return Ok(respuesta);
        }
    }
}
