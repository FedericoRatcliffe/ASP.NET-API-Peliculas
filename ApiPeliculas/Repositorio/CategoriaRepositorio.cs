using ApiPeliculas.Data;
using ApiPeliculas.Modelos;
using ApiPeliculas.Repositorio.IRepositorio;

namespace ApiPeliculas.Repositorio
{
    /// <summary>
    /// Repositorio para gestionar las operaciones relacionadas con las categorías.
    /// Implementa la interfaz <see cref="ICategoriaRepositorio"/> para definir los métodos específicos de categoría.
    /// </summary>
    public class CategoriaRepositorio : ICategoriaRepositorio
    {
        private readonly ApplicationDbContext _bd;




        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CategoriaRepositorio"/>.
        /// </summary>
        /// <param name="bd">Contexto de base de datos de la aplicación.</param>
        public CategoriaRepositorio(ApplicationDbContext bd)
        {
            _bd = bd;
        }




        /// <summary>
        /// Actualiza la información de una categoría existente.
        /// </summary>
        /// <param name="categoria">Objeto <see cref="Categoria"/> que contiene la información actualizada.</param>
        /// <returns>True si la actualización se realizó con éxito, de lo contrario false.</returns>
        public bool ActualizarCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = DateTime.Now;

            // Arreglar problema del PUT
            var categoriaExistente = _bd.Categorias.Find(categoria.Id);
            if (categoriaExistente != null)
            {
                _bd.Entry(categoriaExistente).CurrentValues.SetValues(categoria);
            }
            else
            {
                _bd.Categorias.Update(categoria);
            }

            return Guardar();
        }




        /// <summary>
        /// Elimina una categoría del repositorio.
        /// </summary>
        /// <param name="categoria">Objeto <see cref="Categoria"/> que se desea eliminar.</param>
        /// <returns>True si la eliminación se realizó con éxito, de lo contrario false.</returns>
        public bool BorrarCategoria(Categoria categoria)
        {
            _bd.Categorias.Remove(categoria);
            return Guardar();
        }




        /// <summary>
        /// Crea una nueva categoría en el repositorio.
        /// </summary>
        /// <param name="categoria">Objeto <see cref="Categoria"/> que se desea agregar.</param>
        /// <returns>True si la creación se realizó con éxito, de lo contrario false.</returns>
        public bool CrearCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = DateTime.Now;
            _bd.Categorias.Add(categoria);
            return Guardar();
        }




        /// <summary>
        /// Verifica si existe una categoría por su ID.
        /// </summary>
        /// <param name="id">Identificador de la categoría.</param>
        /// <returns>True si la categoría existe, de lo contrario false.</returns>
        public bool ExisteCategoria(int id)
        {
            return _bd.Categorias.Any(c => c.Id == id);
        }




        /// <summary>
        /// Verifica si existe una categoría por su nombre.
        /// </summary>
        /// <param name="nombre">Nombre de la categoría.</param>
        /// <returns>True si la categoría existe, de lo contrario false.</returns>
        public bool ExisteCategoria(string nombre)
        {
            return _bd.Categorias.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
        }




        /// <summary>
        /// Obtiene una categoría por su identificador.
        /// </summary>
        /// <param name="CategoriaId">Identificador de la categoría.</param>
        /// <returns>El objeto <see cref="Categoria"/> correspondiente al identificador, o null si no existe.</returns>
        public Categoria GetCategoria(int CategoriaId)
        {
            return _bd.Categorias.FirstOrDefault(c => c.Id == CategoriaId);
        }




        /// <summary>
        /// Obtiene una lista de todas las categorías en el repositorio.
        /// </summary>
        /// <returns>Una colección de categorías ordenadas por nombre.</returns>
        public ICollection<Categoria> GetCategorias()
        {
            return _bd.Categorias.OrderBy(c => c.Nombre).ToList();
        }




        /// <summary>
        /// Guarda los cambios realizados en el contexto de la base de datos.
        /// </summary>
        /// <returns>True si los cambios se guardaron con éxito, de lo contrario false.</returns>
        public bool Guardar()
        {
            return _bd.SaveChanges() >= 0 ? true : false;
        }
    }
}
