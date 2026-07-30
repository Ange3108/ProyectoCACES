using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;
using CACES.BLL.Servicios.Configuracion;

namespace CACES.BLL.Servicios.Notificacion
{
   
    public class EmailServicio : IEmailServicio
    {
        private readonly IConfiguracionServicio _configuracionServicio;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailServicio> _logger;

        public EmailServicio(
            IConfiguracionServicio configuracionServicio,
            IConfiguration configuration,
            ILogger<EmailServicio> logger)
        {
            _configuracionServicio = configuracionServicio;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> EnviarCorreoAsync(string destinatario, string asunto, string cuerpoHtml)
        {
            if (string.IsNullOrWhiteSpace(destinatario))
            {
                _logger.LogWarning("EnviarCorreoAsync: destinatario vacío, se omite el envío.");
                return false;
            }

            try
            {
                var host = await ObtenerConFallback("Smtp.Host", "SmtpSettings:Servidor");
                if (string.IsNullOrWhiteSpace(host))
                {
                    _logger.LogWarning("EnviarCorreoAsync: no se encontró configuración de host SMTP (ni en BD ni en appsettings).");
                    return false;
                }

                var puertoStr = await ObtenerConFallback("Smtp.Puerto", "SmtpSettings:Puerto");
                var puerto = int.TryParse(puertoStr, out var p) ? p : 587;

                var usuario = await ObtenerConFallback("Smtp.Usuario", "SmtpSettings:Usuario");
                var password = await ObtenerConFallback("Smtp.Password", "SmtpSettings:Contrasena");

                var usarSslStr = await ObtenerConFallback("Smtp.UsarSsl", "SmtpSettings:EnableSsl");
                var usarSsl = bool.TryParse(usarSslStr, out var ssl) ? ssl : true;

                var correoOrigen = await ObtenerConFallback("Smtp.CorreoOrigen", "SmtpSettings:Usuario");
                if (string.IsNullOrWhiteSpace(correoOrigen))
                    correoOrigen = usuario;

                var nombreOrigen = await ObtenerConFallback("Smtp.NombreOrigen", null) ?? "Sistema CACES";

                using var mensaje = new MailMessage
                {
                    From = new MailAddress(correoOrigen, nombreOrigen),
                    Subject = asunto,
                    Body = cuerpoHtml,
                    IsBodyHtml = true
                };
                mensaje.To.Add(destinatario);

                using var cliente = new SmtpClient(host, puerto)
                {
                    Credentials = new NetworkCredential(usuario, password),
                    EnableSsl = usarSsl
                };

                await cliente.SendMailAsync(mensaje);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar correo a {Destinatario} con asunto '{Asunto}'", destinatario, asunto);
                return false;
            }
        }

        private async Task<string?> ObtenerConFallback(string claveBd, string? claveAppSettings)
        {
            var valor = await _configuracionServicio.ObtenerValorString(claveBd);

            if (!string.IsNullOrWhiteSpace(valor))
                return valor;

            if (!string.IsNullOrEmpty(claveAppSettings))
                return _configuration[claveAppSettings];

            return null;
        }
    }
}