using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using RedMujer_Backend.models;

namespace RedMujer_Backend.repositories
{
    public class ComunaRepository : IComunaRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public ComunaRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection")!;
        }

        private IDbConnection CreateConnection()
            => new NpgsqlConnection(_connectionString);

        public async Task<IEnumerable<Comuna>> GetAllAsync()
        {
            const string query = "SELECT * FROM \"Comunas\"";
            using var connection = CreateConnection();
            var result = await connection.QueryAsync<Comuna>(query);
            return result.ToList();
        }

        public async Task<Comuna?> GetByIdsAsync(int idRegion, int idComuna)
        {
            const string query = "SELECT * FROM \"Comunas\" WHERE id_region = @Id_Region AND id_comuna = @Id_Comuna";
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Comuna>(query, new { Id_Region = idRegion, Id_Comuna = idComuna });
        }

        public async Task<Comuna?> GetByIdAsync(int idComuna)
        {
            const string query = "SELECT * FROM \"Comunas\" WHERE id_comuna = @Id_Comuna";
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Comuna>(query, new { Id_Comuna = idComuna });
        }

        public async Task<IEnumerable<Comuna>> ObtenerComunasPorRegionAsync(int idRegion)
        {
            const string query = "SELECT * FROM \"Comunas\" WHERE id_region = @Id_Region";
            using var connection = CreateConnection();
            var result = await connection.QueryAsync<Comuna>(query, new { Id_Region = idRegion });
            return result.ToList();
        }

        public async Task CrearAsync(Comuna comuna)
        {
            const string query = "INSERT INTO \"Comunas\" (id_region, nombre, vigencia) VALUES (@Id_Region, @Nombre, @Vigencia)";
            using var connection = CreateConnection();
            await connection.ExecuteAsync(query, comuna);
        }

        public async Task ActualizarAsync(Comuna comuna)
        {
            const string query = "UPDATE \"Comunas\" SET id_region = @Id_Region, nombre = @Nombre, vigencia = @Vigencia WHERE id_comuna = @Id_Comuna";
            using var connection = CreateConnection();
            await connection.ExecuteAsync(query, comuna);
        }

        public async Task EliminarAsync(int idComuna)
        {
            const string query = "DELETE FROM \"Comunas\" WHERE id_comuna = @Id_Comuna";
            using var connection = CreateConnection();
            await connection.ExecuteAsync(query, new { Id_Comuna = idComuna });
        }
    }
}
