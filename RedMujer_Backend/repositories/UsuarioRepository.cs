using Dapper;
using Npgsql;
using RedMujer_Backend.models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace RedMujer_Backend.repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            const string query = @"SELECT * FROM ""Usuarios""";
            using var conn = CreateConnection();
            return await conn.QueryAsync<Usuario>(query);
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            const string query = @"SELECT * FROM ""Usuarios"" WHERE id_usuario = @Id";
            using var conn = CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<Usuario>(query, new { Id = id });
        }

        public async Task CrearAsync(Usuario usuario)
        {
            const string query = @"
                INSERT INTO ""Usuarios"" (usuario, contrasenna, vigencia, tipo_usuario, correo)
                VALUES (@UsuarioNombre, @Contrasenna, @Vigencia, @Tipo_Usuario::tipo_usuario, @Correo)";

            usuario.Contrasenna = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasenna);

            using var conn = CreateConnection();
            await conn.ExecuteAsync(query, usuario);
        }

        public async Task ActualizarAsync(Usuario usuario)
        {
            const string query = @"
                UPDATE ""Usuarios""
                SET usuario = @UsuarioNombre,
                    contrasenna = @Contrasenna,
                    vigencia = @Vigencia,
                    tipo_usuario = @Tipo_Usuario::tipo_usuario,
                    correo = @Correo
                WHERE id_usuario = @Id_Usuario";

            usuario.Contrasenna = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasenna);

            using var conn = CreateConnection();
            await conn.ExecuteAsync(query, usuario);
        }

        public async Task EliminarAsync(int id)
        {
            const string query = @"DELETE FROM ""Usuarios"" WHERE id_usuario = @Id";
            using var conn = CreateConnection();
            await conn.ExecuteAsync(query, new { Id = id });
        }

        public async Task<Usuario?> GetByUsuarioNombreAsync(string usuarioNombre)
        {
            const string query = @"SELECT * FROM ""Usuarios"" WHERE usuario = @UsuarioNombre";
            using var conn = CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<Usuario>(query, new { UsuarioNombre = usuarioNombre });
        }
    }
}
