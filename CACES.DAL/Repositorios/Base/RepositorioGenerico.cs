using CACES.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CACES.DAL.Repositorios.Base
{
    /// <summary>
    /// Implementación genérica de repositorio que proporciona operaciones CRUD estándar
    /// </summary>
    /// <typeparam name="T">Tipo de entidad que maneja el repositorio</typeparam>
    public class RepositorioGenerico<T> : IRepositorioGenerico<T> where T : class
    {
        private readonly CACESDbContext _context;
        private readonly DbSet<T> _dbSet;

        public RepositorioGenerico(CACESDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        /// <summary>
        /// Obtiene todas las entidades de forma asíncrona
        /// </summary>
        public async Task<List<T>> ObtenerTodosAsync(
            bool asNoTracking = true,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            // Aplicar includes
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            // Aplicar asNoTracking si es requerido
            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        /// <summary>
        /// Obtiene una entidad por su ID de forma asíncrona
        /// </summary>
        public async Task<T?> ObtenerPorIdAsync(
            int id,
            bool asNoTracking = true,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            // Aplicar includes
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            // Aplicar asNoTracking si es requerido
            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            // Buscar por ID usando FirstOrDefault (más eficiente que FindAsync en este contexto)
            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }

        /// <summary>
        /// Busca una entidad que cumple con un predicado específico
        /// </summary>
        public async Task<T?> BuscarAsync(
            Expression<Func<T, bool>> predicate,
            bool asNoTracking = true,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(predicate);
        }




        public async Task Crear(T entity)
        {
            _dbSet.Add(entity);
          
        }

        /// <summary>
        /// Actualiza una entidad existente
        /// </summary>
        public async Task Actualizar(T entity)
        {
            _dbSet.Update(entity);
        }

        /// <summary>
        /// Elimina una entidad por su ID
        /// </summary>
        public async Task Eliminar(int id)
        {
            var entity = _dbSet.Find(id);

            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        /// <summary>
        /// Guarda todos los cambios realizados en la base de datos
        /// </summary>
        public async Task<bool> GuardarCambiosAsync()
        {
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<List<T>> ObtenerActivos(bool asNoTracking = true, params Expression<Func<T, object>>[] includes)
        {
            var propertyInfo = typeof(T).GetProperty("Estado");
            if (propertyInfo == null)
            {
                throw new InvalidOperationException($"La entidad {typeof(T).Name} no tiene una propiedad 'Estado'.");
            }

            IQueryable<T> query = _context.Set<T>();

            if (asNoTracking)
                query = query.AsNoTracking();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            // Construir dinámicamente: e => e.Estado == true
            var parametro = Expression.Parameter(typeof(T), "e");
            var propiedad = Expression.Property(parametro, propertyInfo);
            var valorTrue = Expression.Constant(true, propertyInfo.PropertyType);
            var igualdad = Expression.Equal(propiedad, valorTrue);
            var lambda = Expression.Lambda<Func<T, bool>>(igualdad, parametro);

            query = query.Where(lambda);

            return await query.ToListAsync();
        }

        public async Task DesactivarEstado(T entity)
        {
            var propertyInfo = entity.GetType().GetProperty("Estado");
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(entity, false);
            }   
        }

       
    }
}
