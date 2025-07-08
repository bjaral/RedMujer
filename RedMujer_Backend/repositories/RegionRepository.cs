using Dapper;
using Npgsql;
using RedMujer_Backend.models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;

        public RegionRepository(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryAsync<Region>("SELECT * FROM \"Regiones\" WHERE vigencia = true");
        }

        public async Task<Region?> GetByIdAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Region>(
                "SELECT * FROM \"Regiones\" WHERE \"id_region\" = @Id AND vigencia = true", new { Id = id });
        }

        public async Task InsertAsync(Region region)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "INSERT INTO \"Regiones\" (\"Nombre\", vigencia) VALUES (@Nombre, @vigencia)",
                new { Nombre = region.Nombre, Vigencia = region.Vigencia });
        }

        public async Task UpdateAsync(Region region)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "UPDATE \"Regiones\" SET \"Nombre\" = @Nombre, vigencia = @vigencia WHERE \"id_region\" = @Id",
                new { Nombre = region.Nombre, Vigencia = region.Vigencia, Id = region.Id_Region });
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "UPDATE \"Regiones\" SET Vigencia = false WHERE \"id_region\" = @Id", new { Id = id });
        }
    }
}
