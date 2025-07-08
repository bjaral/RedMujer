using Dapper;
using Npgsql;
using RedMujer_Backend.models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.repositories
{
    public class PersonaEmprendimientoRepository : IPersonaEmprendimientoRepository
    {
        private readonly string _connectionString;

        public PersonaEmprendimientoRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        public async Task<IEnumerable<PersonaEmprendimiento>> GetAllAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryAsync<PersonaEmprendimiento>(
                "SELECT * FROM \"Persona_emprendimiento\"");
        }

        public async Task<PersonaEmprendimiento?> GetByIdsAsync(int idPersona, int idEmprendimiento)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<PersonaEmprendimiento>(
                @"SELECT * FROM ""Persona_emprendimiento""
                  WHERE ""id_persona"" = @Id_Persona AND ""id_emprendimiento"" = @Id_Emprendimiento",
                new { IdPersona = idPersona, IdEmprendimiento = idEmprendimiento });
        }

        public async Task InsertAsync(PersonaEmprendimiento pe)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                @"INSERT INTO ""Persona_emprendimiento""
                (""id_persona"", ""id_emprendimiento"")
                VALUES (@Id_Persona, @Id_Emprendimiento)",
                pe);
        }

        public async Task DeleteAsync(int idPersona, int idEmprendimiento)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                @"DELETE FROM ""Persona_emprendimiento""
                  WHERE ""id_persona"" = @Id_Persona AND ""id_emprendimiento"" = @Id_Emprendimiento",
                new { IdPersona = idPersona, IdEmprendimiento = idEmprendimiento });
        }
    }
}
