using CACES.DAL.Entidades.SeguimientoPostOperatorio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CACES.BLL.DTOs.SeguimientoPostOperatorio
{
    public class RegistrarPreguntasPOpDTO
    {
        [Required(ErrorMessage = "El mensaje de la pregunta es obligatorio.")]
        public string Texto { get; set; } = null!;
        [Required(ErrorMessage = "El valor mínimo de la pregunta es obligatorio.")]
        public int ValorMinimo { get; set; }
        [Required(ErrorMessage = "El valor máximo de la pregunta es obligatorio.")]
        public int ValorMaximo { get; set; }
        [Required(ErrorMessage = "El umbral de alerta de la pregunta es obligatorio.")]
        public int UmbralAlerta { get; set; }
        [Required(ErrorMessage = "La dirección de alerta de la pregunta es obligatoria.")]
        public DireccionAlerta DireccionAlerta { get; set; }
    }
}
