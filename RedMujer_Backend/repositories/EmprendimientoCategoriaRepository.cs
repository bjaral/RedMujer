using Dapper;
using Npgsql;
using RedMujer_Backend.models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.repositories
{
    public class EmprendimientoCategoriaRepository : IEmprendimientoCategoriaRepository
    {
        private readonly string _connectionString;

        public EmprendimientoCategoriaRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        public async Task<IEnumerable<EmprendimientoCategoria>> GetAllAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryAsync<EmprendimientoCategoria>(
                "SELECT * FROM \"Emprendimiento_categoria\"");
        }

        public async Task<EmprendimientoCategoria?> GetByIdsAsync(int idEmprendimiento, int idCategoria)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<EmprendimientoCategoria>(
                @"SELECT * FROM ""Emprendimiento_categoria""
                  WHERE ""id_emprendimiento"" = @Id_Emprendimiento AND ""id_categoria"" = @Id_Categoria",
                new { IdEmprendimiento = idEmprendimiento, IdCategoria = idCategoria });
        }

        public async Task InsertAsync(EmprendimientoCategoria ec)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                @"INSERT INTO ""Emprendimiento_categoria""
                (""id_emprendimiento"", ""id_categoria"")
                VALUES (@Id_Emprendimiento, @Id_Categoria)",
                ec);
        }

        public async Task DeleteAsync(int idEmprendimiento, int idCategoria)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                @"DELETE FROM ""Emprendimiento_categoria""
                  WHERE ""id_emprendimiento"" = @Id_Emprendimiento AND ""id_categoria"" = @Id_Categoria",
                new { IdEmprendimiento = idEmprendimiento, IdCategoria = idCategoria });
        }
    }
}
