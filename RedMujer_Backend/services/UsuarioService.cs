using RedMujer_Backend.models;
using RedMujer_Backend.repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repo;

        public UsuarioService(IUsuarioRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync() =>
            await _repo.GetAllAsync();

        public async Task<Usuario?> GetByIdAsync(int id) =>
            await _repo.GetByIdAsync(id);

        public async Task<int> CrearAsync(Usuario usuario)
        {
            usuario.Contrasenna = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasenna);
            return await _repo.CrearAsync(usuario);
        }

        // Obtener usuario por correo (sin autenticación, solo búsqueda)
        public async Task<Usuario?> GetByCorreoAsync(string correo)
        {
            return await _repo.GetByCorreoAsync(correo);
        }

        // Autenticación por correo
        public async Task<Usuario?> AuthenticateByCorreoAsync(string correo, string plainPassword)
        {
            var user = await _repo.GetByCorreoAsync(correo);
            if (user == null) return null;
            return BCrypt.Net.BCrypt.Verify(plainPassword, user.Contrasenna) ? user : null;
        }

        public async Task ActualizarAsync(int id, Usuario usuario)
        {
            var existente = await _repo.GetByIdAsync(id);
            usuario.Id_Usuario = id;

            if (string.IsNullOrWhiteSpace(usuario.Contrasenna) && existente != null)
            {
                usuario.Contrasenna = existente.Contrasenna;
            }
            else
            {
                usuario.Contrasenna = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasenna);
            }

            await _repo.ActualizarAsync(usuario);
        }

        public async Task EliminarAsync(int id) =>
            await _repo.EliminarAsync(id);

        // Autenticación por nombre de usuario
        public async Task<Usuario?> AuthenticateAsync(string usuarioNombre, string plainPassword)
        {
            var user = await _repo.GetByUsuarioNombreAsync(usuarioNombre);
            if (user == null) return null;
            return BCrypt.Net.BCrypt.Verify(plainPassword, user.Contrasenna) ? user : null;
        }
    }
}
