using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CACES.DAL.Entidades
{
    [Table("Icono")]
    public class Icono
    {
       
            public int IdIcono { get; set; }
            public string Codigo { get; set; } = null!;
            public string Nombre { get; set; } = null!;

            public ICollection<Especialidad>? Especialidades { get; set; }
        
    }
}
