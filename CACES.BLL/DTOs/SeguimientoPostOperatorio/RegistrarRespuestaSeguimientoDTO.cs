using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CACES.BLL.DTOs.SeguimientoPostOperatorio
{
    public class RegistrarRespuestaSeguimientoDTO
    {
        [Required(ErrorMessage = "El campo seguimiemto es obligatorio.")]
        public int IdSeguimiento { get; set; }
        [Required(ErrorMessage = "La pregunta es obligatorio.")]
        public int IdPregunta { get; set; }
        [Required(ErrorMessage = "El valor de la respuesta es obligatorio.")]
        public int ValorRespuesta { get; set; }

    }
}
