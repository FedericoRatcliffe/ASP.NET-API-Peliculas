using ApiPeliculas.Data;
using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiPeliculas.Repositorio
{
    /// <summary>
    /// Repositorio para gestionar las operaciones relacionadas con los usuarios.
    /// Implementa la interfaz <see cref="IUsuarioRepositorio"/> para definir los métodos específicos de usuario.
    /// </summary>
    public class UsuarioRepositorio : IUsuarioRepositorio
    {

        private readonly ApplicationDbContext _bd;
        private readonly string claveSecreta;
        private readonly UserManager<AppUsuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;




        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="UsuarioRepositorio"/>.
        /// </summary>
        /// <param name="bd">Contexto de base de datos de la aplicación.</param>
        /// <param name="config">Configuración de la aplicación para acceder a valores como la clave secreta.</param>
        /// <param name="userManager">Gestor de usuarios para operaciones relacionadas con <see cref="AppUsuario"/>.</param>
        /// <param name="roleManager">Gestor de roles para manejar roles de usuarios.</param>
        /// <param name="mapper">Instancia de AutoMapper para la conversión entre entidades y DTOs.</param>
        public UsuarioRepositorio(ApplicationDbContext bd, IConfiguration config, UserManager<AppUsuario> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _bd = bd;
            claveSecreta = config.GetValue<string>("ApiSettings:Secreta");
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }




        /// <summary>
        /// Obtiene un usuario por su identificador.
        /// </summary>
        /// <param name="usuarioId">Identificador del usuario.</param>
        /// <returns>El usuario correspondiente al identificador, o null si no existe.</returns>
        public AppUsuario GetUsuario(string usuarioId)
        {
            return _bd.AppUsuario.FirstOrDefault(c => c.Id == usuarioId);
        }



        /// <summary>
        /// Obtiene una lista de todos los usuarios.
        /// </summary>
        /// <returns>Una colección de usuarios ordenada por nombre de usuario.</returns>
        public ICollection<AppUsuario> GetUsuarios()
        {
            return _bd.AppUsuario.OrderBy(c => c.UserName).ToList();
        }




        /// <summary>
        /// Verifica si el nombre de usuario es único en la base de datos.
        /// </summary>
        /// <param name="usuario">Nombre de usuario a verificar.</param>
        /// <returns>True si el nombre de usuario es único, de lo contrario false.</returns>
        public bool IsUniqueUser(string usuario)
        {
            var usuarioBd = _bd.AppUsuario.FirstOrDefault(u => u.UserName == usuario);
            return usuarioBd == null;
        }




        /// <summary>
        /// Realiza el inicio de sesión de un usuario.
        /// </summary>
        /// <param name="usuarioLoginDto">Objeto que contiene las credenciales de inicio de sesión.</param>
        /// <returns>Un objeto <see cref="UsuarioLoginRespuestaDto"/> que contiene el token de acceso y la información del usuario.</returns>
        public async Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto)
        {
            var usuario = _bd.AppUsuario.FirstOrDefault(
                u => u.UserName.ToLower() == usuarioLoginDto.NombreUsuario.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(usuario, usuarioLoginDto.Password);

            // Validación de existencia de usuario y validez de la contraseña
            if (usuario == null || !isValid)
            {
                return new UsuarioLoginRespuestaDto()
                {
                    Token = "",
                    Usuario = null
                };
            }

            // Procesamiento de login
            var roles = await _userManager.GetRolesAsync(usuario);
            var manejadoToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(claveSecreta);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = manejadoToken.CreateToken(tokenDescriptor);

            return new UsuarioLoginRespuestaDto()
            {
                Token = manejadoToken.WriteToken(token),
                Usuario = _mapper.Map<UsuarioDatosDto>(usuario),
            };
        }




        /// <summary>
        /// Registra un nuevo usuario en el sistema.
        /// </summary>
        /// <param name="usuarioRegistroDto">Objeto que contiene los datos del usuario a registrar.</param>
        /// <returns>Un objeto <see cref="UsuarioDatosDto"/> que representa al usuario registrado, o un objeto vacío si el registro falla.</returns>
        public async Task<UsuarioDatosDto> Registro(UsuarioRegistroDto usuarioRegistroDto)
        {
            AppUsuario usuario = new AppUsuario()
            {
                UserName = usuarioRegistroDto.NombreUsuario,
                Email = usuarioRegistroDto.NombreUsuario,
                NormalizedEmail = usuarioRegistroDto.NombreUsuario.ToUpper(),
                Nombre = usuarioRegistroDto.Nombre
            };

            var result = await _userManager.CreateAsync(usuario, usuarioRegistroDto.Password);
            if (result.Succeeded)
            {
                if (!_roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("Registrado"));
                }

                await _userManager.AddToRoleAsync(usuario, "Admin");
                var usuarioRetornado = _bd.AppUsuario.FirstOrDefault(u => u.UserName == usuarioRegistroDto.NombreUsuario);

                return _mapper.Map<UsuarioDatosDto>(usuarioRetornado);
            }

            return new UsuarioDatosDto();
        }




        // Método para encriptar contraseña con MD5 se usa tanto en el Acceso como en el Registro
        // public static string obtenermd5(string valor)
        // {
        //     MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
        //     byte[] data = System.Text.Encoding.UTF8.GetBytes(valor);
        //     data = x.ComputeHash(data);
        //     string resp = "";
        //     for (int i = 0; i < data.Length; i++)
        //         resp += data[i].ToString("x2").ToLower();
        //     return resp;
        // }
    
    
    
    }
}



