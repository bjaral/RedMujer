using Dapper;
using Npgsql;
using RedMujer_Backend.models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryAsync<Usuario>(
                @"SELECT 
                    ""id_usuario"" AS ""Id_Usuario"",
                    ""usuario"" AS ""UsuarioNombre"",
                    ""contrasenna"" AS ""Contrasenna"",
                    ""vigencia"" AS ""vigencia"",
                    ""tipo_usuario"" AS ""Tipo_Usuario"",
                    ""correo"" AS ""Correo""
                  FROM ""Usuarios"" WHERE vigencia = true");
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Usuario>(
                @"SELECT 
                    ""id_usuario"" AS ""Id_Usuario"",
                    ""usuario"" AS ""UsuarioNombre"",
                    ""contrasenna"" AS ""Contrasenna"",
                    ""vigencia"" AS ""vigencia"",
                    ""tipo_usuario"" AS ""Tipo_Usuario"",
                    ""correo"" AS ""Correo""
                  FROM ""Usuarios"" WHERE ""id_usuario"" = @Id AND vigencia = true",
                new { Id = id });
        }

        public async Task<Usuario?> GetByUsuarioNombreAsync(string usuarioNombre)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Usuario>(
                @"SELECT 
                    ""id_usuario"" AS ""Id_Usuario"",
                    ""usuario"" AS ""UsuarioNombre"",
                    ""contrasenna"" AS ""Contrasenna"",
                    ""vigencia"" AS ""vigencia"",
                    ""tipo_usuario"" AS ""Tipo_Usuario"",
                    ""correo"" AS ""Correo""
                  FROM ""Usuarios"" WHERE ""usuario"" = @UsuarioNombre AND vigencia = true",
                new { UsuarioNombre = usuarioNombre });
        }

        // *** AQUÍ VIENE EL CAMBIO: ::tipo_usuario ***
        public async Task<int> CrearAsync(Usuario usuario)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.ExecuteScalarAsync<int>(
                @"INSERT INTO ""Usuarios"" 
                    (""usuario"", ""contrasenna"", ""vigencia"", ""tipo_usuario"", ""correo"") 
                  VALUES (@UsuarioNombre, @Contrasenna, @vigencia, @TipoUsuarioStr::tipo_usuario, @Correo) RETURNING id_usuario",
                new {
                    usuario.UsuarioNombre,
                    usuario.Contrasenna,
                    usuario.Vigencia,
                    TipoUsuarioStr = usuario.Tipo_Usuario.ToString(),
                    usuario.Correo
                });
        }
        public async Task<Usuario?> GetByCorreoAsync(string correo)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Usuario>(
                @"SELECT 
                    ""id_usuario"" AS ""Id_Usuario"",
                    ""usuario"" AS ""UsuarioNombre"",
                    ""contrasenna"" AS ""Contrasenna"",
                    ""vigencia"" AS ""vigencia"",
                    ""tipo_usuario"" AS ""Tipo_Usuario"",
                    ""correo"" AS ""Correo""
                FROM ""Usuarios"" WHERE ""correo"" = @Correo AND vigencia = true",
                new { Correo = correo });
        }

        public async Task ActualizarAsync(Usuario usuario)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                @"UPDATE ""Usuarios"" SET 
                        ""usuario"" = @UsuarioNombre,
                        ""contrasenna"" = @Contrasenna,
                        ""vigencia"" = @vigencia,
                        ""tipo_usuario"" = @TipoUsuarioStr::tipo_usuario,
                        ""correo"" = @Correo
                  WHERE ""id_usuario"" = @Id_Usuario",
                new {
                    usuario.UsuarioNombre,
                    usuario.Contrasenna,
                    usuario.Vigencia,
                    TipoUsuarioStr = usuario.Tipo_Usuario.ToString(),
                    usuario.Correo,
                    usuario.Id_Usuario
                });
        }

        public async Task EliminarAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                @"UPDATE ""Usuarios"" SET vigencia = false WHERE ""id_usuario"" = @Id_Usuario",
                new { Id_Usuario = id });
        }
    }
}
