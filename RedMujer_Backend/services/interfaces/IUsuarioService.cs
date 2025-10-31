using RedMujer_Backend.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.services
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(int id);
        Task<int> CrearAsync(Usuario usuario);
        Task<Usuario?> GetByCorreoAsync(string correo);
        Task ActualizarAsync(int id, Usuario usuario);
        Task EliminarAsync(int id);
        Task<Usuario?> AuthenticateAsync(string usuarioNombre, string plainPassword);
        Task<Usuario?> AuthenticateByCorreoAsync(string correo, string plainPassword);
        Task<bool> CambiarContrasenaAsync(int userId, string contrasenaActual, string contrasenaNueva);
    }
}
