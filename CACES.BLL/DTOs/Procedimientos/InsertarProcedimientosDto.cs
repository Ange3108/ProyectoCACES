using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CACES.BLL.DTOs.Procedimientos
{
    public class InsertarProcedimientosDto
    {
        [Required]
        public int Id_Procedimiento { get; set; }
        public int Id_Especialidad { get; set; }

        public string? NombreEspecialidad { get; set; }

        [Required(ErrorMessage = "El nombre del procedimiento es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Nombre { get; set; } = null!;

        [StringLength(200, ErrorMessage = "La descripción no puede superar los 200 caracteres.")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El precio base es obligatorio.")]
        [Range(0, 99999999.99, ErrorMessage = "El precio debe ser un valor positivo.")]
        public decimal PrecioBase { get; set; }

        public bool Estado { get; set; } = true;
    }
}
