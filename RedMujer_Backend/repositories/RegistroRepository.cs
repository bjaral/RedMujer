using Dapper;
using Npgsql;
using RedMujer_Backend.models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.repositories
{
    public class RegistroRepository : IRegistroRepository
    {
        private readonly string _connectionString;

        public RegistroRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        public async Task<IEnumerable<Registro>> GetAllAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryAsync<Registro>(
                "SELECT * FROM \"Registros\"");
        }

        public async Task<Registro?> GetByIdAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Registro>(
                "SELECT * FROM \"Registros\" WHERE \"id_registro\" = @Id",
                new { Id = id });
        }

        public async Task InsertAsync(Registro registro)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                @"INSERT INTO ""Registros"" 
                (""usuario_id"", ""fecha"", ""valor_actual"", ""tipo_registro"") 
                VALUES (@UsuarioId, @Fecha, @ValorActual, @TipoRegistro)",
                registro);
        }

        public async Task UpdateAsync(Registro registro)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                @"UPDATE ""Registros"" SET 
                    ""usuario_id"" = @UsuarioId,
                    ""fecha"" = @Fecha,
                    ""valor_actual"" = @ValorActual,
                    ""tipo_registro"" = @TipoRegistro
                WHERE ""id_registro"" = @IdRegistro",
                registro);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "DELETE FROM \"Registros\" WHERE \"id_registro\" = @Id",
                new { Id = id });
        }
    }
}
