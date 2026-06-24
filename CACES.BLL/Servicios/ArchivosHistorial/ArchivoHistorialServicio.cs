using System;
using System.Collections.Generic;
using System.Text;
using CACES.DAL.Entidades;
using CACES.DAL.Repositorios.ArchivosHistorial;

namespace CACES.BLL.Servicios.ArchivosHistorial
{
    public class ArchivoHistorialServicio : IArchivoHistorialServicio
    {
        private readonly IArchivoHistorialRepositorio _archivoRepositorio;

        public ArchivoHistorialServicio(
            IArchivoHistorialRepositorio archivoRepositorio)
        {
            _archivoRepositorio = archivoRepositorio;
        }

        public async Task<bool> CrearArchivoAsync(ArchivoHistorial archivo)
        {
            return await _archivoRepositorio.CrearArchivoAsync(archivo);
        }

        public async Task<List<ArchivoHistorial>> GetArchivosByHistorialAsync(int idHistorial)
        {
            return await _archivoRepositorio.GetArchivosByHistorialAsync(idHistorial);
        }
    }
}