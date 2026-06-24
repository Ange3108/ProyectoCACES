using System;
using System.Collections.Generic;
using System.Text;
using CACES.DAL.Entidades;

namespace CACES.BLL.Servicios.ArchivosHistorial
{
    public interface IArchivoHistorialServicio
    {
        Task<bool> CrearArchivoAsync(ArchivoHistorial archivo);

        Task<List<ArchivoHistorial>> GetArchivosByHistorialAsync(int idHistorial);
    }
}