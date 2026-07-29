namespace CACES.BLL.Servicios.Notificacion
{
 
    public interface INotificadorServicio
    {
        Task NotificarAsync(string evento, int idUsuario, string titulo, string mensaje, string? correoDestinoManual = null);
    }
}
