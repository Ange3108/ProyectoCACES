using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.DTOs.Configuracion
{
    public class ConfiguracionDTO
    {
        public int IdConfiguracion { get; set; }
        public string Clave { get; set; }
        public string Valor { get; set; }
        public string Tipo { get; set; }
        public string Categoria { get; set; }
        public string? Descripcion { get; set; }
    }
}
