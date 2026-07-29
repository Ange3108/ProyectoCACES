using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CACES.BLL.DTOs.SeguimientoPostOperatorio
{
    public class RegistrarConfiguracionCheckpointDTO
    {
        [Required(ErrorMessage = "El día del checkpoint es obligatorio.")]
        public int DiaCheckpoint { get; set; }
    }
}
