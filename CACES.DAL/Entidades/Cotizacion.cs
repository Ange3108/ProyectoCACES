using System;
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

        public DateTime FechaSolicitud { get; set; } = DateTime.UtcNow;

        // =============================
        // COSTOS
        // =============================

        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecioBase { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal HonorariosMedico { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal CostoEquipo { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal CostoEstadia { get; set; }

        public int DiasEstadia { get; set; } = 1;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Descuento { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Impuesto { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }

        // =============================
        // INFORMACIÓN
        // =============================

        [StringLength(500)]
        public string? Observaciones { get; set; }

        /// <summary>
        /// 1 = Pendiente
        /// 2 = Enviada
        /// 3 = Aprobada
        /// 4 = Rechazada
        /// </summary>
        public byte Estado { get; set; } = 1;

        public DateTime FechaDeRegistro { get; set; } = DateTime.UtcNow;

        public DateTime? FechaDeModificacion { get; set; }

        // =============================
        // RELACIONES
        // =============================

        [ForeignKey(nameof(IdPaciente))]
        public virtual Paciente Paciente { get; set; } = null!;

        [ForeignKey(nameof(IdMedico))]
        public virtual Medico Medico { get; set; } = null!;

        [ForeignKey(nameof(IdProcedimiento))]
        public virtual Procedimiento Procedimiento { get; set; } = null!;
    }
}