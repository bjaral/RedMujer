using Dapper;
using Npgsql;
using RedMujer_Backend.models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.repositories
{
    public class PlataformaRepository : IPlataformaRepository
    {
        private readonly IConfiguration _config;
        private readonly string? _connectionString;

        public PlataformaRepository(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Plataforma>> GetAllAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryAsync<Plataforma>("SELECT * FROM \"Plataforma\" WHERE \"vigencia\" = true");
        }

        public async Task<Plataforma?> GetByIdAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Plataforma>(
                "SELECT * FROM \"Plataforma\" WHERE \"id_plataforma\" = @Id AND \"vigencia\" = true", new { Id = id }
            );
        }

        public async Task<int> InsertAsync(Plataforma plataforma)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var query = @"
                INSERT INTO ""Plataforma""
                    (""id_emprendimiento"", ""ruta"", ""descripcion"", ""vigencia"", ""tipo_plataforma"")
                VALUES
                    (@Id_Emprendimiento, @Ruta, @Descripcion, @Vigencia, @Tipo_Plataforma::tipo_plataforma)
                RETURNING id_plataforma;
            ";

            var id = await connection.ExecuteScalarAsync<int>(query, new
            {
                plataforma.Id_Emprendimiento,
                plataforma.Ruta,
                plataforma.Descripcion,
                plataforma.Vigencia,
                Tipo_Plataforma = plataforma.Tipo_Plataforma.ToString()
            });

            return id;
        }


        public async Task<int> UpdateAsync(Plataforma plataforma)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var query = @"
                UPDATE ""Plataforma""
                SET 
                    ""id_emprendimiento"" = @Id_Emprendimiento,
                    ""ruta"" = @Ruta,
                    ""descripcion"" = @Descripcion,
                    ""vigencia"" = @Vigencia,
                    ""tipo_plataforma"" = @Tipo_Plataforma::tipo_plataforma
                WHERE ""id_plataforma"" = @Id_Plataforma
                RETURNING id_plataforma;
            ";

            var id = await connection.QuerySingleAsync<int>(query, new
            {
                plataforma.Id_Emprendimiento,
                plataforma.Ruta,
                plataforma.Descripcion,
                plataforma.Vigencia,
                Tipo_Plataforma = plataforma.Tipo_Plataforma.ToString(),
                plataforma.Id_Plataforma
            });

            return id;
        }



        public async Task DeleteAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "UPDATE \"Plataforma\" SET \"vigencia\" = false WHERE \"id_plataforma\" = @Id", new { Id = id });
        }
    }
}
