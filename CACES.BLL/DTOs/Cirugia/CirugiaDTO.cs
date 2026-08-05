using CACES.DAL.Entidades;


namespace CACES.BLL.DTOs.Cirugia
{
    public class CirugiaDTO
    {
        public int Id_Cirugia { get; set; }
        public int Paciente { get; set; } 
        public int Procedimiento { get; set; } 
        public int id_cita { get; set; }
        public int idhorario { get; set; }
      
        public EstadoCirugia Estado { get; set; } // Pendiente, Realizado, Cancelado
        public int Medico { get; set; } 
        
    }
}
