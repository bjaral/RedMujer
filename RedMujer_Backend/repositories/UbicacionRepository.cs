using Dapper;
using Npgsql;
using RedMujer_Backend.models;
using RedMujer_Backend.DTOs;
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
        // MÉTODO NUEVO: CATEGORÍAS POR EMPRENDIMIENTO
        public async Task<IEnumerable<UbicacionDto>> GetUbicacionesPorEmprendimientoAsync(int id_Emprendimiento)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var query = @"
                    SELECT u.""id_ubicacion"" as Id_Ubicacion,
                        u.""id_comuna"" as Id_Comuna,
                        c.""id_region"" as Id_Region,
                        u.""calle"" as Calle,
                        u.""numero"" as Numero,
                        u.""referencia"" as Referencia,
                        u.""vigencia"" as Vigencia
                    FROM ""Emprendimiento_ubicacion"" eu
                    JOIN ""Ubicaciones"" u ON eu.""id_ubicacion"" = u.""id_ubicacion""
                    JOIN ""Comunas"" c ON u.""id_comuna"" = c.""id_comuna""
                    WHERE eu.""id_emprendimiento"" = @Id_Emprendimiento AND u.""vigencia"" = true
                ";
            return await connection.QueryAsync<UbicacionDto>(query, new { Id_Emprendimiento = id_Emprendimiento });
        }
    }
}
