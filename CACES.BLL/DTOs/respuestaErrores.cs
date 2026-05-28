using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.DTOs
{
    public class respuestaErrores<T>
    {
        public bool EsCorrecto { get; set; } 
        public string mensaje { get; set; } = string.Empty;
        public T Dato { get; set; }
        public int codigoError { get; set; } 

        public respuestaErrores() 
        { 
            EsCorrecto = true; 
            mensaje = "Operación realizada correctamente.";
            codigoError = 200;
        }

    }
}
