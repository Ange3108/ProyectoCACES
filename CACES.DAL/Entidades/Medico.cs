using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CACES.DAL.Entidades
{
    [Table("Medicos")]
    public partial class Medico
    {
        [Key]
        [Column("Id_Medico")]
        public int IdMedico { get; set; }

        [Column("Id_Especialidad")]
        public int IdEspecialidad { get; set; }

        [Column("Id_Usuario")]
        public int IdUsuario { get; set; }

        [Column("Experiencia")]
        public int Experiencia { get; set; }

        [Column("Certificaciones")]
        public string Certificaciones { get; set; } = null!;

        [Column("FechaDeRegistro")]
        public DateTime FechaDeRegistro { get; set; }

        public virtual Usuario Usuario { get; set; } = null!;

        public virtual Especialidad Especialidad { get; set; } = null!;

        public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();

        public virtual ICollection<HorariosDisponibles> HorariosDisponibles { get; set; } = new List<HorariosDisponibles>();

        public virtual ICollection<Cirugias> Cirugias { get; set; } = new List<Cirugias>();

        public virtual ICollection<Precios> Precios { get; set; } = new List<Precios>();
    }
}