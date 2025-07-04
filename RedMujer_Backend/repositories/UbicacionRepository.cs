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
            return await connection.QueryAsync<Ubicacion>("SELECT * FROM \"Ubicaciones\" WHERE \"vigencia\" = true");
        }

        public async Task<Ubicacion?> GetByIdAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Ubicacion>(
                "SELECT * FROM \"Ubicaciones\" WHERE \"id_ubicacion\" = @Id AND \"vigencia\" = true", new { Id = id });
        }

        public async Task<int> InsertAsync(Ubicacion ubicacion)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.ExecuteScalarAsync<int>(
                "INSERT INTO \"Ubicaciones\" (\"id_comuna\", \"calle\", \"numero\", \"referencia\", \"vigencia\") " +
                "VALUES (@Id_Comuna, @Calle, @Numero, @Referencia, @vigencia) RETURNING \"id_ubicacion\";", ubicacion);
        }

        public async Task UpdateAsync(Ubicacion ubicacion)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "UPDATE \"Ubicaciones\" SET \"id_comuna\" = @Id_Comuna," +
                "\"calle\" = @Calle, \"numero\" = @Numero, \"referencia\" = @Referencia, \"vigencia\" = @vigencia " +
                "WHERE \"id_ubicacion\" = @Id_Ubicacion", ubicacion);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "UPDATE \"Ubicaciones\" SET \"vigencia\" = false WHERE \"id_ubicacion\" = @Id", new { Id = id });
        }
    }
}
