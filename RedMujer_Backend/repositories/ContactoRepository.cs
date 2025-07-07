using Dapper;
using Npgsql;
using RedMujer_Backend.models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.repositories
{
    public class ContactoRepository : IContactoRepository
    {
        private readonly string _connectionString;

        public ContactoRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        public async Task<IEnumerable<Contacto>> GetAllAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryAsync<Contacto>(
                "SELECT * FROM \"Contactos\" WHERE vigencia = true");
        }

        public async Task<Contacto?> GetByIdAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Contacto>(
                "SELECT * FROM \"Contactos\" WHERE \"id_contacto\" = @Id AND vigencia = true",
                new { Id = id });
        }

        public async Task InsertAsync(Contacto contacto)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                @"INSERT INTO ""Contactos"" 
                (""id_emprendimiento"", ""valor"", ""vigencia"", ""tipo_contacto"") 
                VALUES (@IdEmprendimiento, @Valor, @vigencia, @Tipo_Contacto)",
                contacto);
        }

        public async Task UpdateAsync(Contacto contacto)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                @"UPDATE ""Contactos"" SET 
                    ""id_emprendimiento"" = @IdEmprendimiento,
                    ""valor"" = @Valor,
                    ""vigencia"" = @vigencia,
                    ""tipo_contacto"" = @Tipo_Contacto
                WHERE ""id_contacto"" = @IdContacto",
                contacto);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "UPDATE \"Contactos\" SET vigencia = false WHERE \"id_contacto\" = @Id",
                new { Id = id });
        }
    }
}
