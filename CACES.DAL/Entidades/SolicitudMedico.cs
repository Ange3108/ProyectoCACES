using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CACES.DAL.Entidades
{
    [Table("SolicitudMedico")]
    public class SolicitudMedico
    {
        [Key]
        [Column("Id_Solicitud")]
        public int IdSolicitud { get; set; }

        [Required]
        [StringLength(80)]
        public string Nombres { get; set; } = string.Empty;

        [Required]
        [StringLength(60)]
        public string PrimerApellido { get; set; } = string.Empty;

        [StringLength(60)]
        public string? SegundoApellido { get; set; }

        [Required]
        [StringLength(120)]
        [EmailAddress]
        public string CorreoElectronico { get; set; } = string.Empty;

        [Required]
        [StringLength(25)]
        public string Telefono { get; set; } = string.Empty;

        [Column("Id_Especialidad")]
        public int IdEspecialidad { get; set; }

        [Range(0, 60)]
        public int AniosExperiencia { get; set; }

        [StringLength(500)]
        public string? Certificaciones { get; set; }

        [Required]
        [StringLength(500)]
        public string Motivo { get; set; } = string.Empty;

        [StringLength(250)]
        public string? Curriculum { get; set; }

        [StringLength(250)]
        public string? Foto { get; set; }

        /// <summary>
        /// 1 = Pendiente
        /// 2 = En revisión
        /// 3 = Aprobada
        /// 4 = Rechazada
        /// </summary>
        public byte Estado { get; set; } = 1;

        [StringLength(500)]
        public string? ObservacionAdministrador { get; set; }

        public DateTime FechaSolicitud { get; set; } = DateTime.UtcNow;

        public DateTime? FechaRespuesta { get; set; }

        [ForeignKey(nameof(IdEspecialidad))]
        public virtual Especialidad Especialidad { get; set; } = null!;
    }
}