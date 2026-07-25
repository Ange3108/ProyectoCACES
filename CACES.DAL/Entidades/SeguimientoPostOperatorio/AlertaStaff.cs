using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CACES.DAL.Entidades.SeguimientoPostOperatorio
{
    [Table("AlertaStaff")]
    public class AlertaStaff
    {
        [Key]
        public int IdAlerta { get; set; }

        public int IdSeguimiento { get; set; }

        public DateTime FechaGenerada { get; set; }

        public EstadoAlerta Estado { get; set; }

        public int? IdUsuarioAtendio { get; set; }

        public string? Observaciones { get; set; }

        public DateTime? FechaAtencion { get; set; }

        // Navegación
        public SeguimientoPaciente? SeguimientoPaciente { get; set; }
        public Usuario? UsuarioAtendio { get; set; }
    }
    public enum EstadoAlerta
    {
        Pendiente = 0,
        Contactado = 1,
        Resuelto = 2
    }
}
