using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Modelos
{



    /// <summary>
    /// Representa un usuario en el sistema.
    /// Esta clase contiene la información de acceso y los detalles del usuario.
    /// </summary>
    public class Usuario
    {



        /// <summary>
        /// Obtiene o establece el identificador único del usuario.
        /// </summary>
        [Key]
        public int Id { get; set; }




        /// <summary>
        /// Obtiene o establece el nombre de usuario para la autenticación.
        /// </summary>
        public string NombreUsuario { get; set; }




        /// <summary>
        /// Obtiene o establece el nombre completo del usuario.
        /// </summary>
        public string Nombre { get; set; }




        /// <summary>
        /// Obtiene o establece la contraseña del usuario.
        /// Se recomienda almacenar la contraseña de forma segura (por ejemplo, utilizando hash).
        /// </summary>
        public string Password { get; set; }




        /// <summary>
        /// Obtiene o establece el rol del usuario dentro del sistema (por ejemplo, Admin, Usuario).
        /// </summary>
        public string Role { get; set; }
    }
}
