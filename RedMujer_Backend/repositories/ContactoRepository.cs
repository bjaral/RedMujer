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
        private readonly IConfiguration _config;
        private readonly string? _connectionString;

        public ContactoRepository(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
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

        public async Task<int> InsertAsync(Contacto contacto)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var query = @"
                INSERT INTO ""Contactos"" 
                    (""id_emprendimiento"", ""valor"", ""vigencia"", ""tipo_contacto"") 
                VALUES 
                    (@Id_Emprendimiento, @Valor, @Vigencia, @Tipo_Contacto::tipo_contacto)
                RETURNING id_contacto;
            ";

            var id = await connection.ExecuteScalarAsync<int>(query, new
            {
                contacto.Id_Emprendimiento,
                contacto.Valor,
                contacto.Vigencia,
                Tipo_Contacto = contacto.Tipo_Contacto.ToString()
            });

            return id;
        }


        public async Task UpdateAsync(Contacto contacto)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                @"UPDATE ""Contactos"" SET 
                        ""id_emprendimiento"" = @Id_Emprendimiento,
                        ""valor"" = @Valor,
                        ""vigencia"" = @Vigencia,
                        ""tipo_contacto"" = @Tipo_Contacto::tipo_contacto
                WHERE 
                        ""id_contacto"" = @Id_Contacto",
                new
                {
                    contacto.Id_Emprendimiento,
                    contacto.Valor,
                    contacto.Vigencia,
                    Tipo_Contacto = contacto.Tipo_Contacto.ToString(),
                    contacto.Id_Contacto
                });
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
