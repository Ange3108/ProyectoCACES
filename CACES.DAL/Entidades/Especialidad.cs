using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CACES.DAL.Entidades
{
    [Table("Especialidad")]
    public partial class Especialidad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdEspecialidad {  get; set; }
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;

        public string Icono { get; set; } = null!;

        public DateTime  FechaDeRegistro { get; set; } 

        public bool Estado { get; set; } = true;
    }
}
