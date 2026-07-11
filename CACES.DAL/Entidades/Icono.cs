using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.DAL.Entidades
{
    public class Icono
    {
       
            public int IdIcono { get; set; }
            public string Codigo { get; set; } = null!;
            public string Nombre { get; set; } = null!;
            public bool Activo { get; set; }

            public ICollection<Especialidad>? Especialidades { get; set; }
        
    }
}
