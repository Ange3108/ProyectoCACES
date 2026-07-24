using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CACES.DAL.Entidades.SeguimientoPostOperatorio
{
    [Table("PreguntaSeguimiento")]
    public class PreguntaSeguimiento
    {
        [Key]
        public int IdPregunta { get; set; }

        public string Texto { get; set; } = null!;

        public int ValorMinimo { get; set; }

        public int ValorMaximo { get; set; }

        public int UmbralAlerta { get; set; }

        public DireccionAlerta DireccionAlerta { get; set; }

        public bool Estado { get; set; }
    }

    public enum DireccionAlerta
    {
        MayorIgual = 0,
        MenorIgual = 1
    }
}
