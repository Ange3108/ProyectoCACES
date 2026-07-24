using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.DTOs.SeguimientoPostOperatorio
{
    public class PreguntasPOpDTO
    {
        public int idPregunta { get; set; }
        public string Texto { get; set; } = null!;
        public int ValorMinimo { get; set; }
        public int ValorMaximo { get; set; }
        public int UmbralAlerta { get; set; }
        public string DireccionAlerta { get; set; } = null!; // se muestra como texto legible en UI
        public bool Estado { get; set; }
    }
}
