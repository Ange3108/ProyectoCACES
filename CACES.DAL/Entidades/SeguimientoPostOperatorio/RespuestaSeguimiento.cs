using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CACES.DAL.Entidades.SeguimientoPostOperatorio
{
    [Table("RespuestaSeguimiento")]
    public class RespuestaSeguimiento
    {
        [Key]
        public int IdRespuesta { get; set; }
        public int IdSeguimiento { get; set; }
        public int IdPregunta { get; set; }
        public int ValorRespuesta { get; set; }
        public bool GeneroAlerta { get; set; }

        // Navegación
        public SeguimientoPaciente? SeguimientoPaciente { get; set; }
        public PreguntaSeguimiento? PreguntaSeguimiento { get; set; }
    }
}
