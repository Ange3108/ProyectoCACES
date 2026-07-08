namespace CACES.BLL.DTOs.Perfil
{
    public class PerfilUsuarioDTO
    {
        public int IdUsuario { get; set; }
        public string Nombres { get; set; } = null!;
        public string PrimerApellido { get; set; } = null!;
        public string SegundoApellido { get; set; } = null!;
        public string CorreoElectronico { get; set; } = null!;
        public string DUI { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public bool Estado { get; set; }
        public string TipoSangre { get; set; } = null!;
        public string Alergias { get; set; } = null!;
        public string EnfermedadesCronicas { get; set; } = null!;

        public string MedicamentosActuales { get; set; } = null!;
        public int IdHistorial { get; set; }
    }
}
