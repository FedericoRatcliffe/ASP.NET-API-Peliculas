using System;
using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Modelos
{
    /// <summary>
    /// Representa una categoría de películas.
    /// Esta clase se utiliza para definir las propiedades de una categoría en el sistema.
    /// </summary>
    public class Categoria
    {



        /// <summary>
        /// Obtiene o establece el identificador único de la categoría.
        /// </summary>
        [Key]
        public int Id { get; set; }




        /// <summary>
        /// Obtiene o establece el nombre de la categoría.
        /// Este campo es obligatorio.
        /// </summary>
        [Required]
        public string Nombre { get; set; }




        /// <summary>
        /// Obtiene o establece la fecha de creación de la categoría.
        /// Este campo es obligatorio.
        /// </summary>
        [Required]
        public DateTime FechaCreacion { get; set; }
    }
}
