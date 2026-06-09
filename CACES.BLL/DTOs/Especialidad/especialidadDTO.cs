using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CACES.BLL.DTOs.Especialidad
{
    public class especialidadDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100)]
        public string Nombre { get; set; } = null!;
        [Required(ErrorMessage = "La descripcion es requerida")]
        [StringLength(500, ErrorMessage = "La descripcion no puede exceder los 500 caracteres")]
        public string Descripcion { get; set; } = null!;

        [Required(ErrorMessage = "Porfavor elige un icono para identificar la especialidad")]
        public String Icono { get; set; } = null!;

        public bool Estado { get; set; } = true;

    }
}
