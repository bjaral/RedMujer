using Dapper;
using Npgsql;
using System.Data;
using RedMujer_Backend.models;

namespace RedMujer_Backend.repositories
{
    public class EmprendimientoRepository : IEmprendimientoRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public EmprendimientoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection")!;
        }

        private IDbConnection CreateConnection()
            => new NpgsqlConnection(_connectionString);

        public async Task<IEnumerable<Emprendimiento>> GetAllAsync()
        {
            const string query = "SELECT * FROM \"Emprendimientos\"";
            using var connection = CreateConnection();
            return await connection.QueryAsync<Emprendimiento>(query);
        }

        public async Task<IEnumerable<Emprendimiento>> GetRandomAsync(int cantidad)
        {
            const string query = "SELECT * FROM \"Emprendimientos\" ORDER BY RANDOM() LIMIT @cantidad";
            using var connection = CreateConnection();
            return await connection.QueryAsync<Emprendimiento>(query, new { cantidad });
        }
    }
}
