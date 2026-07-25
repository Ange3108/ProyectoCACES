using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CACES.DAL.Entidades.SeguimientoPostOperatorio
{
    [Table("SeguimientoPaciente")]
    public class SeguimientoPaciente
    {
        [Key]
        public int Id_Seguimiento { get; set; }
        public int Id_Cirugia { get; set; }
        public int DiaCheckpoint { get; set; }
        public DateTime FechaProgramada { get; set; }
        public EstadoSeguimiento Estado { get; set; }
        public DateTime? FechaRegistro { get; set; }

        // Navegación
        public Cirugias? Cirugia { get; set; }
        public ICollection<AlertaStaff> AlertasStaff { get; set; } = new List<AlertaStaff>();
        public ICollection<RespuestaSeguimiento> RespuestasSeguimiento { get; set; } = new List<RespuestaSeguimiento>();
    }

    public enum  EstadoSeguimiento
    {  
        Pendiente = 0,
        Completado = 1,
        Vencido = 2,
        RequiereAtencion = 3
    
    }
}
