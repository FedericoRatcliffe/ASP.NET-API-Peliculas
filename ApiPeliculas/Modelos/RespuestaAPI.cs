using System.Collections.Generic;
using System.Net;

namespace ApiPeliculas.Modelos
{
    /// <summary>
    /// Representa la respuesta de una API.
    /// Esta clase se utiliza para encapsular el estado y los resultados de una operación de API.
    /// </summary>
    public class RespuestaAPI
    {



        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="RespuestaAPI"/>.
        /// Se inicializa la lista de mensajes de error.
        /// </summary>
        public RespuestaAPI()
        {
            ErrorMessages = new List<string>();
        }




        /// <summary>
        /// Obtiene o establece el código de estado HTTP de la respuesta.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }




        /// <summary>
        /// Obtiene o establece un valor que indica si la operación fue exitosa.
        /// Por defecto, se establece en true.
        /// </summary>
        public bool IsSuccess { get; set; } = true;




        /// <summary>
        /// Obtiene o establece una lista de mensajes de error relacionados con la operación.
        /// </summary>
        public List<string> ErrorMessages { get; set; }




        /// <summary>
        /// Obtiene o establece el resultado de la operación.
        /// Puede contener cualquier tipo de objeto.
        /// </summary>
        public object Result { get; set; }
    }
}
