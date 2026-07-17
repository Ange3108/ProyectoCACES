using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CACES.DAL.Entidades
{
    [Table("Especialidad")]
    public partial class Especialidad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id_Especialidad")]
        public int IdEspecialidad { get; set; }

        [Column("Nombre")]
        public string Nombre { get; set; } = null!;

        [Column("Descripcion")]
        public string Descripcion { get; set; } = null!;

        public int IdIcono { get; set; } 


        [Column("FechaDeRegistro")]
        public DateTime FechaDeRegistro { get; set; }

        [Column("Estado")]
        public bool Estado { get; set; }

        public virtual ICollection<Medico> Medicos { get; set; } = new List<Medico>();

        public virtual ICollection<Procedimiento> Procedimientos { get; set; } = new List<Procedimiento>();


        public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();

        public virtual Icono Icono { get; set; } = null!;


    }
}