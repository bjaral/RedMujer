using Dapper;
using Npgsql;
using RedMujer_Backend.models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.repositories
{
    public class ComunaRepository : IComunaRepository
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;
        public ComunaRepository(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Comuna>> GetAllAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryAsync<Comuna>("SELECT * FROM \"Comunas\" WHERE \"Vigencia\" = true");
        }

            public async Task<Comuna?> GetByIdsAsync(int id_region, int id_comuna)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Comuna>(
                "SELECT * FROM \"Comunas\" WHERE \"id_region\" = @id_region AND \"id_comuna\" = @id_comuna AND \"vigencia\" = true",
                new { id_region, id_comuna }
            );
        }

        public async Task InsertAsync(Comuna comuna)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "INSERT INTO \"Comunas\" (\"id_region\", \"Nombre\", \"Vigencia\") VALUES (@id_region, @Nombre, @Vigencia)", comuna);
        }

        public async Task UpdateAsync(Comuna comuna)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "UPDATE \"Comunas\" SET \"id_region\" = @id_region, \"Nombre\" = @Nombre, \"Vigencia\" = @Vigencia WHERE \"id_comuna\" = @id_comuna", comuna);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "UPDATE \"Comunas\" SET \"Vigencia\" = false WHERE \"id_comuna\" = @Id", new { Id = id });
        }
    }
}
