using Microsoft.AspNetCore.Identity;

namespace ApiPeliculas.Modelos
{
    /// <summary>
    /// Representa un usuario de la aplicación que se extiende de <see cref="IdentityUser"/>.
    /// Esta clase agrega propiedades adicionales específicas para la aplicación.
    /// </summary>
    public class AppUsuario : IdentityUser
    {



        /// <summary>
        /// Obtiene o establece el nombre completo del usuario.
        /// </summary>
        public string Nombre { get; set; }



    }
}
