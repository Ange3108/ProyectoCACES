using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CACES.DAL.Entidades
{
    [Table("ArchivosHistorial")]
    public class ArchivoHistorial
    {
        [Key]
        [Column("Id_Archivo")]
        public int IdArchivo { get; set; }

        [Column("Id_Historial")]
        public int IdHistorial { get; set; }

        [Required]
        [StringLength(200)]
        [Column("NombreArchivo")]
        public string NombreArchivo { get; set; } = null!;

        [Required]
        [StringLength(500)]
        [Column("RutaArchivo")]
        public string RutaArchivo { get; set; } = null!;

        [StringLength(50)]
        [Column("TipoArchivo")]
        public string? TipoArchivo { get; set; }

        [Column("FechaDeSubida")]
        public DateTime FechaDeSubida { get; set; }

        [ForeignKey(nameof(IdHistorial))]
        public virtual HistorialMedico HistorialMedico { get; set; } = null!;
    }
}