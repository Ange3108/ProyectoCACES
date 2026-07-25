using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.DTOs.SeguimientoPostOperatorio
{
    public class MostrarRespuestaSeguimientoDTO
    {
        public int idRespuesta { get; set; }
        public int IdSeguimiento { get; set; }
        public int IdPregunta { get; set; }
        public string TextoPregunta { get; set; } = null!;
        public int ValorRespuesta { get; set; }
        public bool GeneroAlerta { get; set; }
    }
}
