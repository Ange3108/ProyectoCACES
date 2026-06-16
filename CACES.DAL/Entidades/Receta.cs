using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CACES.DAL.Entidades
{
    [Table("Recetas")]
    public class Receta
    {
            [Key]
            [Column("Id_Receta")]
            public int IdReceta { get; set; }

            [Column("Id_Cita")]
            public int IdCita { get; set; }

            [Required]
            public string Medicamentos { get; set; } = null!;

            public string? Instrucciones { get; set; }

            public DateTime FechaDeRegistro { get; set; }

            public DateTime FechaDeVencimiento { get; set; }

            // Propiedad de navegación hacia la Cita
            public virtual Cita Cita { get; set; } = null!;
        }
    }
