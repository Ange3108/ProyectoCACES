using CACES.BLL.DTOs.Auth;
using CACES.BLL.Servicios.Usuario;
using CACES.DAL.Repositorios.Usuario;

namespace CACES.BLL.Servicios.Auth
{
    public class AuthServicio : IAuthServicio
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IUsuarioService _usuarioService;

        public AuthServicio(IUsuarioRepositorio usuarioRepositorio, IUsuarioService usuarioService)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _usuarioService = usuarioService;
        }

        public async Task<bool> LoginAsync(LoginDTO dto)
        {
            var usuario = await _usuarioRepositorio.GetUsuarioByEmailAsync(dto.CorreoElectronico);

            if (usuario == null)
                return false;

            if (usuario.Estado != true)
                return false;

            var passwordHash = HashContraseña(dto.Password);

            return usuario.PasswordHash == passwordHash;
        }

        public async Task<DAL.Entidades.Usuario> AutenticarAsync(LoginDTO dto)
        {
            var usuario = await _usuarioRepositorio.GetUsuarioByEmailAsync(dto.CorreoElectronico);

            if (usuario == null || usuario.Estado != true)
                return null;
            

            var passwordHash = HashContraseña(dto.Password);

            if (usuario.PasswordHash != passwordHash)
                return null;

            return usuario;
        }

        private string HashContraseña(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        public async Task<(bool Exito, string Token, string Mensaje)> GenerarTokenRecuperacionAsync(OlvidoContrasenaDTO dto)
        {
            var usuario = await _usuarioRepositorio.GetUsuarioByEmailAsync(dto.CorreoElectronico);

            if (usuario == null || usuario.Estado != true)
                
            {
                return (true, string.Empty, "Si el correo coincide con una cuenta activa, se enviarán las instrucciones.");
            }

            string tokenRecuperacion = Guid.NewGuid().ToString();
            usuario.SecurityStamp = tokenRecuperacion;

            usuario.FechaDeModificacion = DateTime.UtcNow;

            bool guardado = await _usuarioRepositorio.UpdateUsuarioAsync(usuario);

            if (!guardado)
                return (false, string.Empty, "Hubo un problema al procesar la solicitud.");

            return (true, tokenRecuperacion, "Si el correo coincide con una cuenta activa, se enviarán las instrucciones.");
        }

        public async Task<(bool Exito, string Mensaje)>RestablecerContraseñaAsync(RestablecerContrasenaDTO dto)
        {
            var usuario = await _usuarioRepositorio.GetUsuarioByEmailAsync(dto.CorreoElectronico);

            if (usuario == null || usuario.Estado != true)
                
            return (false, "El usuario no es válido.");

            if (string.IsNullOrEmpty(usuario.SecurityStamp) || usuario.SecurityStamp != dto.Token)
            {
                return (false, "El token de recuperación es inválido o ya ha sido utilizado.");
            }

            var validacionPass = _usuarioService.ValidarContraseña(dto.NuevaContrasena);
            if (!validacionPass.IsValid)
            {
                return (false, validacionPass.Message);
            }

            usuario.PasswordHash = HashContraseña(dto.NuevaContrasena);

            usuario.SecurityStamp = Guid.NewGuid().ToString();
            usuario.FechaDeModificacion = DateTime.UtcNow;

            bool actualizado = await _usuarioRepositorio.UpdateUsuarioAsync(usuario);

            if (!actualizado)
                return (false, "No se pudo actualizar la contraseña. Intente más tarde.");

            return (true, "Contraseña restablecida con éxito.");
        }
    }
}