using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.DTOs.Convenios
{
    public class MostrarConvenios
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal? DescuentoPorcentaje { get; set; }
        public string? ContactoTelefono { get; set; }
        public string? ImagenUrl { get; set; }
        public bool Estado { get; set; }
    }
}
