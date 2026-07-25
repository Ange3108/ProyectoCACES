using Microsoft.AspNetCore.Mvc;

namespace CACES.Controllers
{
    public class GestionSeguimientoController : Controller
    {
        
        [HttpGet]
        public IActionResult GestionSeguimientoPostOperatorio()
        {
            return View();
        }
    }
}
