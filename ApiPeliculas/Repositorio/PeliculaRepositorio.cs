using ApiPeliculas.Data;
using ApiPeliculas.Modelos;
using ApiPeliculas.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Repositorio
{
    /// <summary>
    /// Repositorio para gestionar las operaciones relacionadas con las películas.
    /// Implementa la interfaz <see cref="IPeliculaRepositorio"/> para definir los métodos específicos de película.
    /// </summary>
    public class PeliculaRepositorio : IPeliculaRepositorio
    {
        private readonly ApplicationDbContext _bd;




        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="PeliculaRepositorio"/>.
        /// </summary>
        /// <param name="bd">Contexto de base de datos de la aplicación.</param>
        public PeliculaRepositorio(ApplicationDbContext bd)
        {
            _bd = bd;
        }




        /// <summary>
        /// Actualiza la información de una película existente.
        /// </summary>
        /// <param name="pelicula">Objeto <see cref="Pelicula"/> que contiene la información actualizada.</param>
        /// <returns>True si la actualización se realizó con éxito, de lo contrario false.</returns>
        public bool ActualizarPelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.Now;

            // Arreglar problema del PATCH
            var peliculaExistente = _bd.Pelicula.Find(pelicula.Id);
            if (peliculaExistente != null)
            {
                _bd.Entry(peliculaExistente).CurrentValues.SetValues(pelicula);
            }
            else
            {
                _bd.Pelicula.Update(pelicula);
            }

            return Guardar();
        }




        /// <summary>
        /// Elimina una película del repositorio.
        /// </summary>
        /// <param name="pelicula">Objeto <see cref="Pelicula"/> que se desea eliminar.</param>
        /// <returns>True si la eliminación se realizó con éxito, de lo contrario false.</returns>
        public bool BorrarPelicula(Pelicula pelicula)
        {
            _bd.Pelicula.Remove(pelicula);
            return Guardar();
        }




        /// <summary>
        /// Busca películas por nombre o descripción.
        /// </summary>
        /// <param name="nombre">Cadena que se utilizará para filtrar las películas.</param>
        /// <returns>Una colección de películas que coinciden con el criterio de búsqueda.</returns>
        public IEnumerable<Pelicula> BuscarPelicula(string nombre)
        {
            IQueryable<Pelicula> query = _bd.Pelicula;
            if (!string.IsNullOrEmpty(nombre))
            {
                query = query.Where(e => e.Nombre.Contains(nombre) || e.Descripcion.Contains(nombre));
            }
            return query.ToList();
        }




        /// <summary>
        /// Crea una nueva película en el repositorio.
        /// </summary>
        /// <param name="pelicula">Objeto <see cref="Pelicula"/> que se desea agregar.</param>
        /// <returns>True si la creación se realizó con éxito, de lo contrario false.</returns>
        public bool CrearPelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.Now;
            _bd.Pelicula.Add(pelicula);
            return Guardar();
        }




        /// <summary>
        /// Verifica si existe una película por su ID.
        /// </summary>
        /// <param name="id">Identificador de la película.</param>
        /// <returns>True si la película existe, de lo contrario false.</returns>
        public bool ExistePelicula(int id)
        {
            return _bd.Pelicula.Any(c => c.Id == id);
        }




        /// <summary>
        /// Verifica si existe una película por su nombre.
        /// </summary>
        /// <param name="nombre">Nombre de la película.</param>
        /// <returns>True si la película existe, de lo contrario false.</returns>
        public bool ExistePelicula(string nombre)
        {
            return _bd.Pelicula.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
        }




        /// <summary>
        /// Obtiene una película por su identificador.
        /// </summary>
        /// <param name="peliculaId">Identificador de la película.</param>
        /// <returns>El objeto <see cref="Pelicula"/> correspondiente al identificador, o null si no existe.</returns>
        public Pelicula GetPelicula(int peliculaId)
        {
            return _bd.Pelicula.FirstOrDefault(c => c.Id == peliculaId);
        }




        /// <summary>
        /// Obtiene una lista de películas con paginación.
        /// </summary>
        /// <param name="pageNumber">Número de página para la paginación.</param>
        /// <param name="pageSize">Número de elementos por página.</param>
        /// <returns>Una colección de películas en la página solicitada.</returns>
        public ICollection<Pelicula> GetPeliculas(int pageNumber, int pageSize)
        {
            return _bd.Pelicula.OrderBy(c => c.Nombre)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }




        /// <summary>
        /// Obtiene las películas asociadas a una categoría específica.
        /// </summary>
        /// <param name="catId">Identificador de la categoría.</param>
        /// <returns>Una colección de películas que pertenecen a la categoría especificada.</returns>
        public ICollection<Pelicula> GetPeliculasEnCategoria(int catId)
        {
            return _bd.Pelicula.Include(ca => ca.Categoria).Where(ca => ca.categoriaId == catId).ToList();
        }




        /// <summary>
        /// Obtiene el total de películas en el repositorio.
        /// </summary>
        /// <returns>El número total de películas.</returns>
        public int GetTotalPeliculas()
        {
            return _bd.Pelicula.Count();
        }




        /// <summary>
        /// Guarda los cambios realizados en el contexto de la base de datos.
        /// </summary>
        /// <returns>True si los cambios se guardaron con éxito, de lo contrario false.</returns>
        public bool Guardar()
        {
            return _bd.SaveChanges() >= 0;
        }
    }
}
