using Dapper;
using Npgsql;
using RedMujer_Backend.models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.repositories
{
    public class UbicacionRepository : IUbicacionRepository
    {
        private readonly IConfiguration _config;
        private readonly string? _connectionString;
        public UbicacionRepository(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Ubicacion>> GetAllAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryAsync<Ubicacion>("SELECT * FROM \"Ubicaciones\" WHERE \"Vigencia\" = true");
        }

        public async Task<Ubicacion?> GetByIdAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Ubicacion>(
                "SELECT * FROM \"Ubicaciones\" WHERE \"Id_Ubicacion\" = @Id AND \"Vigencia\" = true", new { Id = id });
        }

        public async Task InsertAsync(Ubicacion ubicacion)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "INSERT INTO \"Ubicaciones\" (\"Id_Comuna\", \"Id_Emprendimiento\", \"Calle\", \"Numero\", \"Referencia\", \"Vigencia\") " +
                "VALUES (@Id_Comuna, @Id_Emprendimiento, @Calle, @Numero, @Referencia, @Vigencia)", ubicacion);
        }

        public async Task UpdateAsync(Ubicacion ubicacion)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "UPDATE \"Ubicaciones\" SET \"Id_Comuna\" = @Id_Comuna, \"Id_Emprendimiento\" = @Id_Emprendimiento, " +
                "\"Calle\" = @Calle, \"Numero\" = @Numero, \"Referencia\" = @Referencia, \"Vigencia\" = @Vigencia " +
                "WHERE \"Id_Ubicacion\" = @Id_Ubicacion", ubicacion);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "UPDATE \"Ubicaciones\" SET \"Vigencia\" = false WHERE \"Id_Ubicacion\" = @Id", new { Id = id });
        }
    }
}
