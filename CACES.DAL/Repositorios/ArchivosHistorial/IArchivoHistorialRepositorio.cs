using System;
using System.Collections.Generic;
using System.Text;
using CACES.DAL.Entidades;

namespace CACES.DAL.Repositorios.ArchivosHistorial
{
    public interface IArchivoHistorialRepositorio
    {
        Task<bool> CrearArchivoAsync(ArchivoHistorial archivo);

        Task<List<ArchivoHistorial>> GetArchivosByHistorialAsync(int idHistorial);
    }
}