using Dapper;
using Npgsql;
using RedMujer_Backend.models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly string _connectionString;

        public CategoriaRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        public async Task<IEnumerable<Categoria>> GetAllAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryAsync<Categoria>(
                "SELECT * FROM \"Categorias\" WHERE vigencia = true");
        }

        public async Task<Categoria?> GetByIdAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Categoria>(
                "SELECT * FROM \"Categorias\" WHERE \"id_categoria\" = @Id AND vigencia = true",
                new { Id = id });
        }

        public async Task CrearAsync(Categoria categoria)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                @"INSERT INTO ""Categorias"" 
                (""descripcion"", ""vigencia"", ""grupo_categoria"") 
                VALUES (@Descripcion, @Vigencia, @Grupo_Categoria)",
                categoria);
        }

        public async Task ActualizarAsync(Categoria categoria)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                @"UPDATE ""Categorias"" SET 
                    ""descripcion"" = @Descripcion,
                    ""vigencia"" = @Vigencia,
                    ""grupo_categoria"" = @Grupo_Categoria
                WHERE ""id_categoria"" = @Id_Categoria",
                categoria);
        }

        public async Task EliminarAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "UPDATE \"Categorias\" SET vigencia = false WHERE \"id_categoria\" = @Id_Categoria",
                new { Id_Categoria = id });
        }
    }
}
