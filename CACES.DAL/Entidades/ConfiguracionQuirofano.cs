using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CACES.DAL.Entidades
{
    public class ConfiguracionQuirofano
    {
        [Key]
        public int Id { get; set; }
        public int CupoMaximoDiario { get; set; }
    }
}
