using Dapper;
using Npgsql;
using RedMujer_Backend.models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.repositories
{
    public class PersonaRepository : IPersonaRepository
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString = string.Empty;


        public PersonaRepository(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        public async Task<IEnumerable<Persona>> GetAllAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryAsync<Persona>(
                "SELECT * FROM \"Personas\" WHERE vigencia = true");
        }

        public async Task<Persona?> GetByIdAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Persona>(
                "SELECT * FROM \"Personas\" WHERE \"id_persona\" = @Id AND vigencia = true",
                new { Id = id });
        }

        public async Task<int> InsertAsync(Persona persona)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var id = await connection.ExecuteScalarAsync<int>(
                @"INSERT INTO ""Personas"" 
                    (""id_ubicacion"", ""id_usuario"", ""RUN"", ""nombre"", ""primer_apellido"", ""segundo_apellido"", ""vigencia"")
                  VALUES 
                    (@Id_Ubicacion, @Id_Usuario, @RUN, @Nombre, @PrimerApellido, @SegundoApellido, @Vigencia)
                  RETURNING ""id_persona"";",
                persona);

            persona.Id_Persona = id;
            return id;
        }



        public async Task UpdateAsync(Persona persona)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                @"UPDATE ""Personas"" SET 
                    ""id_ubicacion"" = @Id_Ubicacion,
                    ""id_usuario"" = @Id_Usuario,
                    ""RUN"" = @RUN,
                    ""nombre"" = @Nombre,
                    ""primer_apellido"" = @PrimerApellido,
                    ""segundo_apellido"" = @SegundoApellido,
                    ""vigencia"" = @vigencia
                WHERE ""id_persona"" = @Id_Persona",
                persona);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "UPDATE \"Personas\" SET vigencia = false WHERE \"id_persona\" = @Id",
                new { Id = id });
        }
    }
}
