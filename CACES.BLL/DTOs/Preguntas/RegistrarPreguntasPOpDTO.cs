using CACES.DAL.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.DTOs.Preguntas
{
    public class RegistrarPreguntasPOpDTO
    {
        public string Texto { get; set; } = null!;
        public int ValorMinimo { get; set; }
        public int ValorMaximo { get; set; }
        public int UmbralAlerta { get; set; }
        public DireccionAlerta DireccionAlerta { get; set; }
    }
}
