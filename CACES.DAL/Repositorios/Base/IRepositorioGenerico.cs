using System.Linq.Expressions;

namespace CACES.DAL.Repositorios.Base
{
    /// <summary>
    /// Interfaz genérica que define las operaciones CRUD básicas para cualquier repositorio
    /// </summary>
    /// <typeparam name="T">Tipo de entidad que maneja el repositorio</typeparam>
    public interface IRepositorioGenerico<T> where T : class
    {
        /// <summary>
        /// Obtiene todas las entidades de tipo T de forma asíncrona
        /// </summary>
      
        Task<List<T>> ObtenerTodosAsync(
            bool asNoTracking = true,
            params Expression<Func<T, object>>[] includes);

        
        Task<T?> ObtenerPorIdAsync(
            int id,
            bool asNoTracking = true,
            params Expression<Func<T, object>>[] includes);
        Task<List<T>?> ObtenerActivos(
            bool asNoTracking = true,
            params Expression<Func<T, object>>[] includes);




        Task<T>? BuscarAsync(
            Expression<Func<T, bool>> predicate,
            bool asNoTracking = true,
            params Expression<Func<T, object>>[] includes);

     

        Task Crear(T entity);

        Task Actualizar(T entity);

      
        Task Eliminar(int id);

        Task DesactivarEstado(T entity);

        Task<bool> GuardarCambiosAsync();
    }
}
