using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CACES.BLL.Servicios.ConfirmacionCorreo
{
    public class EmailServicio : IEmailServicio
    {
        private readonly IConfiguration _configuration;

        // Inyectamos el archivo de configuración
        public EmailServicio(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task EnviarCorreoAsync(string para, string asunto, string cuerpo)
        {
            var mail = new MailMessage();
            mail.To.Add(new MailAddress(para));

            // Se lee el usuario del appsettings.json
            string usuario = _configuration["SmtpSettings:Usuario"];
            mail.From = new MailAddress(usuario, "Sistema CACES");
            mail.Subject = asunto;
            mail.Body = cuerpo;
            mail.IsBodyHtml = true;

            // Lee el servidor y el puerto del appsettings.json
            string servidor = _configuration["SmtpSettings:Servidor"];
            int puerto = int.Parse(_configuration["SmtpSettings:Puerto"]);

            using (var smtp = new SmtpClient(servidor, puerto))
            {
                smtp.Credentials = new NetworkCredential(usuario, _configuration["SmtpSettings:Contrasena"]);
                smtp.EnableSsl = bool.Parse(_configuration["SmtpSettings:EnableSsl"]);

                await smtp.SendMailAsync(mail);
            }

        }
    }
}
