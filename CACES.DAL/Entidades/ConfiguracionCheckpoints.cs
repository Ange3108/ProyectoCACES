using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CACES.DAL.Entidades
{
    [Table("ConfiguracionCheckpoints")]
    public class ConfiguracionCheckpoints
    {
        [Key]
        public int IdCheckPoint { get; set; }
        public int DiaCheckPoint { get; set; }

        public bool Estado { get; set; }
    }
}
