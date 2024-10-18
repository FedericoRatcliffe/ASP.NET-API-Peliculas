using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
using AutoMapper;

namespace ApiPeliculas.PeliculasMapper
{
    /// <summary>
    /// Clase de mapeo que configura las transformaciones entre las entidades del modelo y los DTOs (Data Transfer Objects).
    /// Utiliza AutoMapper para facilitar la conversión entre objetos.
    /// </summary>
    public class PeliculasMapper : Profile
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="PeliculasMapper"/> 
        /// y configura los mapeos entre las entidades y los DTOs.
        /// </summary>
        public PeliculasMapper()
        {
            // Mapeo de Categoria a CategoriaDto y viceversa
            CreateMap<Categoria, CategoriaDto>().ReverseMap();

            // Mapeo de Categoria a CrearCategoriaDto y viceversa
            CreateMap<Categoria, CrearCategoriaDto>().ReverseMap();

            // Mapeo de Pelicula a PeliculaDto y viceversa
            CreateMap<Pelicula, PeliculaDto>().ReverseMap();

            // Mapeo de Pelicula a CrearPeliculaDto y viceversa
            CreateMap<Pelicula, CrearPeliculaDto>().ReverseMap();

            // Mapeo de Pelicula a ActualizarPeliculaDto y viceversa
            CreateMap<Pelicula, ActualizarPeliculaDto>().ReverseMap();

            // Mapeo de AppUsuario a UsuarioDatosDto y viceversa
            CreateMap<AppUsuario, UsuarioDatosDto>().ReverseMap();

            // Mapeo de AppUsuario a UsuarioDto y viceversa
            CreateMap<AppUsuario, UsuarioDto>().ReverseMap();
        }
    }
}
