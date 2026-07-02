using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CACES.DAL.Entidades
{
    [Table("Noticias")]
    public class Noticia
    {
        [Key]
        [Column("Id_Noticia")]
        public int IdNoticia { get; set; }

        [Column("Titulo")]
        public string Titulo { get; set; } = null!;

        [Column("Contenido")]
        public string Contenido { get; set; } = null!;

        [Column("FechaDePublicacion")]
        public DateTime FechaDePublicacion { get; set; }

        [Column("FechaDeModificacion")]
        public DateTime? FechaDeModificacion { get; set; }

        [Column("Imagen")]
        public string? Imagen { get; set; }

        [Column("Estado")]
        public bool Estado { get; set; }
    }
}