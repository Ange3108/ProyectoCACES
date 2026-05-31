using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.Servicios.ConfirmacionCorreo
{
    public interface IEmailServicio
    {
        Task EnviarCorreoAsync(string para, string asunto, string cuerpo);
    }
}
