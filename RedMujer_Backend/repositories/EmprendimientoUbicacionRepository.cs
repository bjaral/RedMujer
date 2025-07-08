using Dapper;
using Npgsql;
using RedMujer_Backend.models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.repositories
{
    public class EmprendimientoUbicacionRepository : IEmprendimientoUbicacionRepository
    {
        private readonly string _connectionString;

        public EmprendimientoUbicacionRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        public async Task<IEnumerable<EmprendimientoUbicacion>> GetAllAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryAsync<EmprendimientoUbicacion>(
                "SELECT * FROM \"Emprendimiento_ubicacion\"");
        }

        public async Task<EmprendimientoUbicacion?> GetByIdsAsync(int idEmprendimiento, int idUbicacion)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<EmprendimientoUbicacion>(
                @"SELECT * FROM ""Emprendimiento_ubicacion""
                  WHERE ""id_emprendimiento"" = @Id_Emprendimiento AND ""id_ubicacion"" = @Id_Ubicacion",
                new { IdEmprendimiento = idEmprendimiento, IdUbicacion = idUbicacion });
        }

        public async Task InsertAsync(EmprendimientoUbicacion eu)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                @"INSERT INTO ""Emprendimiento_ubicacion""
                (""id_emprendimiento"", ""id_ubicacion"")
                VALUES (@Id_Emprendimiento, @Id_Ubicacion)",
                eu);
        }

        public async Task DeleteAsync(int idEmprendimiento, int idUbicacion)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                @"DELETE FROM ""Emprendimiento_ubicacion""
                  WHERE ""id_emprendimiento"" = @Id_Emprendimiento AND ""id_ubicacion"" = @Id_Ubicacion",
                new { IdEmprendimiento = idEmprendimiento, IdUbicacion = idUbicacion });
        }
    }
}
