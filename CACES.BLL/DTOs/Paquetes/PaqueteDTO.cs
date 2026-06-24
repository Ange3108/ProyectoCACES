using System.ComponentModel.DataAnnotations;

namespace CACES.BLL.DTOs.Paquete
{
    public class PaqueteDTO
    {
        //Para todas las acciones de turimo medico
        public int IdPaquete { get; set; }
        [Required(ErrorMessage = "El nombre del paquete es requerido")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "La descripción del paquete es requerida")]
        public string Descripcion { get; set; } = null!;

        [Required(ErrorMessage = "La duración del paquete es requerida")]
        public string Duracion { get; set; } = null!;

        [Required(ErrorMessage = "El precio del paquete es requerido")]
        public decimal Precio { get; set; }

        public bool Estado { get; set; }


    }
}
