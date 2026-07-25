using CACES.DAL.Entidades.SeguimientoPostOperatorio;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.DTOs.SeguimientoPostOperatorio
{
    public class AtenderAlertaDTO
    {
        public int IdAlerta { get; set; }
        public int IdUsuarioAtendio { get; set; }
        public EstadoAlerta Estado { get; set; }   // Contactado o Resuelto
        public string? Observaciones { get; set; }
    }
}
