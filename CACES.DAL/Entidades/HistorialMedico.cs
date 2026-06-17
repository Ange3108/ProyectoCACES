using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CACES.DAL.Entidades
{
    [Table("Historial_Medico")]
    public partial class HistorialMedico
    {
        [Key]
        [Column("Id_Historial")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdHistorial { get; set; }

        [Required(ErrorMessage = "Las alergias son requeridas")]
        [StringLength(200)]
        [Column("Alergias")]
        public string Alergias { get; set; } = null!;

        [Required(ErrorMessage = "Las enfermedades crónicas son requeridas")]
        [StringLength(200)]
        [Column("Enfermedades_Crónicas")]
        public string EnfermedadesCronicas { get; set; } = null!;

        [Required(ErrorMessage = "Los detalles son requeridos")]
        [StringLength(100)]
        [Column("Detalles")]
        public string Detalles { get; set; } = null!;

        [Required(ErrorMessage = "El tipo de sangre es requerido")]
        [StringLength(10)]
        [Column("Tipo_Sangre")]
        public string TipoSangre { get; set; } = null!;

        [Required(ErrorMessage = "Los medicamentos son requeridos en caso de que no tome ninguno indicar que no")]
        [StringLength(200)]
        [Column("Medicamentos")]
        public string Medicamentos { get; set; } = null!;

        [Required(ErrorMessage = "Los antecedentes son requeridos")]
        [StringLength(50)]
        [Column("Antecedentes")]
        public string Antecedentes { get; set; } = null!;

        [Column("FechaDeCreacion")]
        public DateTime FechaDeCreacion { get; set; } = DateTime.Now;

        [Column("FechaDeModificacion")]
        public DateTime? FechaDeModificacion { get; set; }


    }
}
