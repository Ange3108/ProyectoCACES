using CACES.BLL.Servicios.Medicos;
using CACES.DAL.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace CACES.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicoController : ControllerBase
    {
        private readonly IMedicoServicio _medicoServicio;

        public MedicoController(IMedicoServicio medicoServicio)
        {
            _medicoServicio = medicoServicio;
        }

        [HttpGet]
        public async Task<IActionResult> GetMedicos()
        {
            var lista = await _medicoServicio.GetMedicosAsync();

            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMedicoById(int id)
        {
            var medico = await _medicoServicio.GetMedicoByIdAsync(id);

            if (medico == null)
                return NotFound();

            return Ok(medico);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMedico([FromBody] Medico medico)
        {
            var result = await _medicoServicio.UpdateMedicoAsync(medico);

            if (!result)
                return BadRequest();

            return Ok();
        }
    }
}