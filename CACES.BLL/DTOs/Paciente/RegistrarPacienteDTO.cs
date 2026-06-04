using CACES.BLL.DTOs.Historial;
using CACES.BLL.DTOs.Usuario;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CACES.BLL.DTOs.Paciente
{
    public class RegistrarPacienteDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombres { get; set; } = null!;

        [Required(ErrorMessage = "El primer apellido es obligatorio")]
        public string PrimerApellido { get; set; } = null!;

        [Required(ErrorMessage = "El segundo apellido es obligatorio")]
        public string SegundoApellido { get; set; } = null!;

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Correo inválido")]
        public string CorreoElectronico { get; set; } = null!;

        [Required(ErrorMessage = "El DUI es obligatorio")]
        public string DUI { get; set; } = null!;

        [Required(ErrorMessage = "La dirección es obligatoria")]
        public string Direccion { get; set; } = null!;

        [Required(ErrorMessage = "La edad es obligatoria")]
        public int Edad { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        public string Telefono { get; set; } = null!;

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        public DateTime Nacimiento { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; } = null!;

        public string Foto { get; set; } = "paciente.jpg";

        [Required(ErrorMessage = "El tipo de sangre es obligatorio")]
        public string TipoSangre { get; set; } = null!;

        [Required(ErrorMessage = "Debe indicar alergias o escribir Ninguna")]
        public string Alergias { get; set; } = null!;

        [Required(ErrorMessage = "Debe indicar enfermedades o escribir Ninguna")]
        public string EnfermedadesCronicas { get; set; } = null!;

        [Required(ErrorMessage = "Debe indicar antecedentes o escribir Ninguno")]
        public string Antecedentes { get; set; } = null!;

        [Required(ErrorMessage = "Los detalles son obligatorios")]
        public string Detalles { get; set; } = null!;
    }
}