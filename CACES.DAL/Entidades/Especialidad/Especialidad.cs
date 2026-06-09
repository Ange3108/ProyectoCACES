using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CACES.DAL.Entidades.Especialidad
{
    [Table("Especialidad")]
    public partial class Especialidad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Especialidad {  get; set; }
        public string Nombre { get; set; } = null!;
        public string Descripción { get; set; } = null!;

        public string Icono { get; set; } = null!;

        public string FechaDeRegistro { get; set; } = null!;

        public bool Estado { get; set; } = true;
    }
}
