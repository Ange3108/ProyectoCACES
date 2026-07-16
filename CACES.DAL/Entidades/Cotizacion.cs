using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CACES.DAL.Entidades
{
    [Table("Cotizacion")]
    public class Cotizacion
    {
        [Key]
        [Column("Id_Cotizacion")]
        public int IdCotizacion { get; set; }

        [Column("Id_Paciente")]
        public int IdPaciente { get; set; }

        [Column("Id_Medico")]
        public int IdMedico { get; set; }

        [Column("Id_Procedimiento")]
        public int IdProcedimiento { get; set; }

        public DateTime FechaSolicitud { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecioBase { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Descuento { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Impuesto { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }

        [StringLength(500)]
        public string? Observaciones { get; set; }

        public byte Estado { get; set; } = 1;

        public DateTime FechaDeRegistro { get; set; } = DateTime.Now;

        public DateTime? FechaDeModificacion { get; set; }

        [ForeignKey(nameof(IdPaciente))]
        public virtual Paciente Paciente { get; set; } = null!;

        [ForeignKey(nameof(IdMedico))]
        public virtual Medico Medico { get; set; } = null!;

        [ForeignKey(nameof(IdProcedimiento))]
        public virtual Procedimiento Procedimiento { get; set; } = null!;
    }
}