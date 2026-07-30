namespace CACES.BLL.Servicios.Notificacion
{
    public interface IEmailServicio
    {
        Task<bool> EnviarCorreoAsync(string destinatario, string asunto, string cuerpoHtml);
    }
}
