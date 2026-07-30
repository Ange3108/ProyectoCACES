using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CACES.DAL.Entidades
{
    [Table("ConfiguracionCotizacion")]
    public class ConfiguracionCotizacion
    {
        [Key]
        [Column("Id_Configuracion")]
        public int IdConfiguracion { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal PorcentajeEquipo { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal CostoEstadiaDiaria { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal PorcentajeImpuesto { get; set; }

        public bool Estado { get; set; } = true;

        public DateTime FechaDeRegistro { get; set; } = DateTime.Now;

        public DateTime? FechaDeModificacion { get; set; }
    }
}